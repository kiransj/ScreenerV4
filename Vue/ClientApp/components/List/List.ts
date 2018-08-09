import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';

interface CompanyInformation
{
    companyName: string;
    symbol: string;
    IsinNumber: string;
    DateOflisting: string;
}

interface EtfInformation
{
    etfName : string;
    symbol: string;
    underlying: string;
    DateOflisting: string;
}

class ListOfCompanyIndex
{
    companyList: CompanyInformation[];
    etfList: EtfInformation[];
    indexList: string[];
    constructor()
    {
        this.companyList = [];
        this.etfList = [];
        this.indexList = [];
    }
}

let list:ListOfCompanyIndex = new ListOfCompanyIndex();
// Lets save the UI state statically
@Component
export default class ListComponent extends Vue {
    display_flag: boolean = false;
    list:ListOfCompanyIndex = <ListOfCompanyIndex>{};
    display:string = "";
    constructor() {
        super();
        Vue.filter('DateToString', function(date:string)  {
            if (date) { return Moment(date).format('DD-MM-YYYY'); }
        });
    }

    created(): void {
        if(list.companyList.length == 0)
        {
            //this.symbol = this.$route.params.symbol;
            fetch('/api/StockData/GetCompanyList')
            .then(response => response.json() as Promise<ListOfCompanyIndex>)
            .then(data => {
                list = this.list = data;
                this.display_flag = true;
            });
        }
        else
        {
            this.list = list;
            this.display_flag = true;
        }
        this.display = "etf";
    }

    mounted() {

    }

    showInformation(what: string) : void {
        switch(what)
        {
            case "company":
            case "etf":
            case "index":
            this.display  = what;
            break;
            default:
            this.display = "company";
            break;
        }
    }
}
