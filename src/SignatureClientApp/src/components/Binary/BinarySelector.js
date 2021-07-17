import React, { useState } from "react";
import { saveAs } from "file-saver";
import { signBinary } from "../../services/signatureService";
import { verifyBinary } from "../../services/verificationService";
import { Button, Input, Typography, Grid } from "@material-ui/core";
import BorderColorIcon from "@material-ui/icons/BorderColor";
import VerifiedUserIcon from "@material-ui/icons/VerifiedUser";
import { withSnackbar } from "../Snackbar";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { base64toBlob } from "../../common/encodingHelpers";

function BinarySelector(props) {
  const {
    setIsLoadingSubmit,
    setVerificationResult,
    setSelectedFile,
    selectedFile,
    isLoadingSubmit,
    snackbarShowMessage,
  } = props;

  const [isVerifyDialogOpened, setVerifyDialogOpened] = useState();
  const [signatureFile, setSignatureFile] = useState();

  const originalFileChangeHandler = (e) => {
    e.preventDefault();

    setSelectedFile(e.target.files[0]);
  };

  const signatureFileChangeHandler = (e) => {
    e.preventDefault();

    setSignatureFile(e.target.files[0]);
  };

  const handleVerify = async () => {
    try {
      setIsLoadingSubmit(true);

      const verificationResponse = await verifyBinary(
        selectedFile,
        signatureFile
      );

      setVerificationResult(verificationResponse);
      setVerifyDialogOpened(false);
      snackbarShowMessage("Successfully verified the file");
    } finally {
      setIsLoadingSubmit(false);
    }
  };

  const handleSubmission = async (e) => {
    e.preventDefault();

    try {
      setIsLoadingSubmit(true);

      const { fileName, signedB64Bytes } = await signBinary(selectedFile);

      saveAs(base64toBlob(signedB64Bytes), `${fileName}`);

      snackbarShowMessage("File signature is downloaded.");
    } finally {
      setIsLoadingSubmit(false);
    }
  };

  return (
    <>
      <Grid container justify="center" spacing={10}>
        <Grid item xs={12} md={12} lg={12} xl={12}>
          <Typography align="center" component="h2" variant="h4">
            Select a file you would like to sign
          </Typography>
        </Grid>
        <Grid item>
          <Input
            type="file"
            variant="outlined"
            onChange={originalFileChangeHandler}
            placeholder="Choose a file"
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
                onClick={() => setVerifyDialogOpened(true)}
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
      <Dialog
        open={isVerifyDialogOpened}
        onClose={() => setVerifyDialogOpened(true)}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">Signature verification</DialogTitle>
        <DialogContent>
          <DialogContentText>
            To verify this file, please provide the detached signature file.
            Verification for non-pdf files requires both original and the
            signature file.
          </DialogContentText>
          <Input
            type="file"
            variant="outlined"
            onChange={signatureFileChangeHandler}
            placeholder="Upload file signature"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setVerifyDialogOpened(false)} color="primary">
            Cancel
          </Button>
          <Button onClick={handleVerify} color="primary">
            Submit
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}

export default withSnackbar(BinarySelector);
