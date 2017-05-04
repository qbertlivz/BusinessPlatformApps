import { DataStoreType } from '../enums/data-store-type';

import { Entry } from '../models/entry';
import { EntryRow } from '../models/entry-row';

import { ViewModelBase } from '../services/view-model-base';

export class SummaryViewModel extends ViewModelBase {
    emailAddress: string = '';
    sendCompletionNotification: boolean = true;
    sendMarketingMail: boolean = false;
    summaryRows: EntryRow[];
    values: any = {};

    constructor() {
        super();
    }

    loadSummaryObjectIntoRows(): void {
        this.textNext = this.MS.Translate.COMMON_RUN;
        this.summaryRows = new Array<EntryRow>();
        let entryRow: EntryRow = new EntryRow();
        for (let text in this.values) {
            if (this.values.hasOwnProperty(text) && this.values[text]) {
                entryRow.entries.push(new Entry(text, this.values[text]));
                if (entryRow.entries.length > 2) {
                    this.summaryRows.push(entryRow);
                    entryRow = new EntryRow();
                }
            }
        }
        if (entryRow.entries.length > 0) {
            this.summaryRows.push(entryRow);
        }
    }

    async OnLoaded(): Promise<void> {
        this.loadSummaryObjectIntoRows();
        this.isValidated = true;
        this.emailAddress = this.MS.DataStore.getValue("EmailAddress");
    }

    async NavigatingNext(): Promise<boolean> {
        this.MS.DataStore.addToDataStore('EmailAddress', this.emailAddress, DataStoreType.Public);
        if (this.sendMarketingMail) {
            this.MS.HttpService.executeAsync('Microsoft-EmailSubscription', {
                isInvisible: true
            });
        }
        this.MS.DataStore.addToDataStore('SendCompletionNotification', this.sendCompletionNotification, DataStoreType.Public);
        return true;
    }
}