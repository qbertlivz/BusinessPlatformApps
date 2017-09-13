import { DataStoreType } from '../enums/data-store-type';

import { ApiManagementService } from '../models/api-management-service';

import { ViewModelBase } from '../services/view-model-base';

export class ApiManagement extends ViewModelBase {
    listApimServices: ApiManagementService[] = [];
    nameApimService: string = '';

    async onLoaded(): Promise<void> {
        this.listApimServices = await this.MS.HttpService.getResponseAsync('Microsoft-GetAPIManagementServices');

        if (this.listApimServices && this.listApimServices.length > 0) {
            this.nameApimService = this.listApimServices[0].name;
            this.isValidated = true;
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('NameApimService', this.nameApimService, DataStoreType.Public);
        return true;
    }
}