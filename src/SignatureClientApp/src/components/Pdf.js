import React, { useState, useEffect } from "react";
import { Container } from "@material-ui/core";
import { Grid } from "@material-ui/core";
import PdfViewer from "./Pdf/Pdf-viewer";
import { Typography } from "@material-ui/core";
import { saveAs } from "file-saver";
import { signPdf } from "../services/signatureService";
import { verifyPdf } from "../services/verificationService";
import { makeStyles } from "@material-ui/core/styles";
import { Paper } from "@material-ui/core";
import { Button } from "@material-ui/core";
import { Input } from "@material-ui/core";
import BorderColorIcon from "@material-ui/icons/BorderColor";
import VerifiedUserIcon from "@material-ui/icons/VerifiedUser";

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
}));

function Pdf(props) {
  const [selectedFile, setSelectedFile] = useState();
  const [isSelected, setIsSelected] = useState(false);
  const [isLoadingSubmit, setIsLoadingSubmit] = useState(false);
  const [pdf, setPdf] = useState();
  const [verificationResult, setVerificationResult] = useState();
  const classes = useStyles();

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
    <Container component="main" maxWidth="lg" className={classes.paper}>
      <Grid container spacing={4}>
        <Grid container xs={4} justify="center">
          <Grid item>
            <Typography align="center" component="h2" variant="h5">
              Select a file you would like to sign
            </Typography>
          </Grid>
          <Grid item>
            <Input
              type="file"
              variant="outlined"
              onChange={changeHandler}
              placeholder="Choose a PDF file"
            />
          </Grid>
          <Grid container item justify="space-around">
            <Grid item>
              <Button
                type="submit"
                onClick={handleSubmission}
                className="submit-button"
                disabled={!selectedFile}
                color="primary"
                variant="contained"
                startIcon={<BorderColorIcon />}
              >
                Sign
              </Button>
            </Grid>
            <Grid item>
              <Button
                type="button"
                onClick={handleVerify}
                className="verify-button"
                disabled={!selectedFile}
                variant="contained"
                startIcon={<VerifiedUserIcon />}
              >
                Verify
              </Button>
            </Grid>
          </Grid>
        </Grid>
        <Grid item xs={12} md={8} lg={8} justify="space-around">
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
