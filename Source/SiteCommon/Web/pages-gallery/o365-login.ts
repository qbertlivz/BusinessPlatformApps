import { AzureLogin } from './azure-login';

export class O365Login extends AzureLogin {
    hasToken: boolean = false;

    async connect(): Promise<void> {
        await this.MS.UtilityService.connectToAzure(this.openAuthorizationType.O365);
    }

    async onLoaded(): Promise<void> {
        this.onInvalidate();

        if (this.hasToken) {
            this.setValidated();
        } else {
            await this.MS.UtilityService.getToken(this.openAuthorizationType.O365, async () => {
                this.hasToken = this.setValidated();
            });
        }
    }
}