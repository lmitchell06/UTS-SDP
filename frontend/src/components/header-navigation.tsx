/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Top navigation
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'

import { State, LoginState } from '../state'
import { LoginBox } from './login'

@Component
class NavigationItem extends Vue {
    @Prop()
    to: string;

    @Prop()
    text: string;

    render(h) {
        return <router-link class="nav-item" active-class="active" tag="li" to={this.to}>
            <a class="nav-link">{this.text}</a>
        </router-link>;
    }
}

@Component
export class HeaderNavigation extends Vue {
    
    state = State.getState();

    render(h) {
        let logInOutNodes;
        if(this.state.loginState == LoginState.In) {
            // what to display if user logged in - name and logout btn
            logInOutNodes = [
                <span class="navbar-text">
                    Welcome, {this.state.currentUser.name}
                </span>,
                <router-link class="btn btn-outline-secondary my-2 my-sm-0" tag="button" to="/logout">Logout</router-link>
            ];
        } else {
            // otherwise display the login box
            logInOutNodes = <LoginBox />;
        }
        return <nav class="header-nav navbar navbar-expand-lg navbar-dark bg-dark">
            <a class="navbar-brand" href="#">Professional Journal</a>
        
            <div class="navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <NavigationItem to="/" text="Home"/>
                </ul>
                {logInOutNodes}
            </div>
        </nav>
    }
}
