import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class Details extends ViewModelBase {
    async onLoaded(): Promise<void> {
        // this.isValidated = false;
    }

    async onValidate(): Promise<boolean> {
        super.onInvalidate();

        this.MS.ErrorService.set(`Must have between 0 and 32 entity aliases in total.`);

        return this.isValidated;
    }
}