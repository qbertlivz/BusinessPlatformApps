import { DataStoreType } from '../../../../../SiteCommon/Web/enums/data-store-type';

import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base';

export class vstsdetails extends ViewModelBase {
    quickbooksToken: string = '';

    async onValidate(): Promise<boolean> {
        if (this.quickbooksToken.length > 0 && this.quickbooksToken.length > 0) {
            //this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateFacebookPage', { FacebookPages: this.searchQuery });

            this.showValidation = this.isValidated;
            this.isValidated = true;

            if (this.isValidated) {
                this.MS.DataStore.addToDataStore('quickbooksToken', this.quickbooksToken, DataStoreType.Private);
            }
        }

        return this.isValidated;
    }
}