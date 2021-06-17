import React, { useState, useEffect } from "react";
import PdfViewer from "./PdfViewer";
import PdfSelector from "./PdfSelector";
import { makeStyles } from "@material-ui/core/styles";
import {
  Paper,
  Typography,
  Grid,
  Container,
  Backdrop,
  CircularProgress,
} from "@material-ui/core";
import VerificationTable from "../VerificationTable";

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    alignItems: "center",
    justifyItems: "center",
  },
  pdfPreviewContainer: {
    marginTop: theme.spacing(3),
    height: "60vh",
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
}));

function Pdf(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [pdf, setPdf] = useState();
  const [verificationResult, setVerificationResult] = useState();
  const classes = useStyles();
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: null,
    severity: "success",
  });

  // Preview PDF
  useEffect(() => {
    setPdf(selectedFile);
  }, [selectedFile]);

  return (
    <Container component="main" maxWidth="xl" className={classes.paper}>
      <Backdrop className={classes.backdrop} open={isLoadingSubmit}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <Grid container spacing={4} justify="space-between">
        <Grid item xs={12} sm={12} md={5} lg={5} xl={5}>
          <Grid container direction="column" spacing={4}>
            <Grid item xs={12}>
              <PdfSelector
                setIsLoadingSubmit={setIsLoadingSubmit}
                setVerificationResult={setVerificationResult}
                setSelectedFile={setSelectedFile}
                selectedFile={selectedFile}
                isLoadingSubmit={isLoadingSubmit}
              />
            </Grid>
            <Grid item xs={12}>
              <VerificationTable verificationResult={verificationResult} />
            </Grid>
          </Grid>
        </Grid>
        <Grid item xs={12} sm={12} md={6} lg={6} xl={6} justify="center">
          <Typography align="center" component="h2" variant="h5">
            Preview
          </Typography>
          <Paper>
            <Grid
              container
              justify="center"
              className={classes.pdfPreviewContainer}
            >
              <PdfViewer pdf={pdf} />
            </Grid>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
}
export default Pdf;
