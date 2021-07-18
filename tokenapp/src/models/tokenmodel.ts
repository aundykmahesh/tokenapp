export interface Token{
    guid:string,
    appUrl:string,
    tokenString:string,
    createdDate:Date,
    status:string
}

export interface TokenFormValue{
    appurl: string
}