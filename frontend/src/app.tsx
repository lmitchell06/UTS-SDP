import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'

import './assets/style'

import 'moment/locale/en-au'
import moment from 'moment'
moment.locale('en-au');

import {
    HeaderNavigation,
    Home,
    Login,
    Register,
    FAQ,
    Logout,
    CreateJournal,
    JournalEntries,
    CreateEditEntry,
    JournalEntry
} from './components'

Component.registerHooks([
    'beforeRouteEnter',
    'beforeRouteLeave',
    'beforeRouteUpdate'
])
Vue.use(VueRouter);


@Component
class Bar extends Vue {
    text = "bar";

    @Prop()
    name: String;

    mounted() {
    }

    render(h) {
        return <div>A: {this.text} B: {this.name}</div>
    }

   /* beforeRouteEnter(to: Route, from: Route, next: (to?: string) => void) {
        console.log(arguments);
        if(to.params.id.endsWith('!'))
            next();
        else next(to.path + '!');
    }*/
}

var router = new VueRouter({
    mode: 'history',
    routes: [
        { path: '/', component: Home, children: [
            { path: 'journal/new', name: 'new-journal', component: CreateJournal },
            { path: 'journal/:journalId?', name: 'view-journal', component: JournalEntries, props: true },
            { path: 'journal/:journalId/edit/:entryId/:revisionId?', name: 'edit-entry', component: CreateEditEntry, props: true },
            { path: 'journal/:journalId/new', name: 'new-entry', component: CreateEditEntry, props: true },
            { path: 'journal/:journalId/:entryId', name: 'view-entry', component: JournalEntry, props: true },
            { path: '', name: 'home', component: JournalEntries }
        ] },
        { path: '/login', name: 'login', component: Login },
        { path: '/register', name: 'register', component: Register },
        { path: '/faq', name: 'faq', component: FAQ },
        { path: '/logout', name: 'logout', component: Logout },
    ]
});

var app = new Vue({
    el: '#root',
    router: router,
    render: h => {
        return <div id="root">
            <HeaderNavigation />
            <router-view class="view"></router-view>
        </div>
    }
});