import React, { useReducer, useEffect, useMemo } from "react";
import axios from "axios";
import AuthContext from "./authContext";
import authReducer from "./authReducer";
import AuthenticationService from "../../services/authenticationService";

const DefaultAuthorizationHeader = "Authorization";

function setAuthenticationHeader(token) {
  if (token) {
    axios.defaults.headers.common[
      DefaultAuthorizationHeader
    ] = `Bearer ${token}`;
  } else {
    delete axios.defaults.headers.common[DefaultAuthorizationHeader];
  }
}

const AuthState = (props) => {
  const token = localStorage.getItem("token");
  const initialState = {
    token,
    isAuthenticated: Boolean(token) || null,
    loading: false,
    user: null,
    error: null,
  };

  useEffect(() => setAuthenticationHeader(token), []);

  const [state, dispatch] = useReducer(authReducer, initialState);

  const loadUser = async () => {
    setAuthenticationHeader(localStorage.token);
    try {
      //const res = await axios.post("/api/auth/");
      dispatch({
        type: "USER_LOADED",
        // payload: res.data,
      });
    } catch (err) {
      dispatch({ type: "AUTH_ERROR" });
    }
  };

  const login = async (data) => {
    try {
      const res = await AuthenticationService.login(data);

      dispatch({
        type: "LOGIN_SUCCESS",
        payload: res,
      });

      localStorage.setItem("token", res.jwt);

      console.log(`Token is ${localStorage.getItem("token")}`);
      loadUser(); //TODO: Antonio - think about this
    } catch (err) {
      dispatch({
        type: "LOGIN_FAIL",
        payload: err.response,
      });

      localStorage.removeItem("token");
    }
  };

  const logout = () => {
    AuthenticationService.logout();

    dispatch({
      type: "LOGOUT",
    });
  };

  const { children } = props;

  const { token: loginToken, isAuthenticated, loading, user, error } = state;
  const value = useMemo(
    () => ({
      token: loginToken,
      isAuthenticated,
      loading,
      user,
      error,
      loadUser,
      login,
      logout,
    }),
    [loginToken, isAuthenticated, loading, user, error, loadUser, login, logout]
  );
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthState;
