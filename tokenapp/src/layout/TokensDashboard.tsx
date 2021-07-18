import { observable } from 'mobx';
import { observer } from 'mobx-react-lite';
import React, { SyntheticEvent, useEffect, useState } from 'react'
import { Button, Form, Icon, Item, Label, Menu, Segment, Table } from 'semantic-ui-react'
import { useStore } from '../stores/stores'
import LoadingComponent from './loadcomponent';
import { history } from "..";
import CreateToken from './Form/CreateToken';




export default observer(function TokensDashboard() {
    const { tokenstore, userstore } = useStore();
    const { loadAllTokens, tokens, disableToken, submitting , generateToken} = tokenstore;
    const { user } = userstore;
    const [target, setTarget] = useState('');

    useEffect(() => {
        if (tokens.length <= 1) loadAllTokens();
    }, [tokens.length, loadAllTokens])

    if (user == null) history.push('/');

    if (tokenstore.loading) return <LoadingComponent content='Loading app......' />

    function handleActivityChangeStatus(e: SyntheticEvent<HTMLButtonElement>, id: string) {
        setTarget(e.currentTarget.name);
        disableToken(id);
    }

    return (

        <Segment.Group>
            <Segment>
                <CreateToken />
                </Segment>

            <Segment>
                <Table celled>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>URL</Table.HeaderCell>
                            <Table.HeaderCell>Token</Table.HeaderCell>
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
                                        <Table.Cell>{tokenitem.createdDate}</Table.Cell>
                                        <Table.Cell>{tokenitem.status}</Table.Cell>
                                        <Table.Cell>
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
                            <Table.HeaderCell colSpan='3'>
                                <Menu floated='right' pagination>
                                    <Menu.Item as='a' icon>
                                        <Icon name='chevron left' />
                                    </Menu.Item>
                                    <Menu.Item as='a'>1</Menu.Item>
                                    <Menu.Item as='a'>2</Menu.Item>
                                    <Menu.Item as='a'>3</Menu.Item>
                                    <Menu.Item as='a'>4</Menu.Item>
                                    <Menu.Item as='a' icon>
                                        <Icon name='chevron right' />
                                    </Menu.Item>
                                </Menu>
                            </Table.HeaderCell>
                        </Table.Row>
                    </Table.Footer>
                </Table>
            </Segment>

        </Segment.Group>


    )
}
)