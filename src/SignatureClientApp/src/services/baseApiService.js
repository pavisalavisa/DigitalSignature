import axios from "axios";

const API_URL = "https://localhost:4201/api/";

export async function post(route, requestModel, responseType = "json") {
  const response = await axios.post(API_URL + route, requestModel, {
    responseType,
  });

  return response.data;
}

export async function get(
  route,
  queryStringModel = null,
  responseType = "json"
) {
  const response = await axios.get(API_URL + route, {
    params: { ...queryStringModel },
    responseType,
  });

  return response.data;
}

export async function put(route, requestModel, responseType = "json") {
  const response = await axios.put(API_URL + route, requestModel, {
    responseType,
  });

  return response.data;
}
