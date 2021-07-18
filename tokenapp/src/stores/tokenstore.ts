import { url } from "inspector";
import { makeAutoObservable, makeObservable, runInAction } from "mobx";
import React from "react";
import agent from "../API/agent";
import { Token, TokenFormValue } from "../models/tokenmodel";


export default class TokenStore {
    loading = false;
    submitting = false;
    tokens: Token[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    setLoadingStatus = (status: boolean) => {
        this.loading = status;
    }

    setSubmittingStatus = (status: boolean) => {
        this.submitting = status;
    }

    // get getAllTokens(){
    //     return this.tokens;
    // }

    loadAllTokens = async () => {
        this.setLoadingStatus(true);
        try {
            await agent.Tokens.list().then(response => {
                //let activty: Activity[] = [];

                response.forEach(element => {
                    this.setToken(element);
                });
                //this.setactivities(activty);
                this.setLoadingStatus(false);

            });
        } catch (error) {
            console.log(error);
            this.setLoadingStatus(false);
        }
    }

    generateToken = async (urlval: string) => {
        this.setLoadingStatus(true);
        try {
            var input:TokenFormValue={appurl:urlval};
            await agent.Tokens.generatetoken(input).then((response) => {
                this.setToken(response);
            });
            this.setLoadingStatus(false);
        } catch (error) {
            console.log(error);
            this.setLoadingStatus(false);
        }
    }

    setToken = (element: Token) => {
        //activty.push(element);
        //this.tokenRegistry.set(element.id, element);
        element.status = this.setTokenStatus(element.status);
        this.tokens.push(element);
        // console.log(this.tokens.length);
    }

    disableToken = async (tokenid: string) => {
        this.setSubmittingStatus(true);
        try {
            await agent.Tokens.disabletoken(tokenid);
            this.tokens.find(c => c.guid == tokenid)!.status = this.setTokenStatus("2");
            this.setSubmittingStatus(false);
        }
        catch (error) {
            console.log(error);
            this.setSubmittingStatus(false);
        }
    }

    setTokenStatus = (status: string) => {
        if (status == "0") return "Enabled";
        if (status == "1") return "Expired";
        return "Disabled";
    }

}