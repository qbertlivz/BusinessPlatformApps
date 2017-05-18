import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class Customize extends ViewModelBase {
    async OnLoaded(): Promise<void> {
        this.isValidated = true;
    }
}