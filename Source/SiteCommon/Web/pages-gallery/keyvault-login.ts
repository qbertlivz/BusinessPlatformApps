import { AzureLogin } from './azure-login';

export class KeyVaultLogin extends AzureLogin {
    hasToken: boolean = false;

    async connect(): Promise<void> {
        this.MS.UtilityService.connectToAzure(this.oauthType, this.MS.Translate.DEFAULT_TENANT);
    }

    async onLoaded(): Promise<void> {
        super.onLoaded();

        if (this.hasToken) {
            this.setValidated();
        } else {
            await this.MS.UtilityService.getToken(this.oauthType, async () => {
                this.hasToken = this.setValidated();
            });
        }
    }
}