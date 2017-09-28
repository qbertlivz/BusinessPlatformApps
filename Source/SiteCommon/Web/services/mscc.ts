export class Mscc {
    cookieName: string = 'MSCC';
    interactiveConsentEnabled: boolean = true;
    version: string = '0.3.6';

    private bannerElement: any;
    private bannerShownClass: any;
    private hasLogged: boolean = false;
    private events: any;
    private globalMinReconsentDate: any;
    private hostname: string = window.location.hostname;
    private interactiveConsentAttribute: string = 'data-mscc-ic';
    private learnMoreLinkElement: any;
    private shouldLog: boolean = true;
    private telemetryUrl = 'https://uhf.microsoft.com/_log';

    constructor(refBanner: any, refLearnMore: any) {
        this.bannerElement = refBanner;
        this.learnMoreLinkElement = refLearnMore;

        this.globalMinReconsentDate = this.nsToMs((new Date('Sun, 01 Jan 2016 08:00:00 GMT')).getTime());

        this.initDomNeeded();
    }

    hasConsent(): boolean {
        return !!this.getCookie(this.cookieName);
    }

    isVisible(): boolean {
        return this.bannerElement != undefined && this.hasClass(this.bannerElement, this.bannerShownClass);
    }

    on(name: string, fn: () => any): void {
        if (!this.events[name]) {
            this.events[name] = [fn];
        } else {
            this.events[name].push(fn);
        }
    }

    setConsent(): void {
        let hasConsentNow = this.hasConsent();
        if (this.bannerElement) {
            this.hideBanner();
        }
        // We only want to emit consent given if it wasn't already given.
        if (!hasConsentNow) {
            this.setCookieOnRootDomain(this.cookieName, '' + this.nsToMs(Date.now()), 390);
            this.emit('consent');
        }
    }

    private addClass(el: any, className: string): void {
        if (el.classList) {
            el.classList.add(className);
        } else {
            el.className += ' ' + className;
        }
    }

    private addEventListener(el: any, eventName: string, handler: any): void {
        if (el.addEventListener) {
            el.addEventListener(eventName, handler.bind(this));
        } else {
            el.attachEvent('on' + eventName, () => {
                handler.call(el);
            });
        }
    }

    private emit(name: string): void {
        let args: any[] = [];

        for (let i = 1; i < arguments.length; i++) {
            args[i - 1] = arguments[i];
        }

        if (!this.events[name]) {
            return;
        }

        this.events[name].forEach((x: any) => {
            x.apply(null, args);
        });
    }

    private getCookie(name: string): string {
        if (!!name) {
            for (let i = 0, a = document.cookie.split('; '); i < a.length; i++) {
                let cookie = a[i];
                let delimiterIndex = cookie.indexOf('=');
                let cookieName_1 = cookie.substring(0, delimiterIndex);
                if (cookieName_1 === name) {
                    return cookie.substring(cookieName_1.length + 1);
                }
            }
        }
        return null;
    }

    private getExplicitInteractiveConsentValue(target: any): number {
        if (target && target.getAttribute) {
            var value = target.getAttribute(this.interactiveConsentAttribute);
            if (value === 'false') {
                return 0 /* OFF */;
            }
            else if (value === 'true') {
                return 1 /* ON */;
            }
        }
        return -1 /* UNDEFINED */;
    }

    private handleInteractiveConsentEvent(evt: any): void {
        let explicitConsentValue: any = this.getExplicitInteractiveConsentValue(evt.target);
        let button: any = evt.button;
        if (!this.interactiveConsentEnabled ||
            explicitConsentValue === 0 /* OFF */ ||
            !(this.wasEnterEvent(evt) || button === 0 || button === 1)) {
            return;
        }
        if (explicitConsentValue === 1 /* ON */ || this.wasConsentLinkEvent(evt, null, this.hostname)) {
            this.setConsent();
        }
    }

    private hasClass(el: any, className: string): boolean {
        if (el.classList) {
            return el.classList.contains(className);
        } else {
            return new RegExp('(^| )' + className + '( |$)', 'gi').test(el.className);
        }
    }

    private hideBanner(): void {
        this.removeClass(this.bannerElement, this.bannerShownClass);
        this.emit('hide');
    }

    private initDomNeeded(): void {
        let hasConsentNow: boolean = this.hasConsent();
        // The banner is hidden and we need consent
        if (this.bannerElement && !hasConsentNow && !this.isVisible()) {
            this.showBanner();
        }
        // The banner is showing and we have consent
        if (this.bannerElement && hasConsentNow && this.isVisible()) {
            this.hideBanner();
        }
        // We want to handle interactive consent
        if (!hasConsentNow) {
            this.addEventListener(document.body, 'mouseup', this.handleInteractiveConsentEvent);
            this.addEventListener(document.body, 'keyup', this.handleInteractiveConsentEvent);
            this.addEventListener(document.body, 'submit', this.setConsent);
        }
        // Invalidate consent on the next page load if consent is expired. The reason we
        // reset before the next page load is because unexpected behaviour can occur if we
        // suddenly revoke consent. The partner likely would not have handled this scenario.
        // This is okay to wait on, since revocation doesn't need to happen within such a
        // precise timeline. Waiting one more request is good enough.
        var consentDate = this.getCookie(this.cookieName);
        if (!!consentDate && parseInt(consentDate) < this.globalMinReconsentDate) {
            this.addEventListener(window, 'beforeunload', () => {
                this.setCookieOnRootDomain(this.cookieName, '0', -1);
            });
        }
    }

    private log(message: string): void {
        if (!this.shouldLog || this.hasLogged) {
            return;
        }
        let image: any = new Image();
        let siteName: any = this.bannerElement.getAttribute('data-site-name');
        let nugetVersion: any = this.bannerElement.getAttribute('data-nver');
        let settingsVersion: any = this.bannerElement.getAttribute('data-sver');
        let siteParam: string = !!siteName ? '&s=' + siteName : '';
        let nugetParam: string = !!nugetVersion ? '&nv=' + nugetVersion : '';
        let settingsParam: string = !!settingsVersion ? '&sv=' + settingsVersion : '';
        let messageParam: string = !!message ? '&m=' + message : '';
        image.src = this.telemetryUrl + '?o=mscc' + siteParam + messageParam + nugetParam + settingsParam;
        this.hasLogged = true;
    }

    private nsToMs(nanoSeconds: any): any {
        return Math.floor(nanoSeconds / 1000);
    }

    private removeClass(el: any, className: string): void {
        if (el.classList) {
            el.classList.remove(className);
        } else {
            el.className = el.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
        }
    }

    private setCookieOnRootDomain(name: string, value: string, expiryDays: any): void {
        let expiryDate = new Date();
        expiryDate.setDate(expiryDate.getDate() + expiryDays);
        let domainParts = this.hostname.split('.'), testDomain = domainParts.pop();
        if (testDomain == 'localhost') {
            document.cookie = name + '=' + value + ';expires=' + expiryDate.toUTCString() + ';path=/';
        } else {
            while (this.getCookie(name) !== value && domainParts.length !== 0) {
                testDomain = domainParts.pop() + '.' + testDomain;
                document.cookie = name + '=' + value + ';expires=' + expiryDate.toUTCString() + ';path=/;domain=.' + testDomain;
            }
        }
    }

    private showBanner() {
        this.addClass(this.bannerElement, this.bannerShownClass);
        this.emit('show');
        this.log('show');
    }

    private wasConsentLinkEvent(e: any, element: any, hostname: string): boolean {
        if (!e && !element) {
            return false;
        }
        // support IE8;
        var target = (e && e.target) || element;
        // Check again for the consent blocking attribute, in case click bubbled
        if (target && this.getExplicitInteractiveConsentValue(target) === 0 /* OFF */) {
            return false;
        }
        // Target is not an anchor or descendant of one
        if (!target || target.tagName !== 'A') {
            return this.wasConsentLinkEvent(null, target.parentElement, hostname);
        }
        // Target is part of the mscc banner itself
        if (target === this.learnMoreLinkElement) {
            return false;
        }
        // Target meets all criteria.
        return true;
    }

    private wasEnterEvent(evt: any): boolean {
        return evt.which === 13 || evt.keyCode === 13;
    }
}