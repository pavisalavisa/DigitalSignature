import { fileToBase64 } from "../common/encodingHelpers";
import { post } from "./baseApiService";

export const verifyPdf = async (file) => {
  const b64Data = await fileToBase64(file);

  const requestModel = {
    fileName: file.name,
    b64Bytes: b64Data,
  };

  return await post("Verification/Pdf", requestModel);
};
