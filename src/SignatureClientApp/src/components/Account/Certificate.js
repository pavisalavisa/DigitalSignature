import React, { useState } from "react";
import {
  Button,
  Typography,
  Input,
  TextField,
  Divider,
  Backdrop,
  CircularProgress,
} from "@material-ui/core";
import { assignCertificate } from "../../services/userManagementService";

import { makeStyles } from "@material-ui/core";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { fileToBase64 } from "../../common/encodingHelpers";

const useStyles = makeStyles((theme) => ({
  buttonGroup: {
    "& > button:first-child": {
      marginRight: theme.spacing(2),
    },
  },
  certificateInput: {
    marginTop: theme.spacing(3),
  },

  backdrop: {
    zIndex: theme.zIndex.drawer + 10,
    color: "#fff",
  },
}));

function Certificate({ snackbarShowMessage }) {
  const { buttonGroup, certificateInput, backdrop } = useStyles();
  const [importDialogOpened, setImportDialogOpened] = useState();
  const [certificate, setCertificate] = useState({
    b64Certificate: null,
    certificatePassword: "",
  });
  const [isLoading, setIsLoading] = useState(false);

  const certificateFileChangeHandler = async (e) => {
    e.preventDefault();

    if (e.target.files[0]) {
      const b64Certificate = await fileToBase64(e.target.files[0]);

      setCertificate({ ...certificate, b64Certificate });
    } else {
      setCertificate({ ...certificate, b64Certificate: null });
    }
  };

  const handleImport = async () => {
    try {
      setIsLoading(true);

      await assignCertificate(certificate);

      setImportDialogOpened(false);
      snackbarShowMessage("Successfully imported the certificate");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div>
      <div>
        <Typography variant="h4">Certificate</Typography>
        <Typography variant="subtitle2" color="textSecondary">
          Import/Export certificate used for digital signature services
        </Typography>
      </div>
      <div className={buttonGroup}>
        <Button variant="outlined" onClick={() => setImportDialogOpened(true)}>
          Import
        </Button>
        <Button variant="outlined">Export</Button>
      </div>
      <Divider />
      <Dialog
        open={importDialogOpened}
        onClose={() => setImportDialogOpened(true)}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">Import certificate</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please select a certificate in .p12 format. Your organization should
            provide you with the personal certificate.
          </DialogContentText>
          <Input
            type="file"
            variant="outlined"
            onChange={certificateFileChangeHandler}
            placeholder="Select .p12 file"
          />
          <div className={certificateInput}>
            <Typography color="textSecondary">Certificate password</Typography>
            <TextField
              type="password"
              size="small"
              value={certificate.certificatePassword}
              onChange={(e) => {
                setCertificate({
                  ...certificate,
                  certificatePassword: e.target.value,
                });
              }}
            />
          </div>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setImportDialogOpened(false)} color="primary">
            Cancel
          </Button>
          <Button
            onClick={handleImport}
            color="primary"
            disabled={
              !certificate.b64Certificate || !certificate.certificatePassword
            }
          >
            Import
          </Button>
        </DialogActions>
        <Backdrop className={backdrop} open={isLoading}>
          <CircularProgress color="inherit" />
        </Backdrop>{" "}
      </Dialog>
    </div>
  );
}

export default Certificate;
