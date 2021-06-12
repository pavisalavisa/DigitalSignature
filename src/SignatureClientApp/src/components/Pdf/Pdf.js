import React, { useState, useEffect } from "react";

import PdfViewer from "./Pdf-viewer";
import "./pdf.css";
import { saveAs } from "file-saver";
import { signPdf } from "../../services/signatureService";
import { verifyPdf } from "../../services/verificationService";

function Pdf(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isSelected, setIsSelected] = useState(false);
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [pdf, setPdf] = useState();
  const [verificationResult, setVerificationResult] = useState();

  const changeHandler = (event) => {
    event.preventDefault();
    setSelectedFile(event.target.files[0]);
    console.log(event.target.files[0]);
    setIsSelected(true);
  };
  // Preview PDF
  useEffect(() => {
    setPdf(selectedFile);
  }, [isSelected, selectedFile]);

  const handleVerify = async () => {
    const verificationResponse = await verifyPdf(selectedFile);

    console.log(verificationResponse);
    setVerificationResult(verificationResponse);
  };

  const handleSubmission = async (e) => {
    e.preventDefault();

    const signedFile = await signPdf(selectedFile);

    saveAs(signedFile, `signed_${selectedFile.name}`);
  };

  return (
    <div className="main-container">
      <div className="left-container">
        <p className="text-muted">Please select a PDF you would like to sign</p>
        <div className="input-group mb-3">
          <input
            type="file"
            name="file"
            className="form-control"
            id="inputGroupFile02"
            onChange={changeHandler}
          />
          <label className="input-group-text" htmlFor="inputGroupFile02">
            Upload PDF
          </label>
        </div>
        <div className="button-container">
          <button
            type="submit"
            onClick={handleSubmission}
            className="submit-button"
            disabled={!selectedFile}
          >
            Sign
          </button>
          <button
            type="button"
            onClick={handleVerify}
            className="verify-button"
            disabled={!selectedFile}
          >
            Verify
          </button>
        </div>
        {!!verificationResult ? (
          <div>
            <p>Validation time: {verificationResult.validationTime}</p>
            <p>Signatures count: {verificationResult.signaturesCount}</p>
            <p>
              Verification result:{" "}
              {verificationResult.jaxbModel.signatureOrTimestamp[0]
                .indication === "TOTAL_PASSED"
                ? "OK"
                : "NOK"}
            </p>
          </div>
        ) : null}
      </div>
      <div className="right-container">
        <h2 className="pdf-preview">Preview</h2>
        <div className="pdf-container">
          {isSelected ? (
            <PdfViewer pdf={pdf} />
          ) : (
            <div className="empty-box"> PDF not selected </div>
          )}
        </div>
      </div>
    </div>
  );
}
export default Pdf;
