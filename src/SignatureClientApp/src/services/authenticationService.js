import axios from "axios";

const API_URL = "https://localhost:4201/api/";

const register = async ({ email, password }) => {
  return await axios.post(API_URL + "Users/Registration", {
    email,
    password,
  });
};

const login = async ({ email, password }) => {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await axios.post(`${API_URL}Token`, { email, password }, config);
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
