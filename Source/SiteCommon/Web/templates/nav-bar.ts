/// <reference path="../../../Site/Microsoft.Deployment.Site.Web/typings/index.d.ts" />

import { bindable } from 'aurelia-framework';

export class NavBar {
    @bindable router: any = null;
    @bindable viewmodel: any = null;
}