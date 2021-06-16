import React from "react";
import { saveAs } from "file-saver";
import { signPdf } from "../../services/signatureService";
import { verifyPdf } from "../../services/verificationService";
import { Button, Input, Typography, Grid } from "@material-ui/core";
import BorderColorIcon from "@material-ui/icons/BorderColor";
import VerifiedUserIcon from "@material-ui/icons/VerifiedUser";

function PdfSelector(props) {
  const {
    setIsLoadingSubmit,
    setVerificationResult,
    setSelectedFile,
    selectedFile,
    isLoadingSubmit,
  } = props;

  console.log(props);

  const changeHandler = (e) => {
    e.preventDefault();

    console.log(setSelectedFile);

    setSelectedFile(e.target.files[0]);
  };

  const handleVerify = async () => {
    try {
      setIsLoadingSubmit(true);

      const verificationResponse = await verifyPdf(selectedFile);

      setVerificationResult(verificationResponse);
    } finally {
      setIsLoadingSubmit(false);
    }
  };

  const handleSubmission = async (e) => {
    e.preventDefault();

    try {
      setIsLoadingSubmit(true);

      const signedFile = await signPdf(selectedFile);

      saveAs(signedFile, `signed_${selectedFile.name}`);
    } finally {
      setIsLoadingSubmit(false);
    }
  };

  return (
    <Grid container justify="space-around" spacing={2}>
      <Grid item xs={12} md={12} lg={12} xl={12}>
        <Typography align="center" component="h2" variant="h5">
          Select a file you would like to sign
        </Typography>
      </Grid>
      <Grid item xs={6} md={6} lg={6} xl={6}>
        <Input
          type="file"
          variant="outlined"
          onChange={changeHandler}
          placeholder="Choose a PDF file"
        />
      </Grid>
      <Grid item>
        <Grid container spacing={1} justify="flex-end">
          <Grid item>
            <Button
              type="submit"
              onClick={handleSubmission}
              className="submit-button"
              disabled={!selectedFile || isLoadingSubmit}
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
              disabled={!selectedFile || isLoadingSubmit}
              variant="contained"
              startIcon={<VerifiedUserIcon />}
            >
              Verify
            </Button>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}

export default PdfSelector;
