import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/Home/Home.vue.html') },
    { path: '/Equity/Report', component: require('./components/EquityReport/EquityReport.vue.html') },
    { path: '/Options/Report', component: require('./components/OptionsReport/OptionsReport.vue.html') },
    { path: '/List', component: require('./components/List/List.vue.html') },
    { path: '/History/:symbol', component: require('./components/History/History.vue.html') },

];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
