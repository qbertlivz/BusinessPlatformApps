SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

-- Must be executed inside the target database
-- Views
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthClientInstalledVersionDetails' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthClientInstalledVersionDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthClientsInstallationIssuesLast60day' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthClientsInstallationIssuesLast60day;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthClientsinventorystatisticslast30days' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthClientsinventorystatisticslast30days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthCountAssignedClientsBySite' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthCountAssignedClientsBySite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthCountClientbyMP' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthCountClientbyMP;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthCountClientsInstalledVersion' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthCountClientsInstalledVersion;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthCountClientsInstalledVersionPerSite' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthCountClientsInstalledVersionPerSite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthEvaluationCount' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthEvaluationCount;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthSummary' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthSummary;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ClientHealthWUAVersionAllClients' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ClientHealthWUAVersionAllClients;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_collection' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_collection;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingAllBaselineCompliance' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingAllBaselineCompliance;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingComplianceByComputers' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingComplianceByComputers;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingComplianceCIForAllBaseline' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingComplianceCIForAllBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingComplianceStateForSpecificBaseline' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingComplianceStateForSpecificBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingComputersComplianceForAllCI' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingComputersComplianceForAllCI;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComplianceSettingsAllBaselineNames' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComplianceSettingsAllBaselineNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ComponentStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ComponentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_computer' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_computer;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_computercollection' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_computercollection;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_computermalware' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_computermalware;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_computerprogram' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_computerprogram;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_computerupdate' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_computerupdate;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ConfigMgrServerHealthBackupStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ConfigMgrServerHealthBackupStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ConfigMgrServerHealthComponentStatusDetails' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ConfigMgrServerHealthComponentStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ConfigMgrServerHealthCountComponentStatusMessagesBySeverity' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ConfigMgrServerHealthCountComponentStatusMessagesBySeverity;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ConfigMgrServerHealthReplicationStatusDetails' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ConfigMgrServerHealthReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_configuration' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_configuration;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_date' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_date;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPoints' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPoints;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsAllDPs' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsAllDPs;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsCompliancebyDPStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsCompliancebyDPStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsCompliancebyPackagesStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsCompliancebyPackagesStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsConfigurationStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsConfigurationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsRateLimiteSchedules' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsRateLimiteSchedules;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_DistributionPointsStatusContentStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_DistributionPointsStatusContentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_OverallComplianceBaseline' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_OverallComplianceBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_program' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_program;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ReplicationStatus' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ReplicationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ReplicationStatusDetails' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_ReplicationStatusReplicationGroup' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_ReplicationStatusReplicationGroup;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_scanhistory' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_scanhistory;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_SoftwareDistribution' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_SoftwareDistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_SoftwareDistributionAllPackageIDs' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_SoftwareDistributionAllPackageIDs;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_SoftwareDistributionOverallApplicationDeployment' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_SoftwareDistributionOverallApplicationDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_SoftwareDistributionOverallPackageDeployment' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_SoftwareDistributionOverallPackageDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_SoftwareDistributionOverallPckAPPDPdistribution' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_SoftwareDistributionOverallPckAPPDPdistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TaskSequenceAllTaskSequenceIDs' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TaskSequenceAllTaskSequenceIDs;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TaskSequenceDeployment' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TasksequenceDPContentStatusReferencedPackagesTaskSequence' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TasksequenceDPContentStatusReferencedPackagesTaskSequence;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TasksequenceOverallTaskSequenceByComputerDeployment' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TasksequenceOverallTaskSequenceByComputerDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TasksequenceOverallTaskSequencebyTaskSequenceDeployment' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TasksequenceOverallTaskSequencebyTaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_TasksequenceProgressofTaskSequenceDeploymentLast7days' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_TasksequenceProgressofTaskSequenceDeploymentLast7days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_update' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_update;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_user' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_user;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='vw_usercomputer' AND TABLE_TYPE='VIEW')
  DROP VIEW pbist_sccm.vw_usercomputer;

-- Tables
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientInstalledVersionDetails' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientInstalledVersionDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientInstalledVersionDetails_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientInstalledVersionDetails_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientsInstallationIssuesLast60day' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientsInstallationIssuesLast60day;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientsInstallationIssuesLast60day_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientsInstallationIssuesLast60day_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientsinventorystatisticslast30days' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientsinventorystatisticslast30days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthClientsinventorystatisticslast30days_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthClientsinventorystatisticslast30days_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountAssignedClientsBySite' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountAssignedClientsBySite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountAssignedClientsBySite_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountAssignedClientsBySite_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientbyMP' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientbyMP;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientbyMP_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientbyMP_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientsInstalledVersion' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientsInstalledVersion;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientsInstalledVersion_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientsInstalledVersion_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientsInstalledVersionPerSite' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientsInstalledVersionPerSite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthCountClientsInstalledVersionPerSite_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthCountClientsInstalledVersionPerSite_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthEvaluationCount' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthEvaluationCount;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthEvaluationCount_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthEvaluationCount_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthSummary' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthSummary;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthSummary_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthSummary_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthWUAVersionAllClients' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthWUAVersionAllClients;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ClientHealthWUAVersionAllClients_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ClientHealthWUAVersionAllClients_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='collection' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[collection];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='collection_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.collection_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingAllBaselineCompliance' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingAllBaselineCompliance;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingAllBaselineCompliance_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingAllBaselineCompliance_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceByComputers' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceByComputers;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceByComputers_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceByComputers_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceCIForAllBaseline' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceCIForAllBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceCIForAllBaseline_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceCIForAllBaseline_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceStateForSpecificBaseline' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceStateForSpecificBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComplianceStateForSpecificBaseline_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComplianceStateForSpecificBaseline_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComputersComplianceForAllCI' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComputersComplianceForAllCI;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComplianceSettingComputersComplianceForAllCI_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComplianceSettingComputersComplianceForAllCI_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComponentStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComponentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ComponentStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ComponentStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computer' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computer;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computer_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computer_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computercollection' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computercollection;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computercollection_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computercollection_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computermalware' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computermalware;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computermalware_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computermalware_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computerprogram' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computerprogram;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computerprogram_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computerprogram_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computerupdate' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computerupdate;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='computerupdate_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.computerupdate_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthBackupStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthBackupStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthBackupStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthBackupStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthComponentStatusDetails' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthComponentStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthComponentStatusDetails_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthComponentStatusDetails_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthCountComponentStatusMessagesBySeverity' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthCountComponentStatusMessagesBySeverity;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthCountComponentStatusMessagesBySeverity_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthCountComponentStatusMessagesBySeverity_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthReplicationStatusDetails' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ConfigMgrServerHealthReplicationStatusDetails_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ConfigMgrServerHealthReplicationStatusDetails_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='configuration' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[configuration];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='date' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[date];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPoints' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPoints;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPoints_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPoints_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsCompliancebyDPStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsCompliancebyDPStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsCompliancebyDPStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsCompliancebyDPStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsCompliancebyPackagesStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsCompliancebyPackagesStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsCompliancebyPackagesStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsCompliancebyPackagesStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsConfigurationStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsConfigurationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsConfigurationStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsConfigurationStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsRateLimiteSchedules' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsRateLimiteSchedules;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsRateLimiteSchedules_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsRateLimiteSchedules_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsStatusContentStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsStatusContentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='DistributionPointsStatusContentStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.DistributionPointsStatusContentStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='malware' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.malware;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='malware_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.malware_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='operatingsystem' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.operatingsystem;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='OverallComplianceBaseline' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.OverallComplianceBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='OverallComplianceBaseline_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.OverallComplianceBaseline_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='program' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.program;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='program_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.program_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatus' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatus_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatus_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatusDetails' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatusDetails_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatusDetails_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatusReplicationGroup' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatusReplicationGroup;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='ReplicationStatusReplicationGroup_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.ReplicationStatusReplicationGroup_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='scanhistory' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.scanhistory;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='scanhistory_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.scanhistory_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='site' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[site];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='site_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.site_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistribution' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistribution_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistribution_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallApplicationDeployment' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallApplicationDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallApplicationDeployment_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallApplicationDeployment_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallPackageDeployment' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallPackageDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallPackageDeployment_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallPackageDeployment_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallPckAPPDPdistribution' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallPckAPPDPdistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='SoftwareDistributionOverallPckAPPDPdistribution_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.SoftwareDistributionOverallPckAPPDPdistribution_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TaskSequenceDeployment' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TaskSequenceDeployment_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TaskSequenceDeployment_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceDPContentStatusReferencedPackagesTaskSequence' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceDPContentStatusReferencedPackagesTaskSequence;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceDPContentStatusReferencedPackagesTaskSequence_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceDPContentStatusReferencedPackagesTaskSequence_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceOverallTaskSequenceByComputerDeployment' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceOverallTaskSequenceByComputerDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceOverallTaskSequenceByComputerDeployment_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceOverallTaskSequenceByComputerDeployment_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceOverallTaskSequencebyTaskSequenceDeployment' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceOverallTaskSequencebyTaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceOverallTaskSequencebyTaskSequenceDeployment_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceOverallTaskSequencebyTaskSequenceDeployment_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceProgressofTaskSequenceDeploymentLast7days' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceProgressofTaskSequenceDeploymentLast7days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='TasksequenceProgressofTaskSequenceDeploymentLast7days_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.TasksequenceProgressofTaskSequenceDeploymentLast7days_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='update' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[update];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='update_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.update_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='user' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.[user];
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='user_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.user_staging;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='usercomputer' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.usercomputer;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='pbist_sccm' AND TABLE_NAME='usercomputer_staging' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE pbist_sccm.usercomputer_staging;

-- Stored procedures
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_get_last_updatetime' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_get_last_updatetime;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_get_prior_content' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_get_prior_content;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_get_replication_counts' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_get_replication_counts;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthClientInstalledVersionDetails' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthClientInstalledVersionDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthClientsInstallationIssuesLast60day' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthClientsInstallationIssuesLast60day;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthClientsinventorystatisticslast30days' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthClientsinventorystatisticslast30days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthCountAssignedClientsBySite' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthCountAssignedClientsBySite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthCountClientbyMP' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthCountClientbyMP;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthCountClientsInstalledVersion' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthCountClientsInstalledVersion;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthCountClientsInstalledVersionPerSite' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthCountClientsInstalledVersionPerSite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthEvaluationCount' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthEvaluationCount;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthSummary' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthSummary;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ClientHealthWUAVersionAllClients' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ClientHealthWUAVersionAllClients;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComplianceSettingAllBaselineCompliance' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComplianceSettingAllBaselineCompliance;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComplianceSettingComplianceByComputers' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComplianceSettingComplianceByComputers;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComplianceSettingComplianceCIForAllBaseline' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComplianceSettingComplianceCIForAllBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComplianceSettingComplianceStateForSpecificBaseline' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComplianceSettingComplianceStateForSpecificBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComplianceSettingComputersComplianceForAllCI' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComplianceSettingComputersComplianceForAllCI;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ComponentStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ComponentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ConfigMgrServerHealthBackupStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ConfigMgrServerHealthBackupStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ConfigMgrServerHealthComponentStatusDetails' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ConfigMgrServerHealthComponentStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ConfigMgrServerHealthCountComponentStatusMessagesBySeverity' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ConfigMgrServerHealthCountComponentStatusMessagesBySeverity;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ConfigMgrServerHealthReplicationStatusDetails' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ConfigMgrServerHealthReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPoints' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPoints;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsCompliancebyDPStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsCompliancebyDPStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsCompliancebyPackagesStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsCompliancebyPackagesStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsConfigurationStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsConfigurationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsRateLimiteSchedules' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsRateLimiteSchedules;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_DistributionPointsStatusContentStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_DistributionPointsStatusContentStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_OverallComplianceBaseline' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_OverallComplianceBaseline;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ReplicationStatus' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ReplicationStatus;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ReplicationStatusDetails' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ReplicationStatusDetails;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_ReplicationStatusReplicationGroup' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_ReplicationStatusReplicationGroup;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_SoftwareDistribution' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_SoftwareDistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_SoftwareDistributionOverallApplicationDeployment' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_SoftwareDistributionOverallApplicationDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_SoftwareDistributionOverallPackageDeployment' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_SoftwareDistributionOverallPackageDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_SoftwareDistributionOverallPckAPPDPdistribution' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_SoftwareDistributionOverallPckAPPDPdistribution;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_TaskSequenceDeployment' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_TaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_TasksequenceDPContentStatusReferencedPackagesTaskSequence' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_TasksequenceDPContentStatusReferencedPackagesTaskSequence;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_TasksequenceOverallTaskSequenceByComputerDeployment' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_TasksequenceOverallTaskSequenceByComputerDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_TasksequenceOverallTaskSequencebyTaskSequenceDeployment' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_TasksequenceOverallTaskSequencebyTaskSequenceDeployment;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populate_TasksequenceProgressofTaskSequenceDeploymentLast7days' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populate_TasksequenceProgressofTaskSequenceDeploymentLast7days;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecollection' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecollection;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecomputer' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecomputer;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecomputercollection' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecomputercollection;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecomputermalware' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecomputermalware;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecomputerprogram' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecomputerprogram;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatecomputerupdate' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatecomputerupdate;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatemalware' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatemalware;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populateprogram' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populateprogram;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatescanhistory' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatescanhistory;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populatesite' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populatesite;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populateupdate' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populateupdate;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populateuser' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populateuser;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='pbist_sccm' AND ROUTINE_NAME='sp_populateusercomputer' AND ROUTINE_TYPE='PROCEDURE')
  DROP PROCEDURE pbist_sccm.sp_populateusercomputer;

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name='pbist_sccm')
BEGIN
    EXEC ('CREATE SCHEMA pbist_sccm AUTHORIZATION dbo'); -- Avoid batch error
END;
