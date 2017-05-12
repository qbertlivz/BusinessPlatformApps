import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class CognitiveText extends ViewModelBase {
    isTermsChecked: boolean = false;

    constructor() {
        super();
        this.isValidated = false;
    }

    verifyTerms() {
        this.isValidated = this.isTermsChecked;
    }

    async NavigatingNext(): Promise<boolean> {
        if (!super.NavigatingNext()) {
            return false;
        }
        return true;
    }

}