import { NewsEntity } from '../../../../../SiteCommon/Web/models/news-entity'

import { ViewModelBase } from '../../../../../SiteCommon/Web/services/view-model-base'

export class Customize extends ViewModelBase {
    entities: NewsEntity[] = [];
    selectedEntity: NewsEntity = null;

    async OnLoaded(): Promise<void> {
        this.isValidated = true;

        if (this.entities.length === 0) {
            this.entities[0] = new NewsEntity();
            this.selectedEntity = this.entities[0];
        }
    }
}