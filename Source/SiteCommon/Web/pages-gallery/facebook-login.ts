import { AzureConnection } from '../enums/azure-connection';
import { DataStoreType } from '../enums/data-store-type';

import { ActionResponse } from '../models/action-response';

import { ViewModelBase } from '../services/view-model-base';

export class AzureLogin extends ViewModelBase {
    authToken: any = {};
    oauthType: string = '';
    pages: any[] = [];
    selectedPage: any;

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToFacebook();
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();
        this.oauthType = "Facebook";
        await this.MS.UtilityService.getToken(this.oauthType, async () => { this.setValidated(); });
        this.pages = this.MS.DataStore.getJson("data");
    }
}