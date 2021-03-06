import React, { useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';

import { ToastContainer } from 'react-toastify';
import { Route, Switch } from 'react-router-dom';
import { useStore } from '../stores/stores';
import homepage from './homepage';
import LoginForm from './LoginForm';
import TokensDashboard from './TokensDashboard';
import NavigationBar from './NavigationBar';


function App() {
  const {userstore, commonstore} = useStore();

  useEffect(() => {
   if(commonstore.token){
     userstore.getUser().finally(() => commonstore.setAppLoaded());
   }
   else{
     commonstore.setAppLoaded();
   }
  }, [commonstore,userstore])

  // if(commonstore.apploaded) <LoadingComponenet content='loading app....' />

  return (
    <>
      <ToastContainer position="bottom-right" hideProgressBar />
      <Route exact path="/" component={homepage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <>
            <NavigationBar />
            <Container style={{ marginTop: '7em' }}>
              <Switch>
              <Route exact path="/tokens" component={TokensDashboard} />
              <Route path="/login" component={LoginForm} />

              </Switch>
              
            </Container>
          </>
        )}
      />

    </>
  );
}
//add mobx observer to make it an observer, else cant write back to state variables
export default observer(App);

