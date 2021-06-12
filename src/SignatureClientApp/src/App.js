import React from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

import AuthState from './context/auth/authState';
import Login from './components/Login';
import PrivateRoute from './components/PrivateRoute';
import Home from "./components/Home";
import Pdf from "./Pdf/Pdf";
import Zip from "./Zip/Zip";

import "./styles.css";

function App() {
  return (
    <AuthState>
      <Router>
        {/* <div>
          <h1 className="info-menu">Electronic signature example</h1>
          <p className="info-menu">Choose the Filetype</p>
          <nav className="menu">
            <Link className="links" to="/pdf">
              PDF
            </Link>
            <Link className="links" to="/zip">
              ZIP
            </Link>
          </nav> */}

        {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
        <Switch>
          <Route exact path="/login" component={Login} />
          <PrivateRoute exact path="/" component={Home} />
          
          {/* <Route path="/pdf">
              <Pdf />
            </Route>
            <Route path="/zip">
              <Zip />
            </Route> */}
        </Switch>
        {/* </div> */}
      </Router>
    </AuthState>
  );
}

export default App;
