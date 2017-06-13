import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {
    email: string = '';
    password: string = '';
    server: string = '';
    sku: string = 'B1';
    ssasType: string = 'New';

    async NavigatingNext(): Promise<boolean> {
        if (this.ssasType == 'New') {
            if (!(await this.MS.HttpService.executeAsync('Microsoft-DeployAzureAnalysisServices', { ASServerName: this.server, ASSku: this.sku })).IsSuccess) return false;

            this.server = this.MS.DataStore.getValue("ASServerUrl");
            this.ssasType = "Existing";

            if (!(await this.MS.HttpService.executeAsync('Microsoft-ValidateConnectionToAS')).IsSuccess) return false;
        }

        return true;
    }

    async OnLoaded(): Promise<void> {
        this.isValidated = false;
    }

    async OnValidate(): Promise<boolean> {
        this.showValidation = true;
        if (this.ssasType == "New") {
            if (this.server.length < 3 || this.server.length > 63 || !/[a-z]/.test(this.server[0]) || !/^[a-z0-9]+$/.test(this.server)) {
                this.MS.ErrorService.message = this.MS.Translate.SSAS_INVALID_SERVER_NAME;
                return false;
            }

            let body: any = {};
            body.ASServerName = this.server;

            this.isValidated = (await this.MS.HttpService.executeAsync('Microsoft-CheckASServerNameAvailability', body)).IsSuccess

            return this.isValidated;
        } else {
            let body: any = {};
            body.ASServerUrl = this.server;

            if ((await this.MS.HttpService.executeAsync('Microsoft-ValidateConnectionToAS', body)).IsSuccess) {
                this.isValidated = true;
                this.MS.DataStore.addToDataStore("ASServerUrl", this.server, DataStoreType.Public);
                return true;
            }

            this.isValidated = false;
            return false;
        }
    }
}