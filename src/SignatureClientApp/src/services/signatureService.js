import { fileToBase64 } from "../common/encodingHelpers";
import { post } from "./baseApiService";

const DefaultSignatureProfile = "LTA";

export const signPdf = async (file, profile = DefaultSignatureProfile) => {
  return await sign(file, profile, "Signature/Pdf/Download", "blob");
};

export const signBinary = async (file, profile = DefaultSignatureProfile) => {
  return await sign(file, profile, "Signature/Binary", "json");
};

const sign = async (file, profile, route, responseType) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    profile: profile,
    b64Bytes: b64Data,
  };

  return await post(route, requestModel, responseType);
};
