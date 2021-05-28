import axios from "axios";
import { b64Certificate, certificatePassword } from "../constants";
import { fileToBase64 } from "../common/encodingHelpers";

const API_URL = "http://localhost:8080/api/v1/signature/";

export const signPdf = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "pdf/download", "blob");
};

export const signBinary = async (file, includeTimestamp = false) => {
  return await sign(file, includeTimestamp, "binary", "json");
};

const sign = (file, includeTimestamp, route, responseType) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    includeTimestamp: includeTimestamp,
    certificate: {
      b64Certificate,
      certificatePassword,
    },
    b64Bytes: b64Data,
  };

  const response = await axios.post(API_URL + route, requestModel, {
    responseType,
  });

  return response.data;
};
