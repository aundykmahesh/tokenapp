import { createContext, useContext } from "react";
import CommonStore from "./commonstore";
import TokenStore from "./tokenstore";
import userStore from "./userstore";

interface Store{
    tokenstore : TokenStore;
    commonstore : CommonStore;
    userstore: userStore;
}

export const store : Store = {
    tokenstore : new TokenStore(),
    userstore: new userStore(),
    commonstore: new CommonStore()
}

export const storeContext = createContext(store);

export function useStore(){
    return useContext(storeContext)
}