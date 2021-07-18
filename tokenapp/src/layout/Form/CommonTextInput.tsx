import { useField } from "formik";
import React from "react";
import { Label } from "semantic-ui-react";
import Form from "semantic-ui-react/dist/commonjs/collections/Form";


export interface Props{
    placeholder : string;
    name: string;
    type?: string;
    label?: string;
    color?:string;
}

export default function CommonTextInput(props:Props) {
    const [field,meta] = useField(props.name);

    return(
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <input {...field} {...props}  />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ):null}
        </Form.Field>
    )
}