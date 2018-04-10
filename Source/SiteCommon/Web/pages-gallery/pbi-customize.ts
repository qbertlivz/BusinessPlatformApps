import { ViewModelBase } from '../services/view-model-base';
import { PBIWorkspace } from '../models/pbi-workspace';
import { DataStoreType } from '../enums/data-store-type';
import { AzureConnection } from '../enums/azure-connection';
import { QueryParameter } from '../constants/query-parameter';

export class Customize extends ViewModelBase {
    pbiWorkspaces: PBIWorkspace[] = [];
    selectedPBIWorkspaceId: string = '';
    authToken: any = {};
    azureDirectory: string = '';
    connectionType: AzureConnection = AzureConnection.Organizational;
    oauthType: string = 'powerbi';

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToAzure(this.oauthType, this.isConnectionMicrosoft() ? this.azureDirectory : this.MS.Translate.DEFAULT_TENANT);        
    }

    isConnectionMicrosoft(): boolean {
        return this.connectionType.toString() === AzureConnection.Microsoft.toString();
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();

        this.textNext = 'Deploy';

        await this.getToken(this.oauthType, async () => {
            await this.getInstances();
            this.selectedPBIWorkspaceId = this.pbiWorkspaces[0].id;
        });

        if (this.pbiWorkspaces && this.pbiWorkspaces.length > 0) {
            this.isValidated = true;
        }
    }

    async getInstances(): Promise<void> {
        if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-GetPBIClusterUri')) {
            this.pbiWorkspaces = await this.MS.HttpService.getResponseAsync('Microsoft-GetPBIWorkspaces');            
        }
    }

    async getToken(openAuthorizationType: string, callback: () => Promise<void>): Promise<void> {
        let queryParam: any = this.MS.UtilityService.getItem('queryUrl');
        if (queryParam) {
            let token = this.MS.UtilityService.getQueryParameterFromUrl(QueryParameter.CODE, queryParam);
            if (token == '') {
                this.pbiWorkspaces = [];
                this.MS.ErrorService.set(this.MS.Translate.POWER_BI_UNKNOWN_ERROR, this.MS.UtilityService.getQueryParameterFromUrl(QueryParameter.ERROR_DESCRIPTION, queryParam));                
            } else {
                if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-GetAzureToken', { code: token, oauthType: openAuthorizationType })) {
                    await callback();
                }
            }
            this.MS.UtilityService.removeItem('queryUrl');
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('PBIWorkspaceId', this.selectedPBIWorkspaceId, DataStoreType.Public);        
        return true;
    }
}