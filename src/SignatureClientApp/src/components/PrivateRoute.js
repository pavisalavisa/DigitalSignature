/* eslint-disable react/jsx-props-no-spreading */
import React, { useContext } from "react";
import { Route, Redirect } from "react-router-dom";
import AuthContext from "../context/auth/authContext";
import AppNavigation from "./AppNavigation";
import axios from "axios";

const axiosContext = {};

axios.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (!!error.response && error.response.status === 401) {
      axiosContext.logout("Authentication failed");
    }

    return Promise.reject(error);
  }
);

const PrivateRoute = ({ component: Component, ...rest }) => {
  const authContext = useContext(AuthContext);
  const { isAuthenticated, logout } = authContext;

  axiosContext.logout = logout;

  return (
    <AppNavigation>
      <Route
        {...rest}
        render={(props) =>
          isAuthenticated ? <Component {...props} /> : <Redirect to="/login" />
        }
      />
    </AppNavigation>
  );
};

export default PrivateRoute;
