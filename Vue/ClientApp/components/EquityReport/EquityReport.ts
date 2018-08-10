import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';

interface StockReport
{
    symbol:string;
    close:number;
    change:number;
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
    startIndex: number = 0;
    elementsPerPage: number = 50;
    created(): void {
        if(stockReport.length == 0)
        {
            //this.symbol = this.$route.params.symbol;
            fetch('/api/StockData/GetLastestStockReport')
            .then(response => response.json() as Promise<StockReport[]>)
            .then(data => {
                stockReport = data;
                this.stockReport = stockReport;
            });
        }
        this.stockReport = stockReport;
    }

    mounted() {

    }

    ShowElements(more: boolean): void {
        if(more)
        {
            if((this.startIndex + 50) < this.stockReport.length)
                this.startIndex += 50;
        } else
        {
            this.startIndex -= 50;
            if(this.startIndex < 0)
                this.startIndex = 0;
        }
    }
}
