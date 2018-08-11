import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import Moment from 'moment';

// Lets save the UI state statically
@Component
export default class HomeComponent extends Vue {

    created(): void {

    }

    mounted() {

    }

    UpdateDataToLatest(): void {
        alert("Updating data to latest");
        fetch('/api/StockData/UpdateDataToLatest')
        .then(response => response.json() as Promise<number>)
        .then(data => {
            alert("Updated data to Latest");
        });
    }
}
