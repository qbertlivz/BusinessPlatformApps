import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {

    axBaseUrl: string = '';

    async onLoaded(): Promise<void> {
    }

    async onValidate(): Promise<boolean> {

        this.MS.DataStore.addToDataStore('AxInstanceName', this.axBaseUrl, DataStoreType.Public);
        this.isValidated = true;
        return true;
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlSubGroup', 'D365', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlEntryName', 'Operations', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('CustomizeBaseUrl', 'SqlEntryValue', 'CreditAndCollections', DataStoreType.Public);

        return true;
    }
}