/*
    Date: 08/10/2017
    By: Adam Burgess / Josh Li (UI)
    Purpose: Frequently Asked Questions
*/

import Vue, { Component as VueComponent } from 'vue'
import { Component, Prop, Watch } from 'vue-property-decorator'
import VueRouter, { Route } from 'vue-router'

@Component
export class FAQ extends Vue {
    render(h) {
        return <div>hello this is home</div>;
    }
}