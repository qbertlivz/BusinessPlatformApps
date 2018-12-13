import { DataStoreType } from '../../../../../SiteCommon/Web/enums/data-store-type';

import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base';

export class Customize extends ViewModelBase {
    dailyTrigger: string = '2:00';
    dailyTriggers: string[] = [];

    async onLoaded(): Promise<void> {
        super.onLoaded();

        this.dailyTriggers = this.MS.UtilityService.generateDailyTriggers();
        this.useDefaultValidateButton = true;
    }

    async onNavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('TaskDescription', 'Power BI Solution Template - Simplement SAP AR', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskDirectory', 'Simplement, Inc\\Solution Template AR', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskFile', 'Simplement.SolutionTemplate.AR.exe', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskName', 'Power BI Solution Template - Simplement SAP AR', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskParameters', '/u', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskProgram', 'cmd', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskStartTime', this.dailyTrigger, DataStoreType.Public);

        return true;
    }

    async onValidate(): Promise<boolean> {
        this.setValidated();
        return this.isValidated;
    }
}