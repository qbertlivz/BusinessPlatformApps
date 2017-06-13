import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class TwitterHandles extends ViewModelBase {
    accounts: string = '';
    twitterHandleId: string = '';
    twitterHandleName: string = '';

    async OnLoaded(): Promise<void> {
        this.isValidated = true;
        this.showValidation = false;
    }

    async OnValidate(): Promise<boolean> {
        let response = await this.MS.HttpService.executeAsync('Microsoft-ValidateTwitterAccount', { Accounts: this.accounts });
        if (response.IsSuccess) {
            this.isValidated = true;
            this.showValidation = true;
            this.twitterHandleName = response.Body.twitterHandle;
            this.twitterHandleId = response.Body.twitterHandleId;
        }

        this.MS.DataStore.addToDataStore('TwitterHandles', this.accounts, DataStoreType.Public);

        return this.isValidated;
    }

    async Invalidate(): Promise<void> {
        this.isValidated = !this.accounts;
    }

    async NavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStoreWithCustomRoute('c1', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c1', 'SqlSubGroup', 'Twitter', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c1', 'SqlEntryName', 'twitterHandle', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c1', 'SqlEntryValue', this.twitterHandleName, DataStoreType.Public);

        this.MS.DataStore.addToDataStoreWithCustomRoute('c2', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c2', 'SqlSubGroup', 'Twitter', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c2', 'SqlEntryName', 'twitterHandleId', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('c2', 'SqlEntryValue', this.twitterHandleId, DataStoreType.Public);

        return true;
    }
}