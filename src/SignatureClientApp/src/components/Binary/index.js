import React, { useState, useEffect } from "react";
import BinarySelector from "./BinarySelector";
import { makeStyles } from "@material-ui/core/styles";
import { Paper, Grid, Backdrop, CircularProgress } from "@material-ui/core";
import VerificationResult from "../VerificationResult";
import { Divider } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  pdfPreviewContainer: {
    height: "93.5vh",
    overflow: "scroll",
  },
  form: {
    width: "100%", // Fix IE 11 issue.
    marginTop: theme.spacing(3),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
  spinner: {
    margin: theme.spacing(3, 0, 2),
  },
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: "#fff",
  },
  actionGrid: {
    padding: theme.spacing(4, 0, 0, 4),
  },
}));

function Binary(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [pdf, setPdf] = useState();
  const [verificationResult, setVerificationResult] = useState();
  const classes = useStyles();

  // Preview PDF
  useEffect(() => {
    setPdf(selectedFile);
    setVerificationResult(null);
  }, [selectedFile]);

  return (
    <Grid container direction="row" justify="space-between">
      <Backdrop className={classes.backdrop} open={isLoadingSubmit}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <Grid item xs={12}>
        <BinarySelector
          setIsLoadingSubmit={setIsLoadingSubmit}
          setVerificationResult={setVerificationResult}
          setSelectedFile={setSelectedFile}
          selectedFile={selectedFile}
          isLoadingSubmit={isLoadingSubmit}
        />
      </Grid>
      <Divider />
      <Grid item xs={12}>
        <VerificationResult verificationResult={verificationResult} />
      </Grid>
    </Grid>
  );
}
export default Binary;
