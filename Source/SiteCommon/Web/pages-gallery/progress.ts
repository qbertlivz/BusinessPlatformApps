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

    async clickSelectWorkspace(): Promise<void> {
        this.showPBIWorkspaces = false;

        this.publishReportLink = await this.MS.HttpService.getResponseAsync('Microsoft-PublishPBIReport', {
            PBIWorkspaceId: this.selectedPBIWorkspaceId,
            PBIXLocation: this.pbixDownloadLink
        });
    }

    async executeActions(): Promise<void> {
        if (await this.MS.DeploymentService.executeActions() && !this.isUninstall) {
            await this.wrangle();

            this.queryRecordCounts();
        }
    }

    hidePBIWorkspaces(): void {
        this.showPBIWorkspaces = false;
        this.showPublishReport = this.enablePublishReport;
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
        } else if (this.MS.DataStore.getValue('HasNavigated') === null) {
            this.MS.NavigationService.navigateHome();
        } else {
            let isDataStoreValid: boolean = true;

            for (let i = 0; i < this.datastoreEntriesToValidate.length && isDataStoreValid; i++) {
                if (this.MS.DataStore.getValue(this.datastoreEntriesToValidate[i]) === null) {
                    this.MS.NavigationService.navigateHome();
                    isDataStoreValid = false;
                }
            }

            if (isDataStoreValid) {
                //this.MS.DeploymentService.isFinished = true;
                //await this.wrangle();
                //this.isDataPullDone = true;
                //this.showPublishReport = this.enablePublishReport;
                this.executeActions();
            }
        }
    }

    async publishReport(): Promise<void> {
        this.MS.UtilityService.connectToAzure(this.oauthType);
    }

    async queryRecordCounts(): Promise<void> {
        if (this.showCounts && !this.isDataPullDone && !this.MS.DeploymentService.hasError) {
            let response = await this.MS.HttpService.executeAsync('Microsoft-GetDataPullStatus', {
                FinishedActionName: this.finishedActionName,
                IsWaiting: false,
                SqlServerIndex: this.sqlServerIndex,
                TargetSchema: this.targetSchema
            });
            if (response.IsSuccess) {
                this.recordCounts = response.Body.status;
                this.sliceStatus = response.Body.slices;
                this.isDataPullDone = response.Body.isFinished;
                this.queryRecordCounts();
            } else {
                this.MS.DeploymentService.hasError = true;
            }
        } else {
            this.showPublishReport = this.enablePublishReport;
        }
    }

    async wrangle(): Promise<void> {
        let ssas = this.MS.DataStore.getValue('ssasDisabled');
        let response: ActionResponse = null;
        if (ssas && ssas === 'false') {
            response = await this.MS.HttpService.executeAsync('Microsoft-WranglePBISSAS', { ASDatabase: this.asDatabase, FileNameSSAS: this.filenameSSAS });
        } else {
            response = await this.MS.HttpService.executeAsync('Microsoft-WranglePBI', { FileName: this.filename, SqlServerIndex: this.sqlServerIndex });
        }
        if (response.IsSuccess) {
            this.pbixDownloadLink = response.Body.value;
            this.isPbixReady = true;
        }

        if (this.hasPowerApp) {
            let responsePowerApp = await this.MS.HttpService.executeAsync('Microsoft-WranglePowerApp', { PowerAppFileName: this.powerAppFileName });

            if (responsePowerApp.IsSuccess && responsePowerApp.Body.value) {
                this.isPowerAppReady = true;
                this.powerAppDownloadLink = responsePowerApp.Body.value;
            } else {
                this.hasPowerApp = false;
            }
        }
    }
}