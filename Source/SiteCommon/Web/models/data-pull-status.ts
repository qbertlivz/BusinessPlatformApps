export class DataPullStatus {
    isFinished: boolean;
    slices: any[];
    status: any[];

    constructor(dataPullStatus: any) {
        this.isFinished = dataPullStatus.isFinished;

        try {
            if (dataPullStatus.status) {
                this.status = JSON.parse(dataPullStatus.status);
            }

            if (dataPullStatus.slices) {
                this.slices = JSON.parse(dataPullStatus.slices);
            }
        } catch (e) {
            // do nothing
        }
    }
}