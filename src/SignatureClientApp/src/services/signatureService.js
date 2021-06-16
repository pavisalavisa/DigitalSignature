import axios from "axios";
import { fileToBase64 } from "../common/encodingHelpers";

const API_URL = "https://localhost:4201/api/Signature/";

export const signPdf = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "Pdf/Download", "blob");
};

export const signBinary = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "Binary", "json");
};

const sign = async (file, includeTimestamp, route, responseType) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    includeTimestamp: includeTimestamp,
    b64Bytes: b64Data,
  };

  const response = await axios.post(API_URL + route, requestModel, {
    responseType,
  });

  return response.data;
};
