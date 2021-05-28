import React, { useState } from "react";
import { signBinary } from "../services/signatureService";

import "./zip.css";

function Zip(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isSelected, setIsSelected] = useState(false);
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);

  const changeHandler = (event) => {
    event.preventDefault();
    setSelectedFile(event.target.files[0]);
    setIsSelected(true);
  };

  const handleDownload = () => {
    const link = document.createElement("a");
    link.download = selectedFile.name;
    link.href = URL.createObjectURL(selectedFile);
    link.click();
    URL.revokeObjectURL(link.href);
  };

  const handleSubmission = async (e) => {
    e.preventDefault();

    const signedFile = await signBinary(selectedFile);

    console.log(signedFile);
    
    saveAs(signedFile, `detachedSignature_${selectedFile.name}`);
  };

  return (
    <div className="main-container-zip">
      <div className="flex-zip">
        <div class="input-group mb-3">
          <input
            type="file"
            name="file"
            class="form-control"
            id="inputGroupFile02"
            onChange={changeHandler}
          />
          <label class="input-group-text" for="inputGroupFile02">
            Upload Zip
          </label>
        </div>
        {isSelected ? (
          <div>
            <p>File name: {selectedFile.name}</p>
            <p>File type: {selectedFile.type}</p>
          </div>
        ) : (
          <div>
            <p>File name: not selected</p>
            <p>File type: not selected</p>
          </div>
        )}
        <div className="button-container-zip">
          <button
            type="submit"
            onClick={handleSubmission}
            className="submit-button-zip"
          >
            Submit
          </button>
          <button
            type="button"
            onClick={handleDownload}
            className="download-button-zip"
          >
            Download
          </button>
        </div>
      </div>
    </div>
  );
}
export default Zip;
