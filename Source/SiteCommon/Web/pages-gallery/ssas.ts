import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {
    email: string = '';
    password: string = '';
    server: string = '';
    sku: string = 'B1';
    ssasType: string = 'New';

    async onNavigatingNext(): Promise<boolean> {
        let isSuccess: boolean = true;

        if (this.ssasType == 'New') {
            isSuccess = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-DeployAzureAnalysisServices', { ASServerName: this.server, ASSku: this.sku });

            if (isSuccess) {
                this.server = this.MS.DataStore.getValue('ASServerUrl');
                this.ssasType = 'Existing';

                isSuccess = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateConnectionToAS');
            }
        }

        return isSuccess;
    }

    async onValidate(): Promise<boolean> {
        this.onInvalidate();

        if (this.ssasType == 'New') {
            if (this.server.length < 3 || this.server.length > 63 || !/[a-z]/.test(this.server[0]) || !/^[a-z0-9]+$/.test(this.server)) {
                this.MS.ErrorService.message = this.MS.Translate.SSAS_INVALID_SERVER_NAME;
                this.isValidated = false;
            } else {
                this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-CheckASServerNameAvailability', { ASServerName: this.server });
            }
        } else {
            this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateConnectionToAS', { ASServerUrl: this.server });
            if (this.isValidated) {
                this.MS.DataStore.addToDataStore('ASServerUrl', this.server, DataStoreType.Public);
            }
        }

        this.showValidation = this.isValidated;

        return this.isValidated;
    }
}