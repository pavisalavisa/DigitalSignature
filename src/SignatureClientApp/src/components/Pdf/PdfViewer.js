import React, { useState } from "react";
import { Document, Page } from "react-pdf/dist/esm/entry.webpack";
import {
  Typography,
  CircularProgress,
  Box,
  makeStyles,
} from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  previewBox: {
    height: "75vh",
    display: "flex",
  },
  previewBoxItem: {
    alignSelf: "center",
  },
}));

function NoData() {
  const { previewBox, previewBoxItem } = useStyles();

  return (
    <Box className={previewBox}>
      <Typography
        color="textSecondary"
        align="center"
        component="span"
        variant="h4"
        className={previewBoxItem}
      >
        PDF not selected
      </Typography>
    </Box>
  );
}

function UploadProgress() {
  const { previewBox, previewBoxItem } = useStyles();

  return (
    <Box className={previewBox}>
      <CircularProgress className={previewBoxItem} />
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
      loading={<UploadProgress />}
      noData={<NoData />}
    >
      {Array.from(new Array(numPages), (_, index) => (
        <Page key={`page_${index + 1}`} pageNumber={index + 1} />
      ))}
    </Document>
  );
}
