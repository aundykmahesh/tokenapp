import React, { useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';

import { ToastContainer } from 'react-toastify';
import { Route, Switch, useLocation } from 'react-router-dom';
import { useStore } from '../stores/stores';
import homepage from './homepage';
import LoginForm from './LoginForm';
import TokensDashboard from './TokensDashboard';
import NavigationBar from './NavigationBar';


function App() {
  const location = useLocation();
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
              {/* <Route exact path="/activities" component={Token} />
              <Route key={location.key} path={["/createactivity", "/manage/:id"]} component={ActivityFom} />
              <Route path="/Errors" component={TestErrors} />
              <Route path="/server-error" component={ServerError} /> */}
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

