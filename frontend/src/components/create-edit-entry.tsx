/*
    Date: 12/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Create or Edit a Journal Entry
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State, Entry, Journal } from '../state'

@Component
export class CreateEditEntry extends Vue {

    @Prop()
    journalId: string;

    @Prop()
    entryId: string;

    @Prop()
    revisionId: string;

    state = State.getState();

    editing = false;
    entryTitle = "";
    entryBody = "";

    errors = [];
    loading = false;

    journal: Journal = null;
    entry: Entry = null;
    
    async submit() {
        this.loading = true;

        this.errors = [];

        if(this.entryTitle.trim() == "")
            this.errors.push("You need a title!");
        if(this.entryBody.trim() == "")
            this.errors.push("You need a body!");

        if(this.errors.length == 0) {
            // save it!
            let id = this.entryId;
            if(!id) {
                id = '' + await this.state.createEntry(this.journalId, this.entryTitle, this.entryBody);
            } else {
                await this.state.createRevision(this.journalId, this.entryId, this.entryBody);
            }
            this.$router.push({
                name: 'view-journal',
                params: {
                    journalId: this.journalId.toString()
                }
            });
        }

        this.loading = false;
    }

    beforeRouteUpdate(to, from, next) {
        this.updateData();
        next();
    }

    mounted() {
        this.updateData();
    }

    setBody = false;

    updateData() {
        try {
            this.journal = this.state.currentUser.journals.find(k => k.id == parseInt(this.journalId));
            this.entry = this.journal.entries.find(e => e.id == parseInt(this.entryId));
            if(this.entry) {
                this.editing = true;
                this.entryTitle = this.entry.title;

                let revision = this.entry.latestRevision;
                if(this.revisionId)
                    revision = this.entry.revisions.find(e => e.id == parseInt(this.revisionId));
                if(revision && !this.setBody) {
                    this.entryBody = revision.body;
                    this.setBody = true;
                }
            }
        } catch {}
    }

    render(h) {
        this.updateData();

        let journal = this.journal;
        let entry = this.entry;


        let spinner;
        if(this.loading) {
            spinner = <span class="spinner"/>
        }

        let journalName = journal.projectName;
        let type = this.editing ? 'Edit' : 'New';
        let btnText = this.editing ? "Create new revision" : "Create Entry";

        return  <div class="col-sm-9 text-left">
                    <h2>{journalName}: {type} Entry</h2>
                    <form>
                        <div class="form-group">
                            <label for="entryTitle">Title</label>
                            <input disabled={this.editing} v-model={this.entryTitle} type="text" class="form-control" id="entryTitle" placeholder="Title"/>
                        </div>
                        <div class="form-group">
                            <label for="textBody">Text <small>You can use <a href="https://guides.github.com/features/mastering-markdown/">markdown</a> to style your text</small></label>
                            <textarea v-model={this.entryBody} class="form-control" id="textBody" rows="10"></textarea>
                        </div>
                    </form>
                    <button disabled={this.loading} onClick={this.submit} type="submit" class="btn btn-primary">{spinner} {btnText}</button>
            </div>
        return <div>hello we are editing an entry: {this.journalId} {this.entryId}</div>;
    }
}