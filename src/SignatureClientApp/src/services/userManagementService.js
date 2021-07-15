import { put, get, post } from "./baseApiService";

export async function getPersonalInfo() {
  return await get("Users/PersonalInformation");
}

export async function updatePersonalInfo(personalInfo) {
  return await put("Users/PersonalInformation", personalInfo);
}

export async function assignCertificate({b64Certificate, certificatePassword}){
  return await post("Users/Certificate", {b64Certificate, certificatePassword});
}
