import React from "react";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";
import Container from "@material-ui/core/Container";
import CssBaseline from "@material-ui/core/CssBaseline";
import AuthState from "./context/auth/authState";
import Login from "./components/Login";
import PrivateRoute from "./components/PrivateRoute";
import Home from "./components/Home";
import Registration from "./components/Registration";
import Navbar from "./components/Navbar";

import "./styles.css";

function App() {
  return (
    <AuthState>
      <Router>
        <Navbar />
        <Switch>
          <Container component="main" maxWidth="xs">
            <CssBaseline />
            <Route exact path="/login" component={Login} />
            <Route exact path="/registration" component={Registration} />
            <PrivateRoute exact path="/" component={Home} />
          </Container>
        </Switch>
      </Router>
    </AuthState>
  );
}

export default App;
