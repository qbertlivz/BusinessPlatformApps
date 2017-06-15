import { MainService } from './main-service';

export class ErrorService {
    MS: MainService;
    details: string = '';
    logLocation: string = '';
    message: string = '';
    showContactUs: boolean = false;

    constructor(MainService: MainService) {
        this.MS = MainService;
    }

    clear(): void {
        this.details = '';
        this.logLocation = '';
        this.message = '';
        this.showContactUs = false;
    }

    set(message: string, details: string = '', showContactUs: boolean = true, logLocation: string = ''): void {
        this.details = details;
        this.logLocation = logLocation;
        this.message = message;
        this.showContactUs = showContactUs;
    }
}