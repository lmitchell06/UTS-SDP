/*
    Date: 12/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Create Journal
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State } from '../state'

@Component
export class CreateJournal extends Vue {
    state = State.getState();

    journalName: string;

    errors = [];
    loading = false;
    
    async submit() {
        this.loading = true;

        this.errors = [];

        if(this.journalName.trim() == "")
            this.errors.push("You need a project name!");

        if(this.errors.length == 0) {
            // save it!
            let id = await this.state.createJournal(this.journalName);
            this.$router.push({
                name: 'view-journal',
                params: {
                    journalId: id.toString()
                }
            });
        }

        this.loading = false;
    }

    render(h) {
        let spinner;
        if(this.loading) {
            spinner = <span class="spinner"/>
        }
        return  <div class="col-sm-9 text-left">
                    <h2>New Journal</h2>
                    <form>
                        <div class="form-group">
                            <label for="journalName">Project name</label>
                            <input v-model={this.journalName} type="text" class="form-control" id="journalName" placeholder="Project name"/>
                        </div>
                    </form>
                    <button disabled={this.loading} onClick={this.submit} type="submit" class="btn btn-primary">{spinner} Create Journal</button>
            </div>
    }
}