import { QueryParameter } from '../constants/query-parameter';

import { DataStoreType } from '../enums/data-store-type';

import { ActionResponse } from '../models/action-response';

import { AzureLogin } from './azure-login';

export class KeyVaultLogin extends AzureLogin {
    hasToken: boolean = false;

    async connect(): Promise<void> {
        this.MS.DataStore.addToDataStore('AADTenant', 'common', DataStoreType.Public);
        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureAuthUri', { oauthType: this.oauthType });
        window.location.href = response.Body.value;
    }

    async onLoaded(): Promise<void> {
        this.isValidated = false;
        this.showValidation = false;

        if (this.hasToken) {
            this.isValidated = true;
            this.showValidation = true;
        } else {
            let queryParam = this.MS.UtilityService.GetItem('queryUrl');
            if (queryParam) {
                let token = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.CODE, queryParam);
                if (token === '') {
                    this.MS.ErrorService.message = this.MS.Translate.AZURE_LOGIN_UNKNOWN_ERROR;
                    this.MS.ErrorService.details = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.ERRORDESCRIPTION, queryParam);
                    this.MS.ErrorService.showContactUs = true;
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