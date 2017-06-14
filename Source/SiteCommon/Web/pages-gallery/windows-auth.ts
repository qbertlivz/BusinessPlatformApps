import { DataStoreType } from '../enums/data-store-type';

import { ViewModelBase } from '../services/view-model-base';

export class WindowsAuth extends ViewModelBase {
    discoveredUsername: string = '';
    enteredUsername: string = '';
    logInAsCurrentUser: boolean = false;
    password: string = '';
    username: string = '';

    loginSelectionChanged(): void {
        this.onInvalidate();
        if (this.logInAsCurrentUser) {
            this.enteredUsername = this.username;
            this.username = this.discoveredUsername;
        } else {
            if (!this.enteredUsername) {
                this.username = this.discoveredUsername;
            } else {
                this.username = this.enteredUsername;
            }
        }
    }

    async onLoaded(): Promise<void> {
        this.isValidated = false;

        if (!this.username) {
            this.discoveredUsername = await this.MS.HttpService.getExecuteResponseAsync('Microsoft-GetCurrentUserAndDomain');
            this.loginSelectionChanged();
        }
    }

    async onValidate(): Promise<boolean> {
        this.isValidated = false;

        let usernameError: string = this.MS.UtilityService.validateUsername(this.username);
        if (usernameError) {
            this.MS.ErrorService.message = usernameError;
        } else {
            let domain: string = this.MS.UtilityService.extractDomain(this.username);
            let usernameWithoutDomain: string = this.MS.UtilityService.extractUsername(this.username);

            this.MS.DataStore.addToDataStore('ImpersonationDomain', domain, DataStoreType.Private);
            this.MS.DataStore.addToDataStore('ImpersonationUsername', usernameWithoutDomain, DataStoreType.Private);
            this.MS.DataStore.addToDataStore('ImpersonationPassword', this.password, DataStoreType.Private);

            this.isValidated = await this.MS.HttpService.isExecuteSuccessAsync('Microsoft-ValidateNtCredential');
        }

        return this.isValidated;
    }
}