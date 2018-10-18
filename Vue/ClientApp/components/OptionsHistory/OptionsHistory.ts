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

interface IndexHistory {
    date:string;
    open: number;
    close: number;
    high: number;
    low: number;
    volume:number;
    turnOver: number;
}

class PlotParameters {
    xaxisString: string[] = [];
    yaxisValue: number[] = [];
}

@Component
export default class HistoryComponent extends Vue {
    callOption: string = "";
    expDate: string = "" ;
    strikePrice: number = 0;

    //Plot parameters
    indexPlot:PlotParameters = new PlotParameters();
    optionPricePlot:PlotParameters = new PlotParameters();

    // Fetched Results
    optionsReport:OptionsReport[] = [];
    indexHistory:IndexHistory[] = [];

    constructor()
    {
        super();
        Vue.filter('DateToString', function(date:string)  { if (date) { return Moment(date).format('DD-MM-YYYY'); }});
    }

    processOptionsReports(optionsReport: OptionsReport[]): OptionsReport[] {
        var i = 0;
        while(i < (optionsReport.length -1))
        {
            optionsReport[i].change = Math.round(100.0 * (optionsReport[i].close - optionsReport[i+1].close)/optionsReport[i+1].close);
            optionsReport[i].oi_change = Math.round(100.0 * (optionsReport[i].openIntrest - optionsReport[i+1].openIntrest)/optionsReport[i+1].openIntrest);
            optionsReport[i].notionalValue = Math.round(optionsReport[i].notionalValue/10000000);
            optionsReport[i].expiryDate = optionsReport[i].expiryDate;
            i++;
        }
        optionsReport[i].notionalValue = Math.round(optionsReport[i].notionalValue/10000000);
        optionsReport[i].expiryDate = optionsReport[i].expiryDate;
        return optionsReport;
    }

    created() {
        this.expDate = this.$route.params.expDate;
        this.strikePrice = parseInt(this.$route.params.strikePrice);
        this.callOption = this.$route.params.callOption;

        fetch('/api/StockData/GetNiftyIndexHistory?index='+ btoa("Nifty 50"))
        .then(response => response.json() as Promise<IndexHistory[]>)
        .then(data => {
            this.indexHistory = data;
            this.indexHistory.forEach(x => {
                this.indexPlot.xaxisString.push(x.date);
                this.indexPlot.yaxisValue.push(x.close);
            });
        })
        .then( x => {
            fetch('/api/StockData/GetNiftyOptionsDataFor?expiryDate='+this.expDate+"&strikePrice="+this.strikePrice+"&callOption="+this.callOption)
            .then(response => response.json() as Promise<OptionsReport[]>)
            .then(data => {
                this.optionsReport = this.processOptionsReports(data);
                this.optionsReport.forEach(x => {
                    this.optionPricePlot.xaxisString.push(x.expiryDate);
                    this.optionPricePlot.yaxisValue.push(x.close);
                });
                this.showGraphs(this.indexPlot, this.optionPricePlot);
            });
        });
    }


    private showGraphs(niftyIndex:PlotParameters,  optionPrice:PlotParameters) : void{

        const fullPlot:Plotly.Data[] = [
            {
                x: niftyIndex.xaxisString,
                y: niftyIndex.yaxisValue,
                //type: "bar",
                xaxis: "Date",
                name: "Nifty Index",
                marker: {
                    autocolorscale: true,
                }
            },
            {
                x: optionPrice.xaxisString,
                y: optionPrice.yaxisValue,
               // type: "bar",
                xaxis: "Date",
                yaxis: "y2",
                name: "Option price" ,
                marker: {
                    autocolorscale: true,
                },
                connectgaps: true
            },
        ];
        Plotly.newPlot('pricePlot', fullPlot, {
            title: "Nifty Options Plot",
            xaxis: { type: "date"},
            yaxis: { title: "Nift Index" },
            yaxis2: { side: "right", title: "Option Price", overlaying: 'y' as '/^y([2-9]|[1-9][0-9]+)?$/'},
         });
    }
}
