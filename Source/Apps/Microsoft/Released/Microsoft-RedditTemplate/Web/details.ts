import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'
import { Registration } from './registration';

export class Details extends ViewModelBase {
    registration: Registration = new Registration();

    async onLoaded(): Promise<void> {
        // this.isValidated = false;
    }

    async onValidate(): Promise<boolean> {
        super.onInvalidate();

        let error: string = "";

        if (this.registration.nameFirst.length === 0 ||
            this.registration.nameLast.length === 0 ||
            this.registration.company.length === 0 ||
            this.registration.jobTitle.length === 0 ||
            this.registration.descriptionOfUse.length === 0) {
            error += "All fields are required.";
        }

        if (this.registration.emailConfirmation.length === 0 ||
            this.registration.email.length === 0 ||
            this.registration.email !== this.registration.emailConfirmation ||
            this.registration.email.indexOf('@') === -1) {
            error += " Email and Email Confirmation are both required and must match";
        }

        if (!this.registration.accepted) {
            error += " You must accept the correspondence terms";
        }

        if (error.length !== 0) {
            this.MS.ErrorService.message = error;
            this.isValidated = false;
        } else {
            this.isValidated = true;
        }

        return this.isValidated;
    }
}