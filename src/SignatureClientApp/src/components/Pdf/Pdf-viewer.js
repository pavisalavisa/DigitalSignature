import React, { useState } from "react";
import { Document, Page } from "react-pdf/dist/esm/entry.webpack";
import { Typography, CircularProgress, Box, makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme)=>({
  noData: {
    height:"50vh",
    display:"flex",
  },
  noDataText:{
    alignSelf: "center",
  }
}));

function NoData() {
  const {noData, noDataText} = useStyles();

  return (
    <Box className={noData}>
      <Typography color="textSecondary" align="center" component="span" className={noDataText}>
        PDF not selected{" "}
      </Typography>
    </Box>
  );
}

export default function PdfViewer(props) {
  const [numPages, setNumPages] = useState(null);

  function onDocumentLoadSuccess({ numPages }) {
    setNumPages(numPages);
  }

  const { pdf } = props;

  return (
    <Document
      file={pdf}
      onLoadSuccess={onDocumentLoadSuccess}
      loading={<CircularProgress />}
      noData={<NoData />}
    >
      {Array.from(new Array(numPages), (_, index) => (
        <Page key={`page_${index + 1}`} pageNumber={index + 1} />
      ))}
    </Document>
  );
}
