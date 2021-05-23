import React, { useState, useEffect } from "react";

import PdfViewer from "./pdf-viewer";
import "./styles.css";

function App() {
  const [selectedFile, setSelectedFile] = useState();
  const [isSelected, setIsSelected] = useState(false);
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [pdf, setPdf] = useState();

  const changeHandler = (event) => {
    event.preventDefault();
    setSelectedFile(event.target.files[0]);
    setIsSelected(true);
  };

  // Preview PDF
  useEffect(() => {
    setPdf(selectedFile);
  }, [isSelected, selectedFile]);

  const handleDownload = () => {
    const link = document.createElement("a");
    link.download = selectedFile.name;
    link.href = URL.createObjectURL(selectedFile);
    link.click();
    URL.revokeObjectURL(link.href);
  };

  const handleSubmission = (e) => {
    e.preventDefault();
    const formData = new FormData();

    formData.append("File", selectedFile);

    // Post form data to backend, -> Await because it's important
    // to wait for signed document, then preview it in preview window
    // TODO: Implement isLoading & download button
    //  await fetch("URL", {
    //     method: "POST",
    //     body: formData
    //   })
  };

  return (
    <div className="main-container">
      <div className="left-container">
        <input
          type="file"
          name="file"
          className="input"
          onChange={changeHandler}
        />
        <div className="button-container">
          <button
            type="submit"
            onClick={handleSubmission}
            className="submit-button"
          >
            Submit
          </button>
          <button
            type="button"
            onClick={handleDownload}
            className="download-button"
          >
            Download
          </button>
        </div>
      </div>
      <div className="right-container">
        <h2 className="pdf-preview">Preview</h2>
        <div className="pdf-container">
          {isSelected ? (
            <>
              <PdfViewer pdf={pdf} />
            </>
          ) : (
            <div className="empty-box"> PDF not selected </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default App;
