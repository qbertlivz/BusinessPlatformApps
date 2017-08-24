import { DataStoreType } from '../../../../../SiteCommon/Web/enums/data-store-type';

import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base';

export class FacebookLogin extends ViewModelBase {
    facebookClientId: string = '';
    facebookClientSecret: string = '';

    async onValidate(): Promise<boolean> {
        if (this.facebookClientId.length > 0 && this.facebookClientSecret.length > 0) {
            this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateFacebookDeveloperAccount', {
                FacebookClientId: this.facebookClientId,
                FacebookClientSecret: this.facebookClientSecret
            });
            this.showValidation = this.isValidated;

            this.MS.DataStore.addToDataStore('FacebookClientId', this.facebookClientId, DataStoreType.Private);
            this.MS.DataStore.addToDataStore('FacebookClientSecret', this.facebookClientSecret, DataStoreType.Private);
        }

        return this.isValidated;
    }
}