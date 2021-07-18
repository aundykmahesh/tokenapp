import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/servererror";


export default class CommonStore{

    error: ServerError | null = null;
    token : string | null = window.localStorage.getItem('jwt');
    apploaded=false;

    constructor(){
        makeAutoObservable(this);

        //mobx reaction. This reacts when token value changes
        reaction(
            () => this.token,
            token => {
                if(token){
                    window.localStorage.setItem('jwt', token);
                }
                else {
                    window.localStorage.removeItem('jwt');
                }
            }
        )
    }

    setServerError = (err: ServerError) => {
        this.error = err;
    }


    setToken = (token: string|null) => {
        this.token = token;
    }

    setAppLoaded = () => {
        this.apploaded = true;
    }

    //https://stackoverflow.com/questions/23593052/format-javascript-date-as-yyyy-mm-dd
    formatDate(date:string) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
    
        if (month.length < 2) 
            month = '0' + month;
        if (day.length < 2) 
            day = '0' + day;
    
        return [day, month, year].join('-');
    }

    AddDays(date:Date, numofdays: number){
        var newdate = new Date(date);
        newdate.setDate(newdate.getDate()+numofdays)
        return newdate;
    }

}