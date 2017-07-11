import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class CognitiveService extends ViewModelBase {
    cognitiveSelectedType: string = 'NewKey';
    cognitiveServiceKey: string = '';
    cognitiveServiceName: string = 'SolutionTemplateCognitiveService';

    onKeyTypeChange(): void {
        this.onInvalidate();
        if (this.cognitiveSelectedType === 'ExistingKey') {
            this.isValidated = false;
        } else if (this.cognitiveSelectedType === 'NewKey') {
            this.cognitiveServiceKey = '';
            this.isValidated = true;
        }
    }

    async onLoaded(): Promise<void> {
        this.isValidated = true;
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('CognitiveServiceKey', this.cognitiveServiceKey, DataStoreType.Private);
        this.MS.DataStore.addToDataStore('CognitiveServiceName', this.cognitiveServiceName, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('CognitiveSkuName', 'S1', DataStoreType.Public);
        return true;
    }

    async onValidate(): Promise<boolean> {
        if (this.cognitiveSelectedType === 'ExistingKey') {
            this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateCognitiveKey', { CognitiveServiceKey: this.cognitiveServiceKey });
            this.showValidation = this.isValidated;
        } else if (this.cognitiveSelectedType === 'NewKey') {
            this.isValidated = true;
        }
        return this.isValidated;
    }
}