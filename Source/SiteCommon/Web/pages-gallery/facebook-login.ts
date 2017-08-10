import { AzureConnection } from '../enums/azure-connection';
import { DataStoreType } from '../enums/data-store-type';

import { ActionResponse } from '../models/action-response';

import { ViewModelBase } from '../services/view-model-base';

export class AzureLogin extends ViewModelBase {
    authToken: any = {};
    azureConnection = AzureConnection;
    azureDirectory: string = '';
    azureProviders: string[] = [];
    bapiServices: string[] = [];
    bingUrl: string = '';
    bingtermsofuse: string = '';
    connectionType: AzureConnection = AzureConnection.Organizational;
    defaultLocation: number = 5;
    oauthType: string = '';
    pricingCalculator: string = '';
    pricingCalculatorUrl: string = '';
    pricingUrl: string = '';
    pricingCost: string = '';
    selectedResourceGroup: string = `SolutionTemplate-${this.MS.UtilityService.getUniqueId(5)}`;
    selectedSubscriptionId: string = '';
    showAdvanced: boolean = false;
    showPricingConfirmation: boolean = false;
    subscriptionsList: any[] = [];

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToFacebook("1631359040270051");
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();

        await this.MS.UtilityService.getToken(this.oauthType, async () => { this.setValidated(); });
    }
}