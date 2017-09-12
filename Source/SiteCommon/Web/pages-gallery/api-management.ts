import { DataStoreType } from '../enums/data-store-type';

import { ApiManagementService } from '../models/api-management-service';

import { ViewModelBase } from '../services/view-model-base';

export class ApiManagement extends ViewModelBase {
    apiManagementServiceName: string = '';
    apiManagementServices: ApiManagementService[] = [];

    async onLoaded(): Promise<void> {
        this.apiManagementServices = await this.MS.HttpService.getResponseAsync('Microsoft-GetAPIManagementServices');

        if (this.apiManagementServices && this.apiManagementServices.length > 0) {
            this.apiManagementServiceName = this.apiManagementServices[0].name;
            this.isValidated = true;
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('ApiManagementServiceName', this.apiManagementServiceName, DataStoreType.Public);
        return true;
    }
}