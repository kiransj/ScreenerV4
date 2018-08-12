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
    tradesList: number[] = [];
    xaxisString: string[] = [];
    favList: string[] = [];

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
                this.deliveredVolumeList.push(this.getPct(x.totDelQty, avgDeliveredVolume));
                this.tradedVolumeList.push(this.getPct(x.totQty, avgTradedVolume));
                this.tradesList.push(this.getPct(x.totTrades, avgTrades));
            });
            this.showGraphs(this.xaxisString, this.ChangeList, this.deliveredVolumeList, this.tradedVolumeList, this.tradesList);
        });
    }

    mounted() {

    }

    private getPct(right: number, left: number): number {
        return (right - left) * 100.0/left;
    }

    private showGraphs(xaxis: string[], price: number[], deliveredVolume: number[], totalVolume: number[], totalTrades: number[]) : void{
        const plotPrice: Plotly.Data[] = [{ x: xaxis, y: price, xaxis: "Date", yaxis: "Price" }];
        const plotVolume: Plotly.Data[] = [
                { x: xaxis, y: deliveredVolume, xaxis: "Date", yaxis: "DeliveredVolume", name: "Delivered Qty" },
                { x: xaxis, y: totalVolume, xaxis: "Date", yaxis: "TradedVolume",  name: "Traded Qty" },
                { x: xaxis, y: totalTrades, xaxis: "Date", yaxis: "Trades",  name: "Trades" },
            ];
        Plotly.newPlot('pricePlot', plotPrice, {title: "Price Plot"});
        Plotly.newPlot('volumePlot', plotVolume, {title: "Volume Plot"});
    }

    showDays(days: number): void {
        if(days == 0) days = -1;
        this.showGraphs(this.xaxisString.slice(0, days),
                        this.ChangeList.slice(0, days),
                        this.deliveredVolumeList.slice(0, days),
                        this.tradedVolumeList.slice(0, days),
                        this.tradesList.splice(0, days));
    }

    AddToFavList(listName: string): void {
        fetch('/api/StockData/AddToFavList?Symbol='+btoa(this.symbol)+"&FavList="+btoa(listName))
        .then(response => response.json() as Promise<number>)
        .then(data => {
            if(data == 1) alert("Added To DB");
            else alert("Unable to add to database");
        });
    }
}
