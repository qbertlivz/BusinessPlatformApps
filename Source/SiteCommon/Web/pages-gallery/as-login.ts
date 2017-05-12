import { QueryParameter } from '../constants/query-parameter';

import { DataStoreType } from '../enums/data-store-type';

import { ActionResponse } from '../models/action-response';

import { AzureLogin } from './azure-login';

export class ASLogin extends AzureLogin {
    hasToken: boolean = false;
    

    constructor() {
        super();
        this.oauthType = "as"
    }

    async OnLoaded(): Promise<void> {
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
                    return;
                }

                var tokenObj: any = { code: token, oauthType: this.oauthType };
                this.authToken = await this.MS.HttpService.executeAsync('Microsoft-GetAzureToken', tokenObj);
                if (this.authToken.IsSuccess) {
                    // The token will be added by the action - hence it was removed
                    this.hasToken = true;
                    this.isValidated = true;
                    this.showValidation = true;
                }
                this.MS.UtilityService.RemoveItem('queryUrl');
            }
        }
    }

    async connect(): Promise<void> {
        var tokenObj: any = { oauthType: this.oauthType };
        this.MS.DataStore.addToDataStore('AADTenant', 'common', DataStoreType.Public);
        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureAuthUri', tokenObj);
        window.location.href = response.Body.value;
    }

    public async NavigatingNext(): Promise<boolean> {
        return true;
    }
}