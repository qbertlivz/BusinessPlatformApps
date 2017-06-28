import { InitParser } from './init-parser';
import { MainService } from './main-service';

export class NavigationService {
    appName: string = '';
    currentViewModel: any = null;
    index: number = -1;
    isCurrentlyNavigating: boolean = false;
    isOnline: boolean = true;
    MS: MainService;
    pages: any[] = [];

    constructor(MainService: MainService) {
        this.MS = MainService;
    }

    activate(): void {
        this.updateIndex();
        this.MS.DataStore.CurrentRoutePage = this.pages[this.index].RoutePageName.toLowerCase();
    }

    getCurrentRoutePath(): string {
        let history: any = this.MS.Router.history;
        let route: string = history.location.hash;
        let routePage = this.MS.NavigationService.appName + route.replace('#', '');
        if (routePage.endsWith('/')) {
            routePage += '//';
            routePage.replace('///', '');
        }

        return routePage;
    }

    getCurrentSelectedPage(): any {
        return this.pages[this.index];
    }

    getIndex(): number {
        return this.index;
    }

    getRoute(): string {
        let history: any = this.MS.Router.history;
        let route: string = history.location.hash;
        return route.replace('#', '').replace('/', '');
    }

    init(pagesJson: any): void {
        this.pages = pagesJson;

        if (this.pages && this.pages.length && this.pages.length > 0) {
            this.index = 0;

            for (let i = 1; i < this.pages.length; i++) {
                this.MS.Router.addRoute({
                    route: this.pages[i].RoutePageName.toLowerCase(),
                    name: this.pages[i].PageName,
                    moduleId: '.' + this.pages[i].Path.replace(/\\/g, "/"),
                    title: this.pages[i].DisplayName,
                    nav: true
                });
            }

            this.MS.Router.addRoute({
                route: '',
                name: this.pages[0].PageName,
                moduleId: '.' + this.pages[0].Path.replace(/\\/g, "/"),
                title: this.pages[0].DisplayName,
                nav: true
            });

            this.pages[0].isActive = true;
            this.pages[0].RoutePageName = '';
        }

        this.updateIndex();
        this.MS.DataStore.CurrentRoutePage = this.pages[this.index].RoutePageName.toLowerCase();
        this.MS.LoggerService.trackPageView(this.getCurrentRoutePath(), window.location.href);
    }

    isFirstPage(): boolean {
        return this.getIndex() === 0;
    }

    isLastPage(): boolean {
        return this.pages.length - 1 === this.getIndex();
    }

    jumpTo(index: any): void {
        this.index = index;
        this.navigateToIndex();
    }

    navigateBack(): void {
        this.updateIndex();
        if (this.index == 0) {
            return;
        }
        this.index = this.index - 1;

        // If you skip the last page then we should throw an error - no check in place - to be added
        while (this.pages[this.index].Parameters.skip && this.index > 0) {
            let body: any = {};
            InitParser.loadVariables(body, this.pages[this.index].Parameters, this.MS, this);
            if (body.skip && body.skip.toString().toLowerCase() === 'true') {
                this.index = this.index - 1;
                continue;
            }
            break;
        }
        this.navigateToIndex();
    }

    navigateHome(): void {
        this.index = 0;
        this.pages[0].isActive = true;
        let body: any = {};
        InitParser.loadVariables(body, this.pages[this.index].Parameters, this.MS, this);
        setTimeout(() => {
            this.navigateToIndex();
        }, 100);
    }

    navigateNext(): void {
        this.updateIndex();
        if (this.index >= this.pages.length - 1 && this.index < this.pages.length - 1) {
            return;
        }
        this.index = this.index + 1;

        // If you skip the last page then we should throw an error - no check in place - to be added
        while (this.pages[this.index].Parameters.skip) {
            let body: any = {};
            InitParser.loadVariables(body, this.pages[this.index].Parameters, this.MS, this);
            if (body.skip && body.skip.toString().toLowerCase() === 'true') {
                this.index = this.index + 1;
                continue;
            }
            break;
        }

        this.navigateToIndex();
    }

    navigateToIndex(): void {
        // Initialize the page
        this.MS.DataStore.CurrentRoutePage = this.pages[this.index].RoutePageName.toLowerCase();

        // The index is set to the next step
        this.MS.Router.navigate('#/' + this.pages[this.index].RoutePageName.toLowerCase());

        this.updateIndex();
        this.MS.LoggerService.trackPageView(this.appName + '/' + this.pages[this.index].RoutePageName.toLowerCase(), window.location.href);
    }

    updateIndex(): any {
        let routePageName = this.getRoute();
        for (let i = 0; i < this.pages.length; i++) {
            if (this.pages[i].RoutePageName.toLowerCase() === routePageName.toLowerCase()) {
                this.index = i;
            }
        }
        for (let i = 0; i < this.pages.length; i++) {
            this.pages[i].isActive = i === this.index;
            this.pages[i].isComplete = i < this.index;
        }
        return this.index;
    }
}