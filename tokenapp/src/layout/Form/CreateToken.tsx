import { ErrorMessage, Formik } from "formik";
import React from "react";
import { Button, Form, Label } from "semantic-ui-react";
import { useStore } from "../../stores/stores";
import CommonTextInput from "./CommonTextInput";
import * as Yup from "yup";


export default function CreateToken(){
    const{tokenstore} = useStore();
    const { generateToken} = tokenstore;

    const ValidationSchema = Yup.object({
        url: Yup.string().required("The URL is required").url("Should be of proper URL format")
    });

    return(
        <Formik initialValues={{url: '', error:null}} validationSchema={ValidationSchema}
            onSubmit={(values,{setErrors}) => generateToken(values.url).catch(error => 
                setErrors({error :'Invalid URL'}))}>
                {
                    //this is formik issubmitting
                    ({handleSubmit, isSubmitting, errors}) => (
                        <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                            <CommonTextInput name='url' placeholder='URL' 
                               color={errors.error && 'red'}
                            />
                            <ErrorMessage 
                                name='error'
                                render = {() => 
                                    <Label style={{marginBottom: 10}} basic
                                    color='red' content={errors.error} />
                                }
                            />
                            <Button loading={isSubmitting} positive content='Create Token' type='submit' fluid />
                        </Form>
                    )
                }
        </Formik>        
    )
}