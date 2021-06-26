import { fileToBase64 } from "../common/encodingHelpers";
import { post } from "./baseApiService";

export const signPdf = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "Signature/Pdf/Download", "blob");
};

export const signBinary = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "Signature/Binary", "json");
};

const sign = async (file, includeTimestamp, route, responseType) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    includeTimestamp: includeTimestamp,
    b64Bytes: b64Data,
  };

  return await post(route, requestModel, responseType);
};
