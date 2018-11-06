/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Displays the login page, with the full-screen background.
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State } from '../state'

@Component
export class Login extends Vue {
    render(h) {
        return <div>Login</div>;
    }
}

@Component
export class LoginBox extends Vue {

    email = "";
    password = "";

    state = State.getState();

    loading = false;
    error = false;
    errors: string[] = [];

    render(h) {
        let errorNode;
        let overlayClass = "login-overlay";
        if(this.error) errorNode =  <div class="alert alert-warning">
                                        {this.errors.map(e => <div>{e}</div>)}
                                    </div>;
        let spinner;
        if(this.loading) spinner = <span class="spinner"></span>;
        return <div><div class="navbar-form navbar-right form-inline">
                
                <input v-model={this.email} type="text" class="from-control" id="email" placeholder="username" style="margin-right:5px;height: 30px"/>
                <input v-model={this.password} type="password" class="from-control" id="password" placeholder="password" style="margin-right:5px;height: 30px"/>
                <button disable={!this.loading} onClick={this.login} class="btn btn-primary btn-sm" >{spinner} Login</button>
                
            </div>{errorNode}</div>
    }

    async login(event) {
        event.preventDefault();

        this.loading = true;

        try {
            await this.state.login(this.email, this.password);
            this.$router.push({ name: 'home' });
            this.error = false;
        } catch(e) {
            this.error = true;
            this.errors = e;
        }

        this.loading = false;
    }
}