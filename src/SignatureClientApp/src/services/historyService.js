import { get } from "./baseApiService";

export const getHistory = async (page = 1, size = 20) => {
  return await get("Events", { page, size });
};

export const getEventDetails = async (id) => {
  return await get(`Events/${id}`);
};
