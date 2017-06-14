import { QueryParameter } from '../constants/query-parameter';

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
    selectedResourceGroup: string = `SolutionTemplate-${this.MS.UtilityService.GetUniqueId(5)}`;
    selectedSubscriptionId: string = '';
    showAdvanced: boolean = false;
    showPricingConfirmation: boolean = false;
    subscriptionsList: any[] = [];

    async onLoaded(): Promise<void> {
        this.isValidated = false;
        this.showValidation = false;
        if (this.subscriptionsList.length > 0) {
            this.isValidated = true;
            this.showValidation = true;
        } else {
            let queryParam = this.MS.UtilityService.GetItem('queryUrl');
            if (queryParam) {
                let token = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.CODE, queryParam);
                if (token === '') {
                    this.MS.ErrorService.message = this.MS.Translate.AZURE_LOGIN_UNKNOWN_ERROR;
                    this.MS.ErrorService.details = this.MS.UtilityService.GetQueryParameterFromUrl(QueryParameter.ERRORDESCRIPTION, queryParam);
                    this.MS.ErrorService.showContactUs = true;
                } else {
                    this.authToken = await this.MS.HttpService.executeAsync('Microsoft-GetAzureToken', { code: token, oauthType: this.oauthType });
                    if (this.authToken.IsSuccess) {
                        let subscriptions: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureSubscriptions');
                        if (subscriptions.IsSuccess) {
                            this.subscriptionsList = subscriptions.Body.value;
                            if (!this.subscriptionsList || (this.subscriptionsList && this.subscriptionsList.length === 0)) {
                                this.MS.ErrorService.message = this.MS.Translate.AZURE_LOGIN_SUBSCRIPTION_ERROR;
                            } else {
                                this.selectedSubscriptionId = this.subscriptionsList[0].SubscriptionId;
                                this.showPricingConfirmation = true;
                                this.isValidated = true;
                                this.showValidation = true;
                                await this.MS.HttpService.executeAsync('Microsoft-PowerBiLogin');
                            }
                        }
                    }
                }

                this.MS.UtilityService.RemoveItem('queryUrl');
            }
        }
    }

    async ValidateResourceGroup(): Promise<boolean> {
        this.onInvalidate();
        let subscriptionObject = this.subscriptionsList.find(x => x.SubscriptionId === this.selectedSubscriptionId);
        this.MS.DataStore.addToDataStore('SelectedSubscription', subscriptionObject, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('SelectedResourceGroup', this.selectedResourceGroup, DataStoreType.Public);

        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-ExistsResourceGroup');

        if (response.IsSuccess) {
            this.isValidated = true;
            this.showValidation = true;
        }
        return this.isValidated;
    }

    async connect(): Promise<void> {
        if (this.connectionType.toString() === AzureConnection.Microsoft.toString()) {
            this.MS.DataStore.addToDataStore('AADTenant', this.azureDirectory, DataStoreType.Public);
        } else {
            this.MS.DataStore.addToDataStore('AADTenant', 'common', DataStoreType.Public);
        }
        let response: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetAzureAuthUri', { oauthType: this.oauthType });
        window.location.href = response.Body.value;
    }

    async onNavigatingNext(): Promise<boolean> {
        let isSuccess: boolean = true;

        this.MS.DataStore.addToDataStore('SelectedSubscription', this.subscriptionsList.find(x => x.SubscriptionId === this.selectedSubscriptionId), DataStoreType.Public);
        this.MS.DataStore.addToDataStore('SelectedResourceGroup', this.selectedResourceGroup, DataStoreType.Public);

        let locationsResponse: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-GetLocations');
        if (locationsResponse.IsSuccess) {
            this.MS.DataStore.addToDataStore('SelectedLocation', locationsResponse.Body.value[this.defaultLocation], DataStoreType.Public);
        }

        isSuccess = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-CreateResourceGroup');

        for (let i = 0; i < this.azureProviders.length && isSuccess; i++) {
            isSuccess = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-RegisterProvider', { AzureProvider: this.azureProviders[i] });
        }

        for (let i = 0; i < this.bapiServices.length && isSuccess; i++) {
            isSuccess = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-RegisterBapiService', { BapiService: this.bapiServices[i] });
        }

        return isSuccess;
    }
}