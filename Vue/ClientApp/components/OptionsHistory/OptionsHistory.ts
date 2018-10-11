import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';
import * as Plotly from 'plotly.js';

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

@Component
export default class HistoryComponent extends Vue {
    callOption: string = "";
    expDate: string = "" ;
    strikePrice: number = 0;

    optionsReport:OptionsReport[] = [];

    constructor()
    {
        super();
        Vue.filter('DateToString', function(date:string)  { if (date) { return Moment(date).format('DD-MM-YYYY'); }});
    }

    created() {
        this.expDate = this.$route.params.expDate;
        this.strikePrice = parseInt(this.$route.params.strikePrice);
        this.callOption = this.$route.params.callOption;
        fetch('/api/StockData/GetNiftyOptionsDataFor?date='+this.expDate+"&strikePrice="+this.strikePrice+"&callOption="+this.callOption)
        .then(response => response.json() as Promise<OptionsReport[]>)
        .then(data => {
            data.forEach(x => {
                x.change = Math.round(100.0 * (x.close - x.open)/x.open);
                x.notionalValue = Math.round(x.notionalValue/10000000);
                x.oi_change = Math.round(100*(x.openIntrest - x.openInterestPrev)/x.openInterestPrev);
                x.expiryDate = Moment(x.expiryDate).format('DD-MM-YYYY')
            });
            this.optionsReport = data;
        });
    }
}
