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
}

let optionsReport: OptionsReport[] = [];
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
        if(this.optionsReport.length == 0)
        {
            //this.symbol = this.$route.params.symbol;
            fetch('/api/StockData/GetLatestNiftyOptionsData')
            .then(response => response.json() as Promise<OptionsReport[]>)
            .then(data => {
                optionsReport = this.optionsReport = data;
                this.dates = data.map(x => x.expiryDate);
                this.dates = this.dates.filter((el, i, a) => i === a.indexOf(el))
            });
        }
    }

    mounted() {

    }

    ShowOptionsForExpiryDate(date: string): void {
        this.currentDisplayedDate = date;
        if(date.length != 0)
        {
            var d = Moment(date).format('DD-MM-YYYY');
            this.currentDisplayedDate = d;
            this.optionsReport = optionsReport.filter(x => Moment(x.expiryDate).format('DD-MM-YYYY').localeCompare(d) == 0);
        }
        else
            this.optionsReport = optionsReport;
    }

    ShowOptionsType(callType: number) {
        if(callType) {
            this.optionsReport = this.optionsReport.filter(x => x.callOptions);
        } else {
            this.optionsReport = this.optionsReport.filter(x => !x.callOptions);
        }
    }
}
