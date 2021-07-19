import { observer } from 'mobx-react-lite';
import React, { SyntheticEvent, useEffect, useState } from 'react'
import { Button, Segment, Table } from 'semantic-ui-react'
import { useStore } from '../stores/stores'
import LoadingComponent from './loadcomponent';
import CreateToken from './Form/CreateToken';
import PaginationForToken from './Pagination';
import TokenList from './TokenList';




export default observer(function TokensDashboard() {
    const { tokenstore} = useStore();

    useEffect(() => {
       tokenstore.loadPaginatedTokens(1);
    }, [tokenstore.loadPaginatedTokens,tokenstore])

    //if (user == null) history.push('/');

    if (tokenstore.loading) return <LoadingComponent content='Loading app......' />

    return (

        <Segment.Group>
            <Segment>
                <CreateToken />
                </Segment>
            <Segment>
                <TokenList />
            </Segment>
        </Segment.Group>
    )
}
)