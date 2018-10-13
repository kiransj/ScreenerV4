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
            this.optionsReport = data;
            var i = 0;
            while(i < (this.optionsReport.length -1))
            {
                this.optionsReport[i].change = Math.round(100.0 * (this.optionsReport[i].close - this.optionsReport[i+1].close)/this.optionsReport[i+1].close);
                this.optionsReport[i].oi_change = Math.round(100.0 * (this.optionsReport[i].openIntrest - this.optionsReport[i+1].openIntrest)/this.optionsReport[i+1].openIntrest);
                this.optionsReport[i].notionalValue = Math.round(this.optionsReport[i].notionalValue/10000000);
                this.optionsReport[i].expiryDate = Moment(this.optionsReport[i].expiryDate).format('DD-MM-YYYY');
                i++;
            }
            this.optionsReport[i].notionalValue = Math.round(this.optionsReport[i].notionalValue/10000000);
            this.optionsReport[i].expiryDate = Moment(this.optionsReport[i].expiryDate).format('DD-MM-YYYY');
        });
    }
}
