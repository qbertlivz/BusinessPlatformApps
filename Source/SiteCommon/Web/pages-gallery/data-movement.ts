import { DataMovementType } from '../enums/data-movement-type';
import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class DataMovement extends ViewModelBase {
    DataMovementType: any = DataMovementType;

    dataMovement: DataMovementType = DataMovementType.ADF;
    password: string = '';
    username: string = '';

    OnDataMovementChanged(): void {
        this.isValidated = this.dataMovement === DataMovementType.ADF;
    }

    async OnLoaded(): Promise<void> {
        this.isValidated = true;
    }

    async NavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('DataMovement', this.dataMovement, DataStoreType.Public);

        return true;
    }
}