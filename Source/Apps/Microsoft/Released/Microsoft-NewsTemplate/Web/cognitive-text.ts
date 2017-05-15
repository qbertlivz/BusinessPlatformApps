import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class CognitiveText extends ViewModelBase {
    isBingChecked: boolean = false;

    constructor() {
        super();
        this.isValidated = false;
    }

    verifyBing() {
        this.isValidated = this.isBingChecked;
    }

    async NavigatingNext(): Promise<boolean> {
        let body: any = {};
        body.CognitiveServices = "TextAnalytics";
        body.CognitiveLocation = "westus";
        let response = await this.MS.HttpService.executeAsync('Microsoft-RegisterCognitiveServices', body);
        if (!response.IsSuccess) {
            return false;
        }

        body = {};
        body.CognitiveServices = "Bing.Search";
        body.CognitiveLocation = "global";
        response = await this.MS.HttpService.executeAsync('Microsoft-RegisterCognitiveServices', body);
        if (!response.IsSuccess) {
            return false;
        }

        return true;
    }

}