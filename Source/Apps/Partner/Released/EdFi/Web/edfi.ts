import { DataStoreType } from '../../../../../SiteCommon/Web/enums/data-store-type';

import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base';

export class Customize extends ViewModelBase {
    sqlServerAdminLogin: string = '';
    sqlServerAdminPassword: string = '';
    sqlServerProductionApiLogin: string = '';
    sqlServerProductionApiPassword: string = '';

    localOrganizationID: string = '';
    stateOrganizationID: string = '';
    agencyCategoryType: string = '';
    institutionName: string = '';
    institutionAddress: string = '';
    institutionCity: string = '';
    institutionState: string = '';
    institutionZip: string = '';

    async onNavigatingNext(): Promise<boolean> {

        this.MS.DataStore.addToDataStore('sqlServerAdminLogin', this.sqlServerAdminLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('sqlServerAdminPassword', this.sqlServerAdminPassword, DataStoreType.Private);
        this.MS.DataStore.addToDataStore('sqlServerProductionApiLogin', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('sqlServerProductionApiPassword', this.sqlServerProductionApiPassword, DataStoreType.Private);

        this.MS.DataStore.addToDataStore('localOrganizationID', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('stateOrganizationID', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('agencyCategoryType', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('institutionName', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('institutionAddress', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('institutionCity', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('institutionState', this.sqlServerProductionApiLogin, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('institutionZip', this.sqlServerProductionApiLogin, DataStoreType.Public);

        return true;
    }


    async onValidate(): Promise<boolean> {
        this.isValidated = this.sqlServerAdminPassword.length > 0 && this.sqlServerAdminLogin.length > 0 && this.sqlServerProductionApiLogin.length > 0 && this.sqlServerProductionApiPassword.length > 0;
        return this.isValidated;
    }
}