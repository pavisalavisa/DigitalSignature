import { post } from "./baseApiService";

const register = async ({ email, password }) => {
  return await post("Users/Registration", {
    email,
    password,
  });
};

const login = async ({ email, password }) => {
  return await post("Token", { email, password });
};

const logout = () => {
  localStorage.removeItem("currentUser");
};

const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem("currentUser"));
};

export default {
  register,
  login,
  logout,
  getCurrentUser,
};
