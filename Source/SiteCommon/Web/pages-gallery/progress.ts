import { QueryParameter } from '../constants/query-parameter';

import { ActionResponse } from '../models/action-response';

import { ViewModelBase } from '../services/view-model-base';

export class ProgressViewModel extends ViewModelBase {
    aadTenant: string = 'common';
    datastoreEntriesToValidate: string[] = [];
    downloadPbiText: string = this.MS.Translate.PROGRESS_DOWNLOAD_PBIX_INFO;
    enablePublishReport: boolean = false;
    filename: string = 'report.pbix';
    filenameSSAS: string = 'reportSSAS.pbix';
    asDatabase: string = 'Sccm';
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
    recordCounts: any[] = [];
    showCounts: boolean = false;
    sliceStatus: any[] = [];
    sqlServerIndex: number = 0;
    successMessage: string = this.MS.Translate.PROGRESS_ALL_DONE;
    successMessage2: string = this.MS.Translate.PROGRESS_ALL_DONE2;
    targetSchema: string = '';

    async publishReport(): Promise<void> {
        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureAuthUri', { AADTenant: this.aadTenant, oauthType: this.oauthType });
        window.location.href = response.Body.value;
    }

    async OnLoaded(): Promise<void> {
        let queryParam: any = this.MS.UtilityService.GetItem('queryUrl');

        if (queryParam) {
            let token = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.CODE, queryParam);

            if (token === '') {
                this.MS.ErrorService.message = this.MS.Translate.AZURE_LOGIN_UNKNOWN_ERROR;
                this.MS.ErrorService.details = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.ERRORDESCRIPTION, queryParam);
                this.MS.ErrorService.showContactUs = true;
            } else {
                await this.MS.HttpService.executeAsync('Microsoft-GetAzureToken', { AADTenant: this.aadTenant, code: token, oauthType: this.oauthType });
            }

            this.MS.UtilityService.RemoveItem('queryUrl');
        } else if (this.MS.DataStore.getValue('HasNavigated') === null) {
            this.MS.NavigationService.NavigateHome();
        } else {
            let isDataStoreValid: boolean = true;

            for (let i = 0; i < this.datastoreEntriesToValidate.length && isDataStoreValid; i++) {
                if (this.MS.DataStore.getValue(this.datastoreEntriesToValidate[i]) === null) {
                    this.MS.NavigationService.NavigateHome();
                    isDataStoreValid = false;
                }
            }

            if (isDataStoreValid) {
                this.ExecuteActions();
            }
        }
    }

    async ExecuteActions(): Promise<void> {
        if (await this.MS.DeploymentService.ExecuteActions() && !this.isUninstall) {
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

            this.QueryRecordCounts();
        }
    }

    async QueryRecordCounts(): Promise<void> {
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
                this.QueryRecordCounts();
            } else {
                this.MS.DeploymentService.hasError = true;
            }
        }
    }
}