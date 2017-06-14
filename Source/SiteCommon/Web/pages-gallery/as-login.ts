import { QueryParameter } from '../constants/query-parameter';

import { DataStoreType } from '../enums/data-store-type';

import { ActionResponse } from '../models/action-response';

import { AzureLogin } from './azure-login';

export class ASLogin extends AzureLogin {
    hasToken: boolean = false;

    constructor() {
        super();
        this.oauthType = 'as';
    }

    async connect(): Promise<void> {
        this.MS.DataStore.addToDataStore('AADTenant', 'common', DataStoreType.Public);
        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureAuthUri', { oauthType: this.oauthType });
        window.location.href = response.Body.value;
    }

    async OnLoaded(): Promise<void> {
        this.Invalidate();

        if (this.hasToken) {
            this.isValidated = true;
            this.showValidation = true;
        } else {
            let queryParam = this.MS.UtilityService.GetItem('queryUrl');
            if (queryParam) {
                let token = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.CODE, queryParam);
                if (token === '') {
                    this.MS.ErrorService.set(this.MS.Translate.AZURE_LOGIN_UNKNOWN_ERROR, this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.ERRORDESCRIPTION, queryParam));
                } else {
                    this.authToken = await this.MS.HttpService.executeAsync('Microsoft-GetAzureToken', { code: token, oauthType: this.oauthType });
                    if (this.authToken.IsSuccess) {
                        this.hasToken = true;
                        this.isValidated = true;
                        this.showValidation = true;
                    }
                }
                this.MS.UtilityService.RemoveItem('queryUrl');
            }
        }
    }
}