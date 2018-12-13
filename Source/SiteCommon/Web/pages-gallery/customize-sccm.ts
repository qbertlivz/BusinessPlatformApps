import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class Customize extends ViewModelBase {
    dailyTrigger: string = '2:00';
    dailyTriggers: string[] = [];
    dataRetentionDays: string = '120';
    endpointComplianceTarget: string = '0.99';
    healthEvaluationTarget: string = '0.99';

    async onLoaded(): Promise<void> {
        super.onLoaded();

        this.dailyTriggers = this.MS.UtilityService.generateDailyTriggers();
        this.useDefaultValidateButton = true;
    }

    async onNavigatingNext(): Promise<boolean> {
        let sourceServer = this.MS.DataStore.getAllValues('Server')[0];
        let sourceDatabase = this.MS.DataStore.getAllValues('Database')[0];

        let targetServer = this.MS.DataStore.getAllValues('Server')[1];
        let targetDatabase = this.MS.DataStore.getAllValues('Database')[1];

        this.MS.DataStore.addToDataStore('TaskDescription', 'Power BI Solution Template - SCCM', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskFile', 'dataload.ps1', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskName', 'Power BI Solution Template - SCCM', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskParameters', `-SourceServer "${sourceServer}" -SourceDatabase "${sourceDatabase}" -DestinationServer "${targetServer}" -DestinationDatabase "${targetDatabase}"`, DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskProgram', 'powershell', DataStoreType.Public);
        this.MS.DataStore.addToDataStore('TaskStartTime', this.dailyTrigger, DataStoreType.Public);

        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize', 'SqlSubGroup', 'System Center', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize', 'SqlEntryName', 'endpointcompliancetarget', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize', 'SqlEntryValue', this.endpointComplianceTarget, DataStoreType.Public);

        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize1', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize1', 'SqlSubGroup', 'System Center', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize1', 'SqlEntryName', 'healthevaluationtarget', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize1', 'SqlEntryValue', this.healthEvaluationTarget, DataStoreType.Public);

        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize2', 'SqlGroup', 'SolutionTemplate', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize2', 'SqlSubGroup', 'System Center', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize2', 'SqlEntryName', 'dataretentiondays', DataStoreType.Public);
        this.MS.DataStore.addToDataStoreWithCustomRoute('Customize2', 'SqlEntryValue', this.dataRetentionDays, DataStoreType.Public);

        return true;
    }

    async onValidate(): Promise<boolean> {
        let dataRetentionDays: number = parseInt(this.dataRetentionDays);
        let endpointComplianceTarget: number = parseFloat(this.endpointComplianceTarget);
        let healthEvaluationTarget: number = parseFloat(this.healthEvaluationTarget);

        let dataRetentionDaysError: string = dataRetentionDays > 0 && dataRetentionDays <= 365
            ? ''
            : this.MS.Translate.CUSTOMIZE_SCCM_ERROR_DATA_RETENTION_DAYS;
        let endpointComplianceTargetError: string = endpointComplianceTarget >= 0 && endpointComplianceTarget <= 1
            ? ''
            : this.MS.Translate.CUSTOMIZE_SCCM_ERROR_ENDPOINT_COMPLIANCE_TARGET;
        let healthEvaluationTargetError: string = healthEvaluationTarget >= 0 && healthEvaluationTarget <= 1
            ? ''
            : this.MS.Translate.CUSTOMIZE_SCCM_ERROR_HEALTH_EVALUATION_TARGET;

        let validationError: string = dataRetentionDaysError || endpointComplianceTargetError || healthEvaluationTargetError;
        if (validationError) {
            this.MS.ErrorService.set(validationError);
        } else {
            this.dataRetentionDays = dataRetentionDays.toString();
            this.endpointComplianceTarget = endpointComplianceTarget.toString();
            this.healthEvaluationTarget = healthEvaluationTarget.toString();

            this.setValidated();
        }

        return this.isValidated;
    }
}