import { observer } from "mobx-react-lite";
import React from "react";
import { NavLink } from "react-router-dom";
import { Menu, Button, Container} from "semantic-ui-react";
import { useStore } from "../stores/stores";


export default observer(function NavigationBar() {
    const { userstore } = useStore();

    return (
        <Menu inverted fixed="top">
            <Container>
                <Menu.Item as={NavLink} to='/' exact >
                    <img src="/assets/logo.png" alt="logo" style={{ marginRight: '10px' }} />
                </Menu.Item>
                <Menu.Item as={NavLink} to='/tokens' exact name="Tokens" />
                {userstore.user &&
                    <Menu.Item position='right'>
                        <Button positive onClick={userstore.logout} content="Logout" />
                    </Menu.Item>
                }

            </Container>

        </Menu>
    )
})