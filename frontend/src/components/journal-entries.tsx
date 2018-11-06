/*
    Date: 12/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: View entries from a journal
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State, Entry, Journal } from '../state'
import linq from 'linq'

import { fuzzyDate, htmlEllipsis, marked } from '../util'

@Component
export class JournalEntries extends Vue {

    @Prop()
    journalId: string;

    state = State.getState();


    findJournal() {
        if(this.journalId && this.state.currentUser.journals.length) {
            let id = parseInt(this.journalId, 10);
            return this.state.currentUser.journals.find(x => x.id == id);
        }
        return null;
    }

    newEntry() {
        this.$router.push({
            name: 'new-entry',
            params: {
                journalId: this.journalId
            }
        });
    }

    edit(entry: Entry) {
        this.$router.push({
            name: 'edit-entry',
            params: {
                journalId: entry.parentId.toString(),
                entryId: entry.id.toString()
            }
        });
    }

    async delete(entry: Entry) {
        this.hiddenLoading = true;
        await this.state.deleteEntry(entry);
        this.hiddenLoading = false;
    }

    openEntry(entry) {
        console.log('opening entry');
        this.$router.push({
            name: 'view-entry',
            params: {
                journalId: entry.parentId.toString(),
                entryId: entry.id.toString()
            }
        })
    }

    hiddenLoading = false;

    async toggleHidden(entry: Entry) {
        this.hiddenLoading = true;
        await this.state.hideEntry(entry);
        this.hiddenLoading = false;
    }

    render(h) {
        let journal = this.findJournal();

        let showHidden = this.state.journalEntriesShowHidden;

        let header;
        let allEntries: Entry[];
        let entries: Entry[];
        let newEntry;
        if(!journal) {
            header = <h2>Viewing {showHidden ? 'all ': ''} entries from all journals</h2>;
            allEntries = linq.from(this.state.currentUser.journals).selectMany(x => x.entries).toArray();
        } else {
            header = <h2>Viewing {showHidden ? 'all ': ''} entries from {journal.projectName}</h2>;
            allEntries = journal.entries;
            newEntry = <button onClick={this.newEntry} class="btn btn-outline-primary">New Entry</button>;
        }

        entries = linq.from(allEntries)
            // if show hidden, show all, otherwise only show not hidden
            .where(e => showHidden ? true : !e.hidden)
            .where(e => e.latestRevision != null)
            .orderByDescending(e => e.latestRevision.edited)
            .toArray();


        let entryRows = entries.map(e => {
            let rev = e.latestRevision;
            let text = "No revisions have been made";
            let edited;
            if(rev) {
                text = marked(htmlEllipsis(rev.body, 80));
                edited = fuzzyDate(rev.edited);
            }

            let hideText = 'Hide';
            if(e.hidden) hideText = 'Un-hide';

            let nameClass = 'name';
            let title = e.title;
            if(e.hidden) {
                title += ' (hidden)';
                nameClass += ' hidden';
            }
            
            return <tr>
                <td class={nameClass} onClick={() => this.openEntry(e)}>{title}</td>
                <td class="description" onClick={() => this.openEntry(e)} domPropsInnerHTML={text}></td>
                <td class="edited" onClick={() => this.openEntry(e)}>{edited}</td>
                <td class="actions">
                    <div class="btn-group">
                        <button disabled={this.hiddenLoading} onClick={() => this.edit(e)} class="btn btn-sm btn-outline-primary">Edit</button>
                        <button disabled={this.hiddenLoading} onClick={() => this.toggleHidden(e)} class="btn btn-sm btn-outline-secondary">{hideText}</button>
                        <button disabled={this.hiddenLoading} onClick={() => this.delete(e)} class="btn btn-sm btn-outline-danger">Delete</button>
                    </div>
                </td>
            </tr>
        });

        if(entryRows.length == 0) {
            let text = 'No entries yet';
            if(!showHidden && allEntries.length)
                text = 'No visible entries';
            entryRows.push(<tr><td colspan="4"><i>{text}</i></td></tr>);
        } 

        let showVisibleText = !showHidden ? 'Showing visible only' : 'Show visible only';
        let showAllText = showHidden ? 'Showing all' : 'Show all';

        let pills = <ul class="nav nav-pills flex-inline vam">
            <li onClick={() => this.state.journalEntriesShowHidden = false} class="nav-item">
                <a class={"nav-link " + (!showHidden ? 'active': '')} href="#">{showVisibleText}</a>
            </li>
            <li onClick={() => this.state.journalEntriesShowHidden = true} class="nav-item">
                <a class={"nav-link " + (showHidden ? 'active': '')} href="#">{showAllText}</a>
            </li>
        </ul>

        return <div class="row-margin col-sm-9 text-left">
                <div class="row"><div class="col-12">{header}</div></div>
                <div class="row"><div class="col-12">
                    {newEntry} {pills}
                </div></div>
                <div class="row">
                    <table class="table table-striped entry-list">
                        <thead>
                            <tr>
                                <th class="name">Title</th>
                                <th class="description">Quickview</th>
                                <th class="edited">Created/Last Edited</th>
                                <th class="actions">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {entryRows}
                        </tbody>
                    </table> 
                </div>
            </div>;

        // journalId not undefined, find it
        // otherwise, show all entries
        //console.log(this.journalId);
        //return <div>hello this is journal entries and should be here</div>;
    }
}