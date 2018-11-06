/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: The main view of journals - sidebar with the list of journals, and a main view.
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State, LoginState } from '../state'

import linq from 'linq'

import { fuzzyDate, marked, htmlEllipsis } from '../util'

@Component
class HomeNotLoggedIn extends Vue {
    render(h) {
        return <div class="bg-holder">
             <div class="header">
                    SDP Group <span class="orange">3</span>
                    <br/>
                    
                    <router-link class="btn btn-lg btn-primary" tag="button" to="/register">Register</router-link>
                </div>
             </div>;

    }
}

@Component
class JournalSidebar extends Vue {

    state = State.getState();

    render(h) {
        let journals = linq.from(this.state.currentUser.journals).orderByDescending(j => j.lastModified).toArray();

        let journalLinks = journals.map(j => {
            let name = j.projectName;
            let date = fuzzyDate(j.lastModified);
            let totalEntries = j.entries.length == 1 ? '1 entry' : j.entries.length + ' entries';
            let hasEntry = j.entries.length;
            let lastEntryName;
            let lastEntryQuick;
            if(hasEntry) {
                lastEntryName = j.latestEntry.title;
                lastEntryQuick = htmlEllipsis(marked(j.latestEntry.latestRevision.body), 50);
                console.log(lastEntryQuick);
            }
            return <router-link active-class="active" class="list-group-item list-group-item-action flex-column align-items-start" tag="a" to={"/journal/" + j.id}>
                <div class="d-flex w-100 justify-content-between">
                    <span class="sidenav-header">{name}</span>
                    <small class="sidenav-date">{date}, {totalEntries}</small>
                </div>
                {!hasEntry ? undefined : [
                    <p class="mb-0">Latest entry: {lastEntryName}</p>,
                    <small domPropsInnerHTML={lastEntryQuick} />
                ]}
            </router-link>;
        });

        if(journalLinks.length == 0) {
            journalLinks.push(<div style="color: gray">no journals yet</div>);
        }

        return <div class="col-sm-3 sidenav align-items-center">
            <button onClick={this.createJournal} class="btn btn-outline-primary mb-3">Create Journal</button>
            <div class="list-group">
                {journalLinks}
            </div>
        </div>;
    }

    createJournal() {
        this.$router.push({
            name: 'new-journal'
        });
    }
}

@Component
export class Home extends Vue {

    state = State.getState();

    render(h) {
        if(!(this.state.loginState == LoginState.In)) {
            return <HomeNotLoggedIn />;
        }
        return  <div class="container-fluid">    
                    <div class="row content">
                        <JournalSidebar />
                        <router-view />
                    </div>
                </div>

    }
}