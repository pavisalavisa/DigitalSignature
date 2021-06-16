import axios from "axios";
import { fileToBase64 } from "../common/encodingHelpers";

const API_URL = "https://localhost:4201/api/Verification/";

export const verifyPdf = async (file) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    b64Bytes: b64Data,
  };

  const response = await axios.post(API_URL + "Pdf", requestModel);

  return response.data;
};
