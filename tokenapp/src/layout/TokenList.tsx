import React, { SyntheticEvent, useState } from "react";
import { Button, Table } from "semantic-ui-react";
import { useStore } from "../stores/stores";
import PaginationForToken from './Pagination';


export default function TokenList(){
    const { tokenstore, userstore,commonstore } = useStore();
    const { paginatedTokens: tokens, disableToken, submitting} = tokenstore;
    const [target, setTarget] = useState('');

    function handleActivityChangeStatus(e: SyntheticEvent<HTMLButtonElement>, id: string) {
        setTarget(e.currentTarget.name);
        if(e.currentTarget.innerText =="Enable Token"){
            var removelength = 'btnenable_'.length;
            tokenstore.enableToken(id.substring(removelength));
        }
        else{
            disableToken(id);
        }
    }

    return(
        <Table celled>
        <Table.Header>
            <Table.Row>
                <Table.HeaderCell>URL</Table.HeaderCell>
                <Table.HeaderCell>Token</Table.HeaderCell>
                <Table.HeaderCell>Created Date</Table.HeaderCell>
                <Table.HeaderCell>Expiry Date</Table.HeaderCell>
                <Table.HeaderCell>Status</Table.HeaderCell>
                <Table.HeaderCell>Action</Table.HeaderCell>

            </Table.Row>
        </Table.Header>

        <Table.Body>
            <>
                {
                    tokens.map(tokenitem => (
                        <Table.Row key={tokenitem.guid}>
                            <Table.Cell>{tokenitem.appUrl}</Table.Cell>
                            <Table.Cell>{tokenitem.tokenString}</Table.Cell>
                            <Table.Cell>{commonstore.formatDate(tokenitem.createdDate.toString())}</Table.Cell>
                            <Table.Cell>{
                                commonstore.formatDate(
                                    commonstore.AddDays(tokenitem.createdDate,7).toString()
                                    )
                            }</Table.Cell>
                            <Table.Cell>{tokenitem.status}</Table.Cell>
                            <Table.Cell>
                            <Button onClick={(e) => { handleActivityChangeStatus(e, `btnenable_${tokenitem.guid}`) }}
                                    name= {`btnenable_${tokenitem.guid}`}
                                    loading={submitting && target === `btnenable_${tokenitem.guid}`}
                                    disabled={tokenitem.status != "Enabled" ? false : true}
                                    floated='right'
                                    content='Enable Token'
                                    color='red'></Button>
                                <Button onClick={(e) => { handleActivityChangeStatus(e, tokenitem.guid) }}
                                    name={tokenitem.guid}
                                    loading={submitting && target === tokenitem.guid}
                                    disabled={tokenitem.status == "Enabled" ? false : true}
                                    floated='right'
                                    content='Disable Token'
                                    color='red'></Button>
                            </Table.Cell>
                        </Table.Row>
                    ))
                }
            </>
        </Table.Body>
        <Table.Footer>
            <Table.Row>
                <Table.HeaderCell colSpan="6">
                    <PaginationForToken tokenarranylength={tokenstore.tokens.length} />

                </Table.HeaderCell>
            </Table.Row>
        </Table.Footer>
    </Table>
    )
}