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
    change5d: number;
    change30d: number;
    hlp: number;
    delQtyChange: number;
}

interface FavLists
{
    symbol: string;
    listName: string;
}

let stockReport:StockReport[] = [];
// Lets save the UI state statically
@Component
export default class ReportComponent extends Vue {
    stockReport:StockReport[] = [];
    favLists:FavLists[] = [];
    favListNames: string[] = [];

    startIndex: number = 0;
    elementsPerPage: number = 25;
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

        // Get list and symbols
        fetch('/api/StockData/GetFavListWithSymbols')
        .then(response => response.json() as Promise<FavLists[]>)
        .then(data => { this.favLists = data; });

        fetch('/api/StockData/GetFavLists')
        .then(response => response.json() as Promise<string[]>)
        .then(data => {
            this.favListNames = data;
        });

        this.stockReport = stockReport;
    }

    mounted() {

    }

    ShowElements(more: boolean): void {
        if(more)
        {
            if((this.startIndex + 25) < this.stockReport.length)
                this.startIndex += 25;
        } else
        {
            this.startIndex -= 25;
            if(this.startIndex < 0)
                this.startIndex = 0;
        }
    }

    showAll(): void {
        this.elementsPerPage = (this.elementsPerPage == 25) ? 100000 : 25;
        if(this.elementsPerPage == 100000) {
            this.startIndex = 0;
        }
    }

    searchQuery:string = "";
    onSearch(): void {
        this.stockReport = this.searchQuery.length == 0 ? stockReport: (stockReport.filter(x => (x.symbol.toLowerCase().indexOf(this.searchQuery.toLowerCase()) >= 0 || x.upDown.toLowerCase().indexOf(this.searchQuery.toLowerCase()) >= 0)));
        this.startIndex = 0;
        this.elementsPerPage = 25;
    }

    sortReverse: number = -1;
    sortBy(sortKey: string): void  {
        this.sortReverse *= -1;
        switch (sortKey) {
            case "totQty": case "hlp":
            case "change5d": case "change30d":
            case "delQtyChange": case "close": case "change":
                this.stockReport = stockReport.sort((left, right): number => (left[sortKey] - right[sortKey]) * this.sortReverse);
                break;
            case "symbol":
            case "upDown":
                this.stockReport = stockReport.sort((left, right): number => (left[sortKey].localeCompare(right[sortKey])) * this.sortReverse);
                break;
        }
        this.startIndex = 0;
    }
}
