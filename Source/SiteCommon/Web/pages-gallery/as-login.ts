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

    async onLoaded(): Promise<void> {
        this.onInvalidate();

        if (this.hasToken) {
            this.isValidated = true;
            this.showValidation = true;
        } else {
            await this.MS.UtilityService.getToken(this.oauthType, async () => {
                this.hasToken = true;
                this.isValidated = true;
                this.showValidation = true;
            });
        }
    }
}