import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {

    axBaseUrl: string = '';
    instances: string[] = [];
    selectedInstance: string = '';

    async onLoaded(): Promise<void> {
        super.onLoaded();
        await this.getInstances();
    }

    async onValidate(): Promise<boolean> {

        if (this.axBaseUrl) {
            this.MS.DataStore.addToDataStore('AxInstanceName', this.axBaseUrl, DataStoreType.Public);
            this.isValidated = true;
        }
        if(this.selectedInstance){
            this.MS.DataStore.addToDataStore('AxInstanceName', this.selectedInstance, DataStoreType.Public);
            this.isValidated = true;
        }        
        return true;
    }

    async onInvalidate(): Promise<void> {
        super.onInvalidate();
        if (this.selectedInstance) {
            this.selectedInstance = '';
        }
        else {
            await this.getInstances();
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlSubGroup', 'D365', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlEntryName', 'Operations', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlEntryValue', 'CreditAndCollections', DataStoreType.Public);

        return true;
    }

    async getInstances(): Promise<void> {
        this.instances = await this.MS.HttpService.getResponseAsync('Microsoft-GetAxInstances');
        if (!this.instances || (this.instances && this.instances.length === 0)) {
            this.MS.ErrorService.message = 'Failed to retrieve AX instances.';
        } else {
            this.selectedInstance = this.instances[0];
        }
    }
}