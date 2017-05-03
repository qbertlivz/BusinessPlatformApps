import { inject } from 'aurelia-framework';

import { MainService } from './SiteCommon/Web/services/main-service';

@inject(MainService)
export class App {
    MS: MainService;

    constructor(MainService: MainService) {
        this.MS = MainService;
    }

    async configureRouter() {
        await this.MS.init();
    }
}