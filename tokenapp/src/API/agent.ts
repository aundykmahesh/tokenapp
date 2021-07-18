import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "..";
import { Token, TokenFormValue } from "../models/tokenmodel";
import { User, UserFormValues } from "../models/user";
import { store } from "../stores/stores";

axios.defaults.baseURL = "http://localhost:5001/api";

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    })
}

//this adds token to all header if logged in
axios.interceptors.request.use(config => {
    if(store.commonstore.token) {
        config.headers.Authorization = `Bearer ${store.commonstore.token}`
    }
    return config;
})

axios.interceptors.response.use(async response => {

    await sleep(1000);
    return response;
}, (error: AxiosError) => {
    const { data, status, config } = error.response!;
    switch (status) {
        case 400:
            if (typeof data === 'string') {
                toast.error(data);
            }
            if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                history.push('/not-found');
            }

            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key])
                    }
                }
                throw modalStateErrors.flat();
            }
            break;
        case 401:
            toast.error("Unauthorized request");
            break;
        case 404:
           history.push('/not-found');
            break;
        case 500:
            toast.error("server not found");
            store.commonstore.setServerError(data);
            history.push("/server-error");
            break;
        default:
            break;
    }
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}
//use back tick to use string literal
const Tokens = {
    list: () => requests.get<Token[]>('/token/tokens'),
    generatetoken: (urlval: TokenFormValue) => requests.post<Token>('/token/generate', urlval),
    disabletoken: (tokenid: string) => requests.post(`/token/disabletoken?tokenid=${tokenid}`, tokenid),
    enabletoken: (tokenid: string) => requests.post(`/token/enabletoken?tokenid=${tokenid}`, tokenid),
    validatetoken: (token: TokenFormValue) => requests.post('/token/validate', token)
}

const Account = {
    current: () => requests.get<User>('/accounts'),
    login: (user: UserFormValues) => requests.post<User>('/accounts/login', user),
}

const agent = {
    Tokens,
    Account
}

export default agent;