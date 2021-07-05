import React from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import CssBaseline from "@material-ui/core/CssBaseline";
import AuthState from "./context/auth/authState";
import Login from "./components/Login";
import PrivateRoute from "./components/PrivateRoute";
import Home from "./components/Home";
import Registration from "./components/Registration";
import Pdf from "./components/Pdf";
import History from "./components/History";
import Binary from "./components/Binary"

function App() {
  return (
    <AuthState>
      <Router>
        <CssBaseline />
          <Switch>
            <Route exact path="/login" component={Login} />
            <Route exact path="/registration" component={Registration} />
            <PrivateRoute exact path="/" component={Home} />
            <PrivateRoute exact path="/pdf" component={Pdf} />
            <PrivateRoute exact path="/history" component={History} />
            <PrivateRoute exact path="/other-files" component={Binary} />
          </Switch>
      </Router>
    </AuthState>
  );
}

export default App;
