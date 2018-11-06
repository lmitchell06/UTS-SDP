/*
    Date: 10/10/2017
    By: Adam Burgess
    Purpose: Frequently Asked Questions
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'
import { State } from '../state'

@Component
export class Logout extends Vue {

    state = State.getState();

    async mounted() {
        console.log('i should log out now');

        // log out and go home
        await this.state.logout();
        this.$router.replace('/');
    }

    render(h) {
        return <div>Logging you out...</div>;
    }

    beforeRouteEnter(to: Route, from: Route, next: (to?: string) => void) {
        if(State.getState().currentUser.id == -1) {
            console.log('you have to log in to log out! taking you back to the homepage.');
            return next('/');
        }
        console.log('ok! loading myself');
        next();
    }
}