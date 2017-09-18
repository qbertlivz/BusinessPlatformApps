import { AzureLogin } from './azure-login';
import { DataStoreType } from "../enums/data-store-type";

export class O365Login extends AzureLogin {
    hasToken: boolean = false;
    permissions: any = {};

    async connect(): Promise<void> {
        this.MS.DataStore.addToDataStore('Permissions', this.permissions, DataStoreType.Public);
        await this.MS.HttpService.getExecuteResponseAsync('Microsoft-CreateADApplication');
        //await this.MS.UtilityService.connectToAzure(this.openAuthorizationType.O365);
    }

    async onLoaded(): Promise<void> {
        this.onInvalidate();

        //if (this.hasToken) {
        //    this.setValidated();
        //} else {
        //    await this.MS.UtilityService.getToken(this.openAuthorizationType.O365, async () => {
        //        this.hasToken = this.setValidated();
        //    });
        //}
    }
}