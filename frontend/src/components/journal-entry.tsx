/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Frequently Asked Questions
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State, Entry, Revision, Journal } from '../state'

import { fuzzyDate, marked, formatDate, readingTime } from '../util'

@Component
export class JournalEntry extends Vue {

    @Prop()
    journalId: string;
    @Prop()
    entryId: string;

    state = State.getState();

    openedRevisions = [];

    edit(revision: Revision) {
        this.$router.push({
            name: 'edit-entry',
            params: {
                journalId: revision.parentEntry.parentJournal.id.toString(),
                entryId: revision.parentEntry.id.toString(),
                revisionId: revision.id.toString()
            }
        });
    }

    journal: Journal = null;
    entry: Entry = null;

    mounted() {
        this.updateData();
    }
    beforeRouteUpdate(to, from, next) {
        this.updateData();
        next();
    }

    updateData() {
        this.journal = this.state.currentUser.journals.find(j => j.id == parseInt(this.journalId));
        if(this.journal)
            this.entry = this.journal.entries.find(e => e.id == parseInt(this.entryId));
    }

    render(h) {
        this.updateData();
        let journal = this.journal;
        let entry = this.entry;

        if(!(journal || entry)) return <div>Loading...</div>;

        let latest = entry.latestRevision;

        let getRevisionStats = (r: Revision) => {
            let stats = readingTime(r.body);
            return `${formatDate(r.edited)} (${stats.text}, ${stats.words} words)`;
        }

        let latestText = marked(latest.body);

        let older = entry.revisions
            .filter(e => e !== latest)
            .reverse()
            .map(r => {
                let stats = readingTime(r.body);
                let open = () => {
                    let index = this.openedRevisions.indexOf(r.id);
                    if(index == -1) this.openedRevisions.push(r.id);
                    else this.openedRevisions.splice(index, 1);
                }
                let opened = this.openedRevisions.indexOf(r.id) > -1;
                let char = '⮞';
                if(opened) char = '⮟';
                return <div>
                        <div onClick={open} class="rev-header">{char} Revision at {formatDate(r.edited)} ({stats.text}, {stats.words} words)</div>
                        <div class={{
                            "rev-body": true,
                            opened
                         }}>
                            <div domPropsInnerHTML={opened ? marked(r.body) : ''}/>
                            <button onClick={() => this.edit(r)} class="btn btn-sm btn-outline-secondary mb-2">Edit from this revision</button>
                        </div>
                    </div>

            });

        let date = fuzzyDate(latest.edited);

        let olderSection;

        if(older.length) {
            olderSection = <div>
                <h5>Older revisions</h5>
                {older}
            </div>;
        }

        let hideText = entry.hidden ? 'Un-hide' : 'Hide';

        return <div class="col-sm-9 text-left">
            <h1><span onClick={this.goback}>⬅</span> {entry.title}- {journal.projectName}</h1>
            <h5>{getRevisionStats(latest)} by {journal.parentUser.name} <div class="btn-group">
                        <button disabled={this.hiddenLoading} onClick={() => this.edit(latest)} class="btn btn-sm btn-outline-primary">Edit</button>
                        <button disabled={this.hiddenLoading} onClick={() => this.toggleHidden()} class="btn btn-sm btn-outline-secondary">{hideText}</button>
                        <button disabled={this.hiddenLoading} onClick={() => this.delete()} class="btn btn-sm btn-outline-danger">Delete</button>
                    </div></h5>
            <div class="ml-3" domPropsInnerHTML={latestText}/>
            {olderSection}
        </div>
    }

    hiddenLoading = false;

    async toggleHidden() {
        this.hiddenLoading = true;
        await this.state.hideEntry(this.entry);
        this.hiddenLoading = false;
    }

    async delete() {
        this.hiddenLoading = true;
        await this.state.deleteEntry(this.entry);
        this.hiddenLoading = false;

        this.goback();
    }

    goback() {
        this.$router.push({
            name: 'view-journal',
            params: {
                journalId: this.journalId.toString()
            }
        });
    }
}