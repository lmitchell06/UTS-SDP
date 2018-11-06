/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Register window
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State, LoginState } from '../state'

@Component
export class Register extends Vue {

    state = State.getState();

    firstName = "";
    lastName = "";
    username = "";
    password = "";
    company = false;
    companyName = "";

    errors = [];

    loading = false;
    success = false;

    mounted() {
        if(this.state.loginState == LoginState.In)
            this.$router.replace('/');
    }

    async submit(e) {
        if(this.loading) return;
        this.loading = true;

        e.preventDefault();

        // rudimentary validation.
        let errors: string[] = [];
        if(!this.firstName.trim().length)
            errors.push("No first name");
        if(!this.lastName.trim().length)
            errors.push("No last name");
        if(!this.username.trim().length)
            errors.push("No username");
        if(!this.password.trim().length)
            errors.push("No password");
        if(this.company && !this.companyName.trim().length)
            errors.push("No company name");

        if(errors.length != 0) {
            this.errors = errors;
        } else {
            this.errors = [];
            let result = await this.state.register(this.firstName, this.lastName, this.username, this.password, this.company ? this.companyName : "");
            if(result === true) {
                // all g! let's redirect to the main page.
                this.success = true;
                setTimeout(() => this.$router.push({ name: 'home' }), 2000);
            } else {
                // oh no! we have some errors
                this.errors = result.filter(e => !e.IsValid).map(e => `${e.Message}`);
            }
        }

        this.loading = false;
    }

    render(h) {
        let companyInput;
        if(this.company) {
            companyInput = <div class="form-group row">
                <label for="companyName" class="col-sm-3 col-form-label">Company name</label>
                <div class="col-sm-9">
                    <input v-model={this.companyName} type="text" class="form-control" id="companyName" placeholder="ACME Inc" />
                </div>
            </div>;
        }
        let spinner;
        if(this.loading) {
            spinner = <span class="spinner"/>
        }
        let buttonNode =    <div class="form-group row">
                                <div class="col-sm-9">
                                    <button disabled={this.loading} onClick={this.submit} type="submit" class="btn btn-primary">{spinner} Register</button>
                                </div>
                            </div>;
        if(this.success) {
            buttonNode =    <div class="form-group row">
                                <div class="col-sm-12">
                                    <div class="alert alert-success" role="alert">
                                        Welcome, {this.firstName} {this.lastName}!<br/>
                                        Redirecting you back to the main page.
                                    </div>
                                </div>
                            </div>
        }
        let errors;
        if(this.errors.length) {
            errors = this.errors.map(e => <div class="alert alert-danger" role="alert">{e}</div>);
        }

        return  <div class="container" style="margin-top: 50px">
                    <div class="row">
                        <div class="col-sm-6" >
                                <img src="./assets/402H.jpg" class="img-rounded registerImage" width="450" height="450"/> 
                        </div>
                        <div class="col-sm-6">
                            <div class="row">
                                <div class="col-12">
                                    <h1>Register</h1>
                                </div>
                            </div>
                           
                            <div class="row">
                                <form class="col-12 no-gutters">
                                    <div class="form-group row">
                                        <label for="firstname" class="col-sm-3 col-form-label">First name</label>
                                        <div class="col-sm-9">
                                            <input v-model={this.firstName} type="text" class="form-control" id="firstname" placeholder="First name" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="lastname" class="col-sm-3 col-form-label">Last name</label>
                                        <div class="col-sm-9">
                                            <input v-model={this.lastName} type="text" class="form-control" id="lastname" placeholder="Last name" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="regUsername" class="col-sm-3 col-form-label">Username</label>
                                        <div class="col-sm-9">
                                            <input v-model={this.username} type="text" class="form-control" id="regUsername" placeholder="Username" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="regPassword" class="col-sm-3 col-form-label">Password (min 8 characters)</label>
                                        <div class="col-sm-9">
                                            <input v-model={this.password} type="password" class="form-control" id="regPassword" placeholder="Password" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3">Part of a company?</div>
                                        <div class="col-sm-9">
                                            <div class="form-check">
                                                <label class="form-check-label">
                                                    <input v-model={this.company} class="form-check-input" type="checkbox" /> Yes
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    {companyInput}
                                    {errors}
                                    {buttonNode}
                                </form>
                            </div>
                        </div>
                    </div>
                </div>;     
    }
}