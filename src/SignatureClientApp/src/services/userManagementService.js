import { put, get } from "./baseApiService";

export async function getPersonalInfo() {
  return await get("Users/PersonalInformation");
}

export async function updatePersonalInfo(personalInfo) {
  return await put("Users/PersonalInformation", personalInfo);
}
