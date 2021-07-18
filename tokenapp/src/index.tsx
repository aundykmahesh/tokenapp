import React from 'react';
import ReactDOM from 'react-dom';
import 'semantic-ui-css/semantic.min.css';
import 'react-toastify/dist/ReactToastify.min.css';
import './layout/styles.css';
import reportWebVitals from './reportWebVitals';
import { createBrowserHistory } from 'history';
import { Router } from 'react-router-dom';
import { store, storeContext } from './stores/stores';
import App from './layout/App';

export const history = createBrowserHistory();

ReactDOM.render(
  <storeContext.Provider value={store}>
    <Router history={history}>
      <App />
    </Router>
  </storeContext.Provider>
  ,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
