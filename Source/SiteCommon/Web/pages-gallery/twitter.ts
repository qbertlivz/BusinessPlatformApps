import { QueryParameter } from '../constants/query-parameter';

import { ActionStatus } from '../enums/action-status';
import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Twitter extends ViewModelBase {
    authToken: any = {};
    isAuthenticated: boolean = false;
    selectedSubscriptionId: string;
    subscriptionsList: any[];

    async connect(): Promise<void> {
        if (!this.isAuthenticated) {
            let response = await this.MS.HttpService.executeAsync('Microsoft-CreateTwitterConnectionToLogicApp');
            if (response.IsSuccess) {
                window.location.href = response.Body['Consent']['value'][0]['link'];
            }
        }
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();

        this.isAuthenticated = false;

        let queryParam = this.MS.UtilityService.getItem('queryUrl');
        if (queryParam) {
            let code = this.MS.UtilityService.getQueryParameterFromUrl(QueryParameter.CODE, queryParam);
            if (code) {
                this.MS.DataStore.addToDataStore('TwitterCode', code, DataStoreType.Private);

                if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ConsentTwitterConnectionToLogicApp')) {
                    this.isAuthenticated = this.setValidated();
                }
            } else {
                let response = await this.MS.HttpService.executeAsync('Microsoft-VerifyTwitterConnection');
                if (response.Status === ActionStatus.FailureExpected) {
                    this.MS.ErrorService.clear();
                }
                if (response.IsSuccess) {
                    this.isAuthenticated = this.setValidated();
                }
            }
            this.MS.UtilityService.removeItem('queryUrl');
        } else {
            let response = await this.MS.HttpService.executeAsync('Microsoft-VerifyTwitterConnection');
            this.MS.ErrorService.clear();
            if (response.IsSuccess) {
                this.isAuthenticated = this.setValidated();
            }
        }
    }
}