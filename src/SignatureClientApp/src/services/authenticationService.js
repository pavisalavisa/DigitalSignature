import axios from "axios";

const API_URL = "https://localhost:4201/api/";

const register = (username, email, password) => {
  return axios.post(API_URL + "Users/Registration", {
    username,
    email,
    password,
  });
};

const login = async ({username, password}) => {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await axios.post(`${API_URL}Token`, { username, password }, config);
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
