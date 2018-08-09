import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';

interface StockReport
{
    symbol:string;
    close:number;
    prevClose:number;
    totQty:number;
    totTrades:number;
    totTraVal:number;
    totDelQty:number;
    circuitBreaker:string;
    upDown:string;
    high52week:number;
    low52week:number;
}

let stockReport:StockReport[] = [];
// Lets save the UI state statically
@Component
export default class ReportComponent extends Vue {
    stockReport:StockReport[] = [];
    created(): void {
        //this.symbol = this.$route.params.symbol;
        fetch('/api/StockData/GetLastestStockReport')
        .then(response => response.json() as Promise<StockReport[]>)
        .then(data => {
            stockReport = this.stockReport = data;
        });
    }

    mounted() {

    }
}
