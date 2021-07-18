import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Container, Header, Segment, Button } from "semantic-ui-react";
import { useStore } from "../stores/stores";

export default observer(function HomePage() {

    const { userstore } = useStore();

    return (
        <Segment inverted textAlign='center' vertical className='masthead'>
            <Container text>
                <Header as='h1' inverted>
                    Token App
                </Header>
                {userstore.isLoggedIn ? (
                    <>
                        <Header as='h2' inverted content='Welcome to Token app' />
                        <Button as={Link} to='/tokens' size='huge' inverted>
                            Go to Tokens Dashboard!
                        </Button>
                    </>
                ) : (
                    <Button as={Link} to='/login' size='huge' inverted>
                        Login!
                    </Button>
                )}

            </Container>

        </Segment>
    )
})