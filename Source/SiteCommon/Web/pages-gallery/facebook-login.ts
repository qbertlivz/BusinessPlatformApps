import { DataStoreType } from '../enums/data-store-type';
import { ViewModelBase } from '../services/view-model-base';

export class AzureLogin extends ViewModelBase {
    authToken: any = {};
    oauthType: string = '';
    pages: any[] = [];
    selectedPage: any;
    pageId: string = '';
    permanentPageToken: string = '';
    ownsPage: boolean = null;
    showOldLogin: boolean = false;
    facebookClientId: string = '';
    facebookClientSecret: string = '';
    firstSelection: boolean = true;
    buttonSelection: string = '';

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToFacebook();
        this.pageId = '';
        this.permanentPageToken = '';
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();
        this.ownsPage = null;
        this.showOldLogin = false;
        this.oauthType = "Facebook";
        await this.MS.UtilityService.getToken(this.oauthType, async () => { this.setValidated(); });
        this.pages = this.MS.DataStore.getJson("data");
        if (this.pages && this.pages.length > 0) {
            this.selectedPage = this.pages[0];
        }
    }

    async onNavigatingNext(): Promise<boolean> {
        if (this.ownsPage) {
            let id: string = '';
            let token: string = '';

            if (this.pageId != '' && this.permanentPageToken != '') {
                id = this.pageId;
                token = this.permanentPageToken;
            }
            else {
                let p = this.pages.find(arg => arg.name == this.selectedPage);
                id = p.id;
                token = p.access_token;
            }

            this.MS.DataStore.addToDataStore("FacebookPageId", id, DataStoreType.Private);
            this.MS.DataStore.addToDataStore("FacebookPageToken", token, DataStoreType.Private);
        } else {
            this.MS.DataStore.addToDataStore('FacebookClientId', this.facebookClientId, DataStoreType.Private);
            this.MS.DataStore.addToDataStore('FacebookClientSecret', this.facebookClientSecret, DataStoreType.Private);
        }
        this.MS.DataStore.addToDataStore("UserOwnsPage", this.ownsPage, DataStoreType.Public);
        return true;
    }

    async onValidate(): Promise<boolean> {
        this.onInvalidate();
        if (this.ownsPage) {
            let body: any = {};
            body.PageId = this.pageId;
            body.PermanentPageToken = this.permanentPageToken;

            if (this.pageId.length > 0 && this.permanentPageToken.length > 0) {
                this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateFacebookPermanentPageToken', body);
            }
        } else {
            if (this.facebookClientId.length > 0 && this.facebookClientSecret.length > 0) {
                this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateFacebookDeveloperAccount', {
                    FacebookClientId: this.facebookClientId,
                    FacebookClientSecret: this.facebookClientSecret
                });
            }
        }
        this.showValidation = this.isValidated;
        return this.isValidated;
    }

    onRadioChanged(): void {
        this.onInvalidate();
       
        if (!this.ownsPage) {
            //this.ownsPage = false;
            this.showOldLogin = true;
            this.facebookClientId = '';
            this.facebookClientSecret = '';
        }
       else {
            this.ownsPage = true;
            this.showOldLogin = false;
            this.pageId = '';
            this.permanentPageToken = '';
            //this.noOwnsPage = 'off';
        }
    }
}