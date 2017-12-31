import { ViewModelBase } from '../services/view-model-base';

export class CognitiveServicesTerms extends ViewModelBase {
    isTermsChecked: boolean = false;    

    verifyChecked() {
        this.isValidated = this.isTermsChecked;
    }    

    async onLoaded(): Promise<void> {
        this.isValidated = this.isTermsChecked;
    }
}