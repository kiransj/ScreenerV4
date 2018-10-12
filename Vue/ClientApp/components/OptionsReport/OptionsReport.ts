import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';

interface OptionsReport {
    optionsId: number;
    expiryDate: string;
    strikePrice: number;
    callOptions: boolean;
    open: number;
    close: number;
    high: number;
    low: number;
    openIntrest: number;
    tradedQty: number;
    numOfCont: number;
    numOfTrade: number;
    notionalValue: number;
    openInterestPrev: number;
    change: number;
    oi_change: number;
}

class UIState {
    optionReport: OptionsReport[] = [];
    currentDisplayedDate: string = "";
    dates: string[] = [];
    callType: number = 0;
    sortKey:string = "";
    sortReverse:number = -1;
}

let uiState:UIState = new UIState();
// Lets save the UI state statically
@Component
export default class OptionsReportComponent extends Vue {
    currentDisplayedDate: string = "";
    optionsReport: OptionsReport[] = [];
    dates: string[] = [];
    constructor()
    {
        super();
        Vue.filter('DateToString', function(date:string)  { if (date) { return Moment(date).format('DD-MM-YYYY'); }});
    }

    created(): void {
        if(uiState.optionReport.length == 0)
        {
            //this.symbol = this.$route.params.symbol;
            fetch('/api/StockData/GetLatestNiftyOptionsData')
            .then(response => response.json() as Promise<OptionsReport[]>)
            .then(data => {
                data.forEach(x => {
                    x.change = Math.round(100.0 * (x.close - x.open)/x.open);
                    x.notionalValue = Math.round(x.notionalValue/10000000);
                    x.oi_change = Math.round(100*(x.openIntrest - x.openInterestPrev)/x.openInterestPrev);
                    x.expiryDate = Moment(x.expiryDate).format('DD-MM-YYYY')
                });
                this.dates = data.map(x => x.expiryDate);
                this.dates = this.dates.filter((el, i, a) => i === a.indexOf(el));

                uiState.optionReport = this.optionsReport = data;
                uiState.dates = this.dates;
            });
        } else {
            this.sortReverse = uiState.sortReverse;
            this.ShowOptionsForExpiryDate(uiState.currentDisplayedDate);
            this.ShowOptionsType(uiState.callType);
            this.sortBy(uiState.sortKey, false);
            this.dates = uiState.dates;
        }
    }

    mounted() {

    }

    ShowOptionsForExpiryDate(date: string): void {
        this.currentDisplayedDate = date;
        this.currentDisplayedDate = uiState.currentDisplayedDate = date;
        if(date.length != 0)
            this.optionsReport = uiState.optionReport.filter(x => x.expiryDate.localeCompare(date) == 0);
        else
            this.optionsReport = uiState.optionReport;
    }

    callType: number = -1;
    ShowOptionsType(callType: number) {
        uiState.callType = this.callType = callType;
        this.ShowOptionsForExpiryDate(this.currentDisplayedDate);
        if(callType == 0) {
            this.optionsReport = this.optionsReport.filter(x => x.callOptions);
        } else if(callType == 1)  {
            this.optionsReport = this.optionsReport.filter(x => !x.callOptions);
        }
    }


    sortKey:string = "";
    sortReverse:number = -1;
    sortBy(sortKey: string, directionChange:Boolean = true): void  {
        this.sortKey = sortKey;
        if(directionChange) {
            this.sortReverse *= -1;
        }
        uiState.sortKey = this.sortKey;
        uiState.sortReverse = this.sortReverse;
        switch (sortKey) {
            case "strikePrice": case "openIntrest": case "change": case "oi_change": case "openInterestPrev":
            case "tradedQty": case "numOfCont": case "numOfTrade": case "notionalValue": case "close":
                this.optionsReport = this.optionsReport.sort((left, right): number => (left[sortKey] - right[sortKey]) * this.sortReverse);
                break;
        }
    }
}
