import { DataStoreType } from '../enums/data-store-type';
import { AzureConnection } from '../enums/azure-connection';
import { ViewModelBase } from '../services/view-model-base';
import { ActionResponse } from "../models/action-response";
import { QueryParameter } from '../constants/query-parameter';

export class Customize extends ViewModelBase {
    authToken: any = {};
    azureDirectory: string = '';
    connectionType: AzureConnection = AzureConnection.Organizational;
    oauthType: string = 'axerp';
    axBaseUrl: string = '';
    urlRegex: RegExp;

    async onLoaded(): Promise<void> {
        super.onLoaded();

        if (this.axBaseUrl === this.MS.DataStore.getValue('ValidatedAxInstanceName')) {    
            this.validationText = this.MS.Translate.NAVIGATION_SUCCESSFULLY_CONNECTED;
            this.setValidated();
        } 

        if (this.axBaseUrl == '' && this.MS.DataStore.getValue('AxInstanceName') != null) {
            this.axBaseUrl = this.MS.DataStore.getValue('AxInstanceName', DataStoreType.Public);
        }

        this.urlRegex = /^(ht|f)tps:\/\/[a-z0-9-\.]+\.[a-z]{2,4}\/?([^\s<>\#%\,\{\}\\|\\\^\[\]]+)?\/$/;
        
        await this.getToken(this.oauthType, async () => {
            if (await this.validateAxInstance()) {
                this.validationText = this.MS.Translate.NAVIGATION_SUCCESSFULLY_CONNECTED;
                this.setValidated();
                this.MS.DataStore.addToDataStore('ValidatedAxInstanceName', this.axBaseUrl, DataStoreType.Public);
            }
            else {
                this.MS.ErrorService.message = this.MS.Translate.AX_LOGIN_UNKNOWN_ERROR;
            }
        });        
    }

    async connect(): Promise<void> {        
        if (this.axBaseUrl) {
            if (!this.axBaseUrl.endsWith('/')) {
                this.axBaseUrl = this.axBaseUrl + '/';
            }
            if (this.urlRegex.test(this.axBaseUrl)) {
                this.MS.DataStore.addToDataStore('AxInstanceName', this.axBaseUrl, DataStoreType.Public);
                await this.connectToAzure(this.oauthType, this.isConnectionMicrosoft() ? this.azureDirectory : this.MS.Translate.DEFAULT_TENANT);
            } else {
                this.MS.ErrorService.message = 'Validation failed. The url address ' + this.axBaseUrl + ' is not valid.';
            }
        }     
    }

    async connectToAzure(openAuthorizationType: string, azureActiveDirectoryTenant: string = this.MS.Translate.DEFAULT_TENANT): Promise<void> {
        this.MS.DataStore.addToDataStore('AADTenant', azureActiveDirectoryTenant, DataStoreType.Public);
        window.location.href = await this.MS.HttpService.getExecuteResponseAsync('Microsoft-GetAzureAuthUri', 'value', { oauthType: openAuthorizationType });
    }

    isConnectionMicrosoft(): boolean {
        return this.connectionType.toString() === AzureConnection.Microsoft.toString();
    }   

    async getToken(openAuthorizationType: string, callback: () => Promise<void>): Promise<void> {
        let queryParam: any = this.MS.UtilityService.getItem('queryUrl');
        if (queryParam) {
            let token = this.MS.UtilityService.getQueryParameterFromUrl(QueryParameter.CODE, queryParam);
            if (token === '') {
                this.MS.ErrorService.set(this.MS.Translate.AX_LOGIN_UNKNOWN_ERROR, this.MS.UtilityService.getQueryParameterFromUrl(QueryParameter.ERROR_DESCRIPTION, queryParam));
            } else {
                if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-GetAzureToken', { code: token, oauthType: openAuthorizationType })) {
                    await callback();
                }
            }
            this.MS.UtilityService.removeItem('queryUrl');
        }
    }    

    async validateAxInstance(): Promise<boolean> {
        let validInstance: ActionResponse = await this.MS.HttpService.executeAsync('Microsoft-ValidateAxInstance');
        if (validInstance.IsSuccess)
            return true;
        return false;
    }

    async onChange(): Promise<any> {
        if (this.axBaseUrl === this.MS.DataStore.getValue('ValidatedAxInstanceName')) {
            this.validationText = this.MS.Translate.NAVIGATION_SUCCESSFULLY_CONNECTED;
            this.setValidated();            
        } else {
            this.isValidated = false;
            this.onInvalidate();
        }
    }
}