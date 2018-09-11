import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';
import * as Plotly from 'plotly.js';

interface StockHistory
{
    date : string;
    close : number;
    change : number;
    totQty : number;
    totTrades : number;
    totTraVal : number;
    totDelQty : number;
}

@Component
export default class HistoryComponent extends Vue {
    symbol: string = "";
    stockHistory:StockHistory[] = [];

    ChangeList: number[] = [];
    deliveredVolumeList: number[] = [];
    tradedVolumeList: number[] = [];
    priceList: number[] = [];
    tradesList: number[] = [];
    xaxisString: string[] = [];
    favList: string[] = [];
    colors: string[] = [];
    constructor()
    {
        super();
        Vue.filter('DateToString', function(date:string)  { if (date) { return Moment(date).format('DD-MM-YYYY'); }});
    }

    created() {
        this.symbol = this.$route.params.symbol

        fetch('/api/StockData/GetFavLists')
        .then(response => response.json() as Promise<string[]>)
        .then(data => {
            this.favList = data;
        });
        fetch('/api/StockData/GetStockHistory?symbol='+ btoa(this.symbol))
        .then(response => response.json() as Promise<StockHistory[]>)
        .then(data => {
            this.stockHistory = data;
            let referencePrice = this.stockHistory[this.stockHistory.length - 1].close;
            let avgDeliveredVolume = 0, avgTradedVolume = 0, avgTrades = 0;

            this.stockHistory.forEach(x => avgDeliveredVolume += x.totDelQty);
            this.stockHistory.forEach(x => avgTradedVolume += x.totQty);
            this.stockHistory.forEach(x => avgTrades += x.totTrades);

            avgDeliveredVolume /= this.stockHistory.length;
            avgTradedVolume /= this.stockHistory.length;
            avgTrades /= this.stockHistory.length;

            data.forEach(x => {
                this.xaxisString.push(x.date);
                this.ChangeList.push(this.getPct(x.close, referencePrice));
                /*this.deliveredVolumeList.push(this.getPct(x.totDelQty, avgDeliveredVolume));
                this.tradedVolumeList.push(this.getPct(x.totQty, avgTradedVolume));
                this.tradesList.push(this.getPct(x.totTrades, avgTrades));*/
                if(x.change > 0) {
                    this.colors.push('darkgreen');
                } else {
                    this.colors.push('red');
                }

                this.priceList.push(x.close);
                this.deliveredVolumeList.push(/*Math.log*/(x.totDelQty));
                this.tradedVolumeList.push(/*Math.log*/(x.totQty));
                this.tradesList.push(x.totTrades);
            });
            this.showGraphs(this.xaxisString, this.priceList, this.ChangeList, this.deliveredVolumeList, this.tradedVolumeList, this.tradesList);
        }).catch(error => {
            alert("Could not download data for " + this.symbol);
        });
    }

    mounted() {

    }

    private getPct(right: number, left: number): number {
        return (right - left) * 100.0/left;
    }


    yAxisType: Plotly.AxisType = "log";
    ToggleLogScale(): void {
        switch(this.yAxisType)
        {
            case "linear":
                this.yAxisType = "log";
                break;
            case "log":
                this.yAxisType = "linear";
                break;
        }
        this.showGraphs(this.xaxisString, this.priceList, this.ChangeList, this.deliveredVolumeList, this.tradedVolumeList, this.tradesList);
    }
    private showGraphs(xaxis: string[], price: number[], relativePrice: number[], deliveredVolume: number[], totalVolume: number[], totalTrades: number[]) : void{
        const plotPrice: Plotly.Data[] = [{ x: xaxis, y: relativePrice, xaxis: "Date", yaxis: "Price" }];
        const plotVolume: Plotly.Data[] = [
                {
                    x: xaxis,
                    y: deliveredVolume,
                    type: "bar",
                    xaxis: "Date",
                    name: "Delivered Qty" ,
                    marker: {
                        autocolorscale: true,
                        color: this.colors,
                        opacity: 0.5,
                    }
                },
                {
                    x: xaxis,
                    y: totalVolume,
                    type: "bar",
                    xaxis: "Date",
                    name: "Traded Qty" ,
                    marker: {
                        autocolorscale: true,
                        color: this.colors,
                        opacity: 0.2,
                    },
                    connectgaps: true
                },
                {
                    x: xaxis,
                    y: totalTrades,
                    type: "scatter",
                    xaxis: "Date",
                    name: "Trades" ,
                    yaxis: "y3",
                    marker: {
                        color: "#6C3483",
                        opacity: 0.5,
                    },
                    connectgaps: true
                },
                {
                    x: xaxis,
                    y:  price,
                    type: "scatter",
                    xaxis: "Date",
                    name: "Price",
                    yaxis: "y2",
                    //fill: 'tonexty',
                    marker: {
                        color: 'black',
                        size: 15
                    },
                }
                ///{ x: xaxis, y: totalVolume, xaxis: "Date", yaxis: "TradedVolume",  name: "Traded Qty" },
                //{ x: xaxis, y: totalTrades, xaxis: "Date", yaxis: "Trades",  name: "Trades" },
            ];
        Plotly.newPlot('pricePlot', plotPrice, {title: "Price Plot"});
        Plotly.newPlot('volumePlot', plotVolume, {
                                                    title: "Volume Plot",
                                                    xaxis:
                                                    {
                                                        type: "date",
                                                        //rangeslider: {range: [this.stockHistory[0].date, this.stockHistory[this.stockHistory.length-1].date]},
                                                    },
                                                    yaxis:
                                                    {
                                                        title: "Qty (log)",
                                                        type: this.yAxisType
                                                    },
                                                    yaxis3:
                                                    {
                                                        side: "left", overlaying: 'y' as '/^y([2-9]|[1-9][0-9]+)?$/',
                                                        title: "Trades",
                                                        type: this.yAxisType
                                                    },
                                                    yaxis2:
                                                    {
                                                        side: "right", overlaying: 'y' as '/^y([2-9]|[1-9][0-9]+)?$/',
                                                        title: "Price"
                                                    },
                                                    barmode: "stack"
                                                 });
    }

    showDays(days: number): void {
        if(days == 0) days = -1;
        this.showGraphs(this.xaxisString.slice(0, days),
                        this.priceList.slice(0, days),
                        this.ChangeList.slice(0, days),
                        this.deliveredVolumeList.slice(0, days),
                        this.tradedVolumeList.slice(0, days),
                        this.tradesList.splice(0, days));
    }

    AddToFavList(listName: string, promt: Boolean = false): void {
        if(promt)
        {
            var str = "";
            str = prompt("Enter List Name") || "";
            if(str == "") return;
            listName = str;
        }

        if(confirm("Adding " + this.symbol + " to " + listName))
        {
            fetch('/api/StockData/AddToFavList?Symbol='+btoa(this.symbol)+"&FavList="+btoa(listName))
            .then(response => response.json() as Promise<number>)
            .then(data => {
                if(data == 1) alert("Added To DB");
                else alert("Unable to add to database");
            });
        }
    }
}
