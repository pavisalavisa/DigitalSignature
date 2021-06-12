import React, { useState, useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import AuthContext from "../context/auth/authContext";

const Login = (props) => {
  const { history } = props;
  const authContext = useContext(AuthContext);

  const { login, error, clearErrors, isAuthenticated } = authContext;

  useEffect(() => {
    if (isAuthenticated) {
      history.push("/");
    }

    if (error === "Invalid Credentials") {
      clearErrors();
    }
  }, [error, isAuthenticated, history]);

  const [user, setUser] = useState({
    email: "",
    password: "",
  });

  const { email, password } = user;

  const onChange = (e) => setUser({ ...user, [e.target.name]: e.target.value });

  const onSubmit = async (e) => {
    e.preventDefault();

    await login({
      email,
      password,
    });
  };

  return (
    <div>
      <h1>
        <span>Login</span>
      </h1>
      <form onSubmit={onSubmit}>
        <div>
          <label htmlFor="user">Email</label>
          <input
            id="email"
            type="text"
            name="email"
            value={email}
            onChange={onChange}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            name="password"
            value={password}
            onChange={onChange}
            required
          />
        </div>
        <input type="submit" value="Login" />
      </form>
      <div>
        <p>Don't have an account?</p>
        <Link to="/registration">Register now</Link>
      </div>
    </div>
  );
};

export default Login;
