import { ViewModelBase } from '../services/view-model-base';
import { PBIWorkspace } from '../models/pbi-workspace';
import { DataStoreType } from '../enums/data-store-type';

export class Customize extends ViewModelBase {
    pbiWorkspaces: PBIWorkspace[] = [];
    selectedPBIWorkspaceId: string = '';

    async onLoaded(): Promise<void> {
        this.textNext = 'Run';
        super.onLoaded();
        await this.getInstances();
    }

    async getInstances(): Promise<void> {
        if (await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-GetPBIClusterUri')) {
            this.pbiWorkspaces = await this.MS.HttpService.getResponseAsync('Microsoft-GetPBIWorkspaces');
            if (this.pbiWorkspaces && this.pbiWorkspaces.length > 0) {
                this.selectedPBIWorkspaceId = this.pbiWorkspaces[0].id;
                this.isValidated = true;
            }
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('PBIWorkspaceId', this.selectedPBIWorkspaceId, DataStoreType.Public);
        
        return true;
    }
}