import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {
    showDescription: boolean = false;
    ssasEnabled: string = 'false';
   
    async onLoaded(): Promise<void> {
        this.isValidated = true;
        this.showValidation = false;
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStoreWithCustomRoute('ssas', 'ssasDisabled', this.ssasEnabled === 'true' ? 'false' : 'true', DataStoreType.Public);
        return true;
    }
}