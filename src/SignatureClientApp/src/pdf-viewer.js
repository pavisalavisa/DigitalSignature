import React, { useState } from "react";
import { Document, Page } from "react-pdf/dist/esm/entry.webpack";

export default function PdfViewer(props) {
  const [numPages, setNumPages] = useState(null);

  function onDocumentLoadSuccess({ numPages }) {
    setNumPages(numPages);
  }

  const { pdf } = props;

  return (
    <Document file={pdf} onLoadSuccess={onDocumentLoadSuccess}>
      {Array.from(new Array(numPages), (_, index) => (
        <Page key={`page_${index + 1}`} pageNumber={index + 1} width={800} />
      ))}
    </Document>
  );
}
