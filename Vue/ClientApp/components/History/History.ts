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

    price: number[] = [];
    volume: number[] = [];
    xaxis: string[] = [];
    constructor()
    {
        super();
        Vue.filter('DateToString', function(date:string)  {
            if (date) { return Moment(date).format('DD-MM-YYYY'); }
        });
    }

    created() {
        this.symbol = this.$route.params.symbol
        fetch('/api/StockData/GetStockHistory?symbol='+btoa(this.symbol))
        .then(response => response.json() as Promise<StockHistory[]>)
        .then(data => {
            this.stockHistory = data;
            data.forEach(x => {
                this.xaxis.push(x.date);
                this.price.push(x.change);
                this.volume.push(x.totDelQty);
            });
            this.showGraphs(this.xaxis, this.price, this.volume);
        });
    }

    mounted() {

    }

    private getPct(right: number, left: number): number {
        return (right - left) * 100.0/left;
    }

    private showGraphs(xaxis: string[], price: number[], volume: number[]) : void{
        const plotPrice: Plotly.Data[] = [{ x: xaxis, y: price, xaxis: "Date", yaxis: "Price" }];
        const plotVolume: Plotly.Data[] = [{ x: xaxis, y: volume, xaxis: "Date", yaxis: "Volume" }];
        Plotly.newPlot('pricePlot', plotPrice, {title: "Price Plot"});
        Plotly.newPlot('volumePlot', plotVolume, {title: "Volume Plot"});
    }
}
