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
    change60d: number;
    change120d: number;
    hlp: number;
    delQtyChange: number;
    marketCap: number;
}

interface FavLists
{
    symbol: string;
    listName: string;
}

class UIState {
    stockReport:StockReport[] = [];
    favList:FavLists[] = []; // Fav companies and its list
    favListNames: string[] = []; //List names
    searchQuery:string = "";  // Search Query
    sortKey:string = "";
    listNameShown: string = ""; // List to show
    startIndex: number = 0;
    elementsPerPage: number = 25;
    sortDirection: number = -1;
    marketCapEnd: number = 0;
    marketCapStart: number = 0;
}

//let favLists:FavLists[] = [];
//let stockReport:StockReport[] = [];
//let favListNames: string[] = [];

let uiState:UIState = new UIState();

// Lets save the UI state statically
@Component
export default class ReportComponent extends Vue {
    stockReport:StockReport[] = [];
    favLists:FavLists[] = [];
    favListNames: string[] = [];
    marketCapEnd: number = 0;
    marketCapStart: number = 0;
    searchQuery:string = "";
    startIndex: number = 0;
    elementsPerPage: number = 25;
    sortReverse: number = -1;

    created(): void {
        if(uiState.stockReport.length == 0) {
            //this.symbol = this.$route.params.symbol;
            fetch('/api/StockData/GetLastestStockReport')
            .then(response => response.json() as Promise<StockReport[]>)
            .then(data => {
                uiState.stockReport = data;
                this.stockReport = uiState.stockReport;
            });
        }

        //if(uiState.favList.length == 0)
        {
            // Get list and symbols
            fetch('/api/StockData/GetFavListWithSymbols')
            .then(response => response.json() as Promise<FavLists[]>)
            .then(data => { this.favLists = uiState.favList = data; });
        }

        if(uiState.favListNames.length == 0) {
            // Get all list names
            fetch('/api/StockData/GetFavLists')
            .then(response => response.json() as Promise<string[]>)
            .then(data => { uiState.favListNames = this.favListNames = data; });
        }
        this.RestoreUiState();
    }

    mounted() {

    }

    RestoreUiState(): void {

        // Filter based on listname
        this.favListNames = uiState.favListNames;
        this.favLists = uiState.favList;


        if(uiState.marketCapEnd != 0) {
            this.filterByMarketCap(uiState.marketCapStart, uiState.marketCapEnd);
        } else {
            this.showCompaniesInList(uiState.listNameShown);
        }

        // Restore the searchQuery
        this.searchQuery = uiState.searchQuery;
        this.onSearch();

        // Sort by last chosen column
        this.sortReverse = uiState.sortDirection;
        this.sortBy(uiState.sortKey, false);

        // Restore the display order
        this.startIndex = uiState.startIndex;
        this.elementsPerPage = uiState.elementsPerPage;
    }

    ShowElements(more: boolean): void {
        this.startIndex += more ? (+25) : (-25);

        if(this.startIndex > this.stockReport.length)
             this.startIndex = this.stockReport.length - 25;
        else if(this.startIndex < 0) this.startIndex = 0

        uiState.elementsPerPage = this.elementsPerPage = 25;
        uiState.startIndex = this.startIndex;
    }

    showAll(): void {
        this.elementsPerPage = (this.elementsPerPage == 25) ? 100000 : 25;
        if(this.elementsPerPage == 100000) {
            uiState.startIndex = this.startIndex = 0;
        }
        uiState.elementsPerPage = this.elementsPerPage;
    }

    onSearch(): void {
        uiState.searchQuery = this.searchQuery;
        if(uiState.marketCapEnd != 0) {
            this.filterByMarketCap(uiState.marketCapStart, uiState.marketCapEnd);
        } else {
            this.showCompaniesInList(uiState.listNameShown);
        }

        if(this.searchQuery.length > 0)
            this.stockReport = this.stockReport.filter(x => (x.symbol.toLowerCase().indexOf(this.searchQuery.toLowerCase()) >= 0 || x.upDown.toLowerCase().indexOf(this.searchQuery.toLowerCase()) == 0));

        uiState.startIndex = this.startIndex = 0;
        uiState.elementsPerPage = this.elementsPerPage = 25;
        this.sortBy(uiState.sortKey, false);
    }

    sortBy(sortKey: string, directionChange:Boolean = true): void  {
        uiState.sortKey = sortKey;
        if(directionChange) {
            this.sortReverse *= -1;
            uiState.sortDirection = this.sortReverse;
        }
        switch (sortKey) {
            case "totQty": case "hlp":
            case "change5d": case "change30d": case "change60d": case "change120d":
            case "marketCap": case "delQtyChange": case "close": case "change":
                this.stockReport = this.stockReport.sort((left, right): number => (left[sortKey] - right[sortKey]) * this.sortReverse);
                break;
            case "symbol":
            case "upDown":
                this.stockReport = this.stockReport.sort((left, right): number => (left[sortKey].localeCompare(right[sortKey])) * this.sortReverse);
                break;
        }
        uiState.startIndex = this.startIndex = 0;
    }

    listName: string ="All";
    showCompaniesInList(listName: string): void {
        this.listName = uiState.listNameShown = listName;
        if(listName.length > 0) {
            var symbolList = uiState.favList.filter((value: FavLists, index: number, array: FavLists[]) => { return value.listName === listName}).map(x => x.symbol);
            this.stockReport = uiState.stockReport.filter((value: StockReport, index: number, array: StockReport[]) => { return symbolList.indexOf(value.symbol) >= 0; });
        }
        else {
            this.stockReport = uiState.stockReport;
            this.listName = "All";
        }
        uiState.startIndex = this.startIndex = 0;
    }

    filterByMarketCap(start: number, end: number): void {
        uiState.marketCapEnd = this.marketCapEnd = end;
        uiState.marketCapStart = this.marketCapStart = start;
        this.stockReport = (end == 0) ? uiState.stockReport: uiState.stockReport.filter(x => x.marketCap >= start && x.marketCap <= end);
        uiState.startIndex = this.startIndex = 0;
    }
}
