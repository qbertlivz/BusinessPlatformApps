import { ActionResponse } from '../models/action-response';
import { PBIWorkspace } from '../models/pbi-workspace';

import { ViewModelBase } from '../services/view-model-base';

export class ProgressViewModel extends ViewModelBase {
    asDatabase: string = 'Sccm';
    datastoreEntriesToValidate: string[] = [];
    downloadPbiText: string = this.MS.Translate.PROGRESS_DOWNLOAD_PBIX_INFO;
    enablePublishReport: boolean = false;
    filename: string = 'report.pbix';
    filenameSSAS: string = 'reportSSAS.pbix';
    finishedActionName: string = '';
    hasPowerApp: boolean = false;
    isDataPullDone: boolean = false;
    isPbixReady: boolean = false;
    isPowerAppReady: boolean = false;
    isUninstall: boolean = false;
    oauthType: string = 'powerbi';
    pbiWorkspaces: PBIWorkspace[] = [];
    pbixDownloadLink: string = '';
    powerAppDownloadLink: string = '';
    powerAppFileName: string = '';
    publishReportLink: string = '';
    recordCounts: any[] = [];
    selectedPBIWorkspaceId: string = '';
    showCounts: boolean = false;
    showPBIWorkspaces: boolean = false;
    showPublishReport: boolean = false;
    sliceStatus: any[] = [];
    sqlServerIndex: number = 0;
    successMessage: string = this.MS.Translate.PROGRESS_ALL_DONE;
    successMessage2: string = this.MS.Translate.PROGRESS_ALL_DONE2;
    targetSchema: string = '';

    async executeActions(): Promise<void> {
        //if (await this.MS.DeploymentService.executeActions(this.showCounts) && !this.isUninstall) {
        if (await this.MS.DeploymentService.executeActions() && !this.isUninstall) {
            await this.wrangle();

            this.publishReportLink = await this.MS.HttpService.getResponseAsync('Microsoft-PublishPBIReport', {
                PBIWorkspaceId: this.selectedPBIWorkspaceId,
                PBIXLocation: this.pbixDownloadLink
            });
        }
    }

    async onLoaded(): Promise<void> {
        if (this.MS.UtilityService.getItem('queryUrl')) {
            this.MS.UtilityService.getToken(this.oauthType, async () => {
                this.MS.DeploymentService.isFinished = true;

                await this.wrangle();

                this.isDataPullDone = true;

                if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-GetPBIClusterUri')) {
                    this.pbiWorkspaces = await this.MS.HttpService.getResponseAsync('Microsoft-GetPBIWorkspaces');
                    this.showPBIWorkspaces = true;
                }
            });
        } else {
            let isDataStoreValid: boolean = true;

            for (let i = 0; i < this.datastoreEntriesToValidate.length && isDataStoreValid; i++) {
                if (this.MS.DataStore.getValue(this.datastoreEntriesToValidate[i]) === null) {
                    this.MS.NavigationService.navigateHome();
                    isDataStoreValid = false;
                }
            }

            if (isDataStoreValid) {
                this.executeActions();
            }
        }
    }

    async publishReport(): Promise<void> {
        this.MS.UtilityService.connectToAzure(this.oauthType);
    }

    async wrangle(): Promise<void> {
        let response: ActionResponse = null;

        response = await this.MS.HttpService.executeAsync('Microsoft-WranglePBI', { FileName: this.filename, SqlServerIndex: this.sqlServerIndex });
        
        if (response.IsSuccess) {
            this.pbixDownloadLink = response.Body.value;
            this.isPbixReady = true;
        }

        if (this.hasPowerApp) {
            let powerAppUri: string = this.MS.DataStore.getValue('PowerAppUri');

            if (powerAppUri) {
                this.isPowerAppReady = true;
                this.powerAppDownloadLink = powerAppUri;
            } else {
                this.hasPowerApp = false;
            }
        }
    }
}