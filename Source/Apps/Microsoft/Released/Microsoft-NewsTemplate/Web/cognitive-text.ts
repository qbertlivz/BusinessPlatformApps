import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class CognitiveText extends ViewModelBase {
    isBingChecked: boolean = false;

    verifyBing() {
        this.isValidated = this.isBingChecked;
    }

    async NavigatingNext(): Promise<boolean> {
        return (await this.MS.HttpService.executeAsync('Microsoft-RegisterCognitiveServices', { CognitiveLocation: 'westus', CognitiveServices: 'TextAnalytics' })).IsSuccess &&
            (await this.MS.HttpService.executeAsync('Microsoft-RegisterCognitiveServices', { CognitiveLocation: 'global', CognitiveServices: 'Bing.Search' })).IsSuccess;
    }

    async OnLoaded(): Promise<void> {
        this.isValidated = this.isBingChecked;
    }
}