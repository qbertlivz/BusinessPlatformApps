param( [string]$TargetName          = "pbi_sccm",
       [string]$SourceServer,
       [string]$SourceDatabase,
       [string]$DestinationServer,
       [string]$DestinationDatabase
)

$sql_uid=""
$sql_pwd=""

# Make sure we execute the newest (or at least the ones installed by us)
$bcp = "E:\App\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\bcp.exe"
$sqlcmd = "E:\App\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\sqlcmd.exe"

#region Win32 credential manager
# This region reuses code published at https://gist.github.com/toburger/2947424, under:
# 
# The MIT License
# 
# Copyright (c) 2012 Tobias Burger
# 
# Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
# to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
# and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
# The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
# 
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
# WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

$sig = @"
[DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

[DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
public static extern bool CredFree([In] IntPtr cred);

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NativeCredential
{
    public UInt32 Flags;
    public CRED_TYPE Type;
    public IntPtr TargetName;
    public IntPtr Comment;
    public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
    public UInt32 CredentialBlobSize;
    public IntPtr CredentialBlob;
    public UInt32 Persist;
    public UInt32 AttributeCount;
    public IntPtr Attributes;
    public IntPtr TargetAlias;
    public IntPtr UserName;

    internal static NativeCredential GetNativeCredential(Credential cred)
    {
        NativeCredential ncred = new NativeCredential();
        ncred.AttributeCount = 0;
        ncred.Attributes = IntPtr.Zero;
        ncred.Comment = IntPtr.Zero;
        ncred.TargetAlias = IntPtr.Zero;
        ncred.Type = CRED_TYPE.GENERIC;
        ncred.Persist = (UInt32)1;
        ncred.CredentialBlobSize = (UInt32)cred.CredentialBlobSize;
        ncred.TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName);
        ncred.CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob);
        ncred.UserName = Marshal.StringToCoTaskMemUni(System.Environment.UserName);
        return ncred;
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct Credential
{
    public UInt32 Flags;
    public CRED_TYPE Type;
    public string TargetName;
    public string Comment;
    public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
    public UInt32 CredentialBlobSize;
    public string CredentialBlob;
    public UInt32 Persist;
    public UInt32 AttributeCount;
    public IntPtr Attributes;
    public string TargetAlias;
    public string UserName;
}

public enum CRED_TYPE : uint
{
    GENERIC = 1,
    DOMAIN_PASSWORD = 2,
    DOMAIN_CERTIFICATE = 3,
    DOMAIN_VISIBLE_PASSWORD = 4,
    GENERIC_CERTIFICATE = 5,
    DOMAIN_EXTENDED = 6,
    MAXIMUM = 7,      // Maximum supported cred type
    MAXIMUM_EX = (MAXIMUM + 1000),  // Allow new applications to run on old OSes
}

public class CriticalCredentialHandle : Microsoft.Win32.SafeHandles.CriticalHandleZeroOrMinusOneIsInvalid
{
    public CriticalCredentialHandle(IntPtr preexistingHandle)
    {
        SetHandle(preexistingHandle);
    }

    public Credential GetCredential()
    {
        if (!IsInvalid)
        {
            NativeCredential ncred = (NativeCredential)Marshal.PtrToStructure(handle, typeof(NativeCredential));
            Credential cred = new Credential();
            cred.CredentialBlobSize = ncred.CredentialBlobSize;
            cred.CredentialBlob = Marshal.PtrToStringUni(ncred.CredentialBlob, (int)ncred.CredentialBlobSize / 2);
            cred.UserName = Marshal.PtrToStringUni(ncred.UserName);
            cred.TargetName = Marshal.PtrToStringUni(ncred.TargetName);
            cred.TargetAlias = Marshal.PtrToStringUni(ncred.TargetAlias);
            cred.Type = ncred.Type;
            cred.Flags = ncred.Flags;
            cred.Persist = ncred.Persist;
            return cred;
        }
        else
        {
            throw new InvalidOperationException("Invalid CriticalHandle!");
        }
    }

    override protected bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            CredFree(handle);
            SetHandleAsInvalid();
            return true;
        }
        return false;
    }
}
"@
Add-Type -MemberDefinition $sig -Namespace "ADVAPI32" -Name 'Util'
#endregion

$nCredPtr= New-Object IntPtr
$success = [ADVAPI32.Util]::CredRead($TargetName, 1, 0, [ref] $nCredPtr)

if ($success)
{
    $critCred = New-Object ADVAPI32.Util+CriticalCredentialHandle $nCredPtr
    try
    {
        $cred = $critCred.GetCredential()
        $sql_uid = $cred.UserName
        $sql_pwd = $cred.CredentialBlob
        $cred = $null
    }
    catch
    {
        Write-Information "Credential Manager target: $TargetName is not defined or empty"
        Write-Information "Target server will use integrated authentication"
    }
}
else
{
    Write-Information "No credentials were found in Windows Credential Manager for TargetName: $TargetName"
    Write-Information "Target server will use integrated authentication"
}


# Create Logs folder
New-Item Logs -ItemType Directory -ErrorAction Ignore

# Map of query files and destination tables
$file2table = [ordered]@{ 
							"site.sql"                = "pbist_sccm.site_staging";
							"update.sql"              = "pbist_sccm.update_staging";
							"user.sql"                = "pbist_sccm.user_staging";
							"usercomputer.sql"        = "pbist_sccm.usercomputer_staging";
							"computermalware.sql"     = "pbist_sccm.computermalware_staging";
							"computer.sql"            = "pbist_sccm.computer_staging";                      
							"malware.sql"             = "pbist_sccm.malware_staging";
							"scanhistory.sql"         = "pbist_sccm.scanhistory_staging";
							"program.sql"             = "pbist_sccm.program_staging";
							"computerupdate.sql"      = "pbist_sccm.computerupdate_staging";                      
							"computerprogram.sql"     = "pbist_sccm.computerprogram_staging";
							"collection.sql"          = "pbist_sccm.collection_staging";
							"computercollection.sql"  = "pbist_sccm.computercollection_staging";
							"MAIN_ClientHealth_Summary.sql"												= "pbist_sccm.ClientHealthSummary_staging";
							"MAIN_Component_Status.sql"													= "pbist_sccm.ComponentStatus_staging";
							"MAIN_DistributionPoints_Status.sql"										= "pbist_sccm.DistributionPointsStatus_staging";
							"MAIN_OverallComplianceBaseline.sql"										= "pbist_sccm.OverallComplianceBaseline_staging";
							"MAIN_Replication_Status.sql"												= "pbist_sccm.ReplicationStatus_staging";
							"MAIN_Software_Distribution.sql"											= "pbist_sccm.SoftwareDistribution_staging";
							"MAIN_Task_Sequence_Deployment.sql"											= "pbist_sccm.TaskSequenceDeployment_staging";
							"Main_SCEP_Definition.sql"													= "pbist_sccm.SCEPDefinition_staging";
							"ClientHealth_client_installed_version_details.sql"							= "pbist_sccm.ClientHealthClientInstalledVersionDetails_staging";
							"ClientHealth_clients_inventory_statistics_last_30days.sql"					= "pbist_sccm.ClientHealthClientsinventorystatisticslast30days_staging";
							"ClientHealth_count_assigned_clients_by_site.sql"							= "pbist_sccm.ClientHealthCountAssignedClientsBySite_staging";
							"ClientHealth_count_clients_installed_version.sql"							= "pbist_sccm.ClientHealthCountClientsInstalledVersion_staging";
							"ClientHealth_count_clients_installed_version_per_site.sql"					= "pbist_sccm.ClientHealthCountClientsInstalledVersionPerSite_staging";
							"ClientHealth_evaluation_count.sql"											= "pbist_sccm.ClientHealthEvaluationCount_staging";
							"ClientHealth_WUA_version_all_clients.sql"									= "pbist_sccm.ClientHealthWUAVersionAllClients_staging";
							"ComplianceSetting_compliance_by_computers.sql"								= "pbist_sccm.ComplianceSettingComplianceByComputers_staging";
							"ComplianceSetting_Compliance_CI_for_all_Baseline.sql"						= "pbist_sccm.ComplianceSettingComplianceCIForAllBaseline_staging";
							"ConfigMgrServerHealth_Backup_Status.sql"									= "pbist_sccm.ConfigMgrServerHealthBackupStatus_staging";
							"ConfigMgrServerHealth_Component_Status_details.sql"						= "pbist_sccm.ConfigMgrServerHealthComponentStatusDetails_staging";
							"ConfigMgrServerHealth_Count_Component_Status_messages_by_severity.sql"		= "pbist_sccm.ConfigMgrServerHealthCountComponentStatusMessagesBySeverity_staging";
							"DistributionPoints_Compliance_by_DP_Status.sql"							= "pbist_sccm.DistributionPointsCompliancebyDPStatus_staging";
							"DistributionPoints_Compliance_by_Packages_Status.sql"						= "pbist_sccm.DistributionPointsCompliancebyPackagesStatus_staging";
							"DistributionPoints_Configuration_Status.sql"								= "pbist_sccm.DistributionPointsConfigurationStatus_staging";
							"Replication_Status_ReplicationGroup.sql"									= "pbist_sccm.ReplicationStatusReplicationGroup_staging";
							"Replication_Status_Details.sql"											= "pbist_sccm.ReplicationStatusDetails_staging";
							"Software_Distribution_OverallApplicationDeployment.sql"					= "pbist_sccm.SoftwareDistributionOverallApplicationDeployment_staging";
							"Software_Distribution_OverallPackageDeployment.sql"						= "pbist_sccm.SoftwareDistributionOverallPackageDeployment_staging";
							"Software_Distribution_OverallPckAPP_DP_distribution.sql"					= "pbist_sccm.SoftwareDistributionOverallPckAPPDPdistribution_staging";
							"Task_sequence_DP_Content_Status_referenced_packages_TaskSequence.sql"		= "pbist_sccm.TasksequenceDPContentStatusReferencedPackagesTaskSequence_staging";
							"Task_sequence_OverallTasksequence_by_computer_Deployment.sql"				= "pbist_sccm.TasksequenceOverallTaskSequenceByComputerDeployment_staging";
							"Task_sequence_OverallTasksequence_by_TaskSequence_Deployment.sql"			= "pbist_sccm.TasksequenceOverallTaskSequencebyTaskSequenceDeployment_staging";
						};

foreach ($k in $file2table.Keys)
{
    $f = $file2table[$k]
    "`nStarting processing for $f"
    "`tStartDateTime: " + (Get-Date)

    .\azurebcp.exe -b 20000 --sourceServer $SourceServer --sourceDatabase $SourceDatabase --queryFile $k --targetServer $DestinationServer --targetDatabase $DestinationDatabase --targetTable $f --si --credMan $TargetName

    if ($LASTEXITCODE -ne 0)
    {
        "Error copying data!!!"
        exit $LASTEXITCODE
    }

    "`tEndDateTime: " + (Get-Date)
}

if ( [string]::IsNullOrEmpty($sql_uid) )
{
    & $sqlcmd -I -a 32767 -S $DestinationServer -d $DestinationDatabase -E -i "process data.sql"
} else
{
    & $sqlcmd -I -a 32767 -S $DestinationServer -d $DestinationDatabase -U $sql_uid -P $sql_pwd -i "process data.sql"
}
