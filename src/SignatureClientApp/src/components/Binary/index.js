import React, { useState } from "react";
import BinarySelector from "./BinarySelector";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Backdrop, CircularProgress } from "@material-ui/core";
import VerificationResult from "../VerificationResult";
import { Divider } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: "#fff",
  },
  selectorContainer: {
    margin: theme.spacing(4, 0, 0),
  },
}));

function Binary(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [verificationResult, setVerificationResult] = useState();
  const classes = useStyles();

  return (
    <Grid container direction="row" justify="space-between" spacing={4}>
      <Backdrop className={classes.backdrop} open={isLoadingSubmit}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <Grid item xs={12} className={classes.selectorContainer}>
        <BinarySelector
          setIsLoadingSubmit={setIsLoadingSubmit}
          setVerificationResult={setVerificationResult}
          setSelectedFile={setSelectedFile}
          selectedFile={selectedFile}
          isLoadingSubmit={isLoadingSubmit}
        />
      </Grid>
      <Grid item xs={12}>
        <Divider />
      </Grid>
      <Grid item xs={12}>
        <VerificationResult verificationResult={verificationResult} />
      </Grid>
    </Grid>
  );
}
export default Binary;
