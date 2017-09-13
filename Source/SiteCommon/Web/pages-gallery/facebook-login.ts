import { DataStoreType } from '../enums/data-store-type';
import { ViewModelBase } from '../services/view-model-base';

export class AzureLogin extends ViewModelBase {
    authToken: any = {};
    oauthType: string = '';
    pages: any[] = [];
    selectedPage: any;
    pageId: string = '';
    permanentPageToken: string = '';
    ownsPage: boolean = false;

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToFacebook();
        this.pageId = '';
        this.permanentPageToken = '';
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();
        this.oauthType = "Facebook";
        await this.MS.UtilityService.getToken(this.oauthType, async () => { this.setValidated(); });
        this.pages = this.MS.DataStore.getJson("data");
        if (this.pages.length > 0) {
            this.selectedPage = this.pages[0];
        }        
    }

    async onNavigatingNext(): Promise<boolean> {

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
        return true;
    }

    async onValidate(): Promise<boolean> {
        this.onInvalidate();

        let body: any = {};
        body.PageId = this.pageId;
        body.PermanentPageToken = this.permanentPageToken;

        this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateFacebookPermanentPageToken', body);

        this.showValidation = this.isValidated;

        return this.isValidated;
    }

    onRadioChanged(): void {
        this.ownsPage = false;
        //this.own
    }
    
}