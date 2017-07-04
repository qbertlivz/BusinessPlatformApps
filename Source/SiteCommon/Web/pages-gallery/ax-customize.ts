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
        return true;
    }
}