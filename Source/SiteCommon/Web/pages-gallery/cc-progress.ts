import { ActionResponse } from '../models/action-response';
import { ViewModelBase } from '../services/view-model-base';

export class ProgressViewModel extends ViewModelBase {
    datastoreEntriesToValidate: string[] = [];
    downloadPbiText: string = this.MS.Translate.PROGRESS_DOWNLOAD_PBIX_INFO;
    enablePublishReport: boolean = false;
    filename: string = 'report.pbix';
    finishedActionName: string = '';
    hasPowerApp: boolean = false;
    isDataPullDone: boolean = false;
    isPbixReady: boolean = false;
    isPowerAppReady: boolean = false;
    isUninstall: boolean = false;
    oauthType: string = 'powerbi';
    pbixDownloadLink: string = '';
    powerAppDownloadLink: string = '';
    powerAppFileName: string = '';
    publishReportLink: string = '';
    recordCounts: any[] = [];
    redirectInSameTab: boolean = false;
    selectedPBIWorkspaceId: string = '';
    showCounts: boolean = false;
    showPublishReport: boolean = false;
    showDownloadButton: boolean = true;
    showBackButton: boolean = true;
    sliceStatus: any[] = [];
    successMessage: string = this.MS.Translate.PROGRESS_ALL_DONE;
    successMessage2: string = this.MS.Translate.PROGRESS_ALL_DONE2;
    targetSchema: string = '';

    async executeActions(): Promise<void> {
        if (await this.MS.DeploymentService.executeActions() && !this.isUninstall) {
            this.MS.DeploymentService.isFinished = true;
            await this.wrangle();

            let response: string = await this.MS.HttpService.getResponseAsync('Microsoft-PublishPBIReportCDSA', {
                PBIWorkspaceId: this.selectedPBIWorkspaceId,
                PBIXLocation: this.pbixDownloadLink
            });

            await this.MS.HttpService.getResponseAsync('Microsoft-UpdatePBIParameters');

            this.publishReportLink = response;
            if (!this.showBackButton)
                this.showBackButtonOnFinalPage = false;
        }
    }

    async onLoaded(): Promise<void> {
        this.publishReportLink = '';
        this.MS.DeploymentService.isFinished = false;

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

    async publishReport(): Promise<void> {
        this.MS.UtilityService.connectToAzure(this.oauthType);
    }

    async wrangle(): Promise<void> {
        let response: ActionResponse = null;

        response = await this.MS.HttpService.executeAsync('Microsoft-WranglePBI', { FileName: this.filename });
        
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