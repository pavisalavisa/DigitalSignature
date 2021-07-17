import {
  Button,
  Typography,
  TextField,
  Divider,
  CircularProgress,
  Backdrop,
} from "@material-ui/core";
import React, { useEffect, useState } from "react";
import {
  getPersonalInfo,
  updatePersonalInfo,
} from "../../services/userManagementService";
import { makeStyles } from "@material-ui/core";
import { withSnackbar } from "../Snackbar";
import Certificate from "./Certificate";

const useStyles = makeStyles((theme) => ({
  accountContainer: {
    margin: theme.spacing(4),
    "& > div": {
      marginTop: theme.spacing(3),
      "& > *": {
        marginTop: theme.spacing(2),
      },
    },
  },
  buttonGroup: {
    "& > button:first-child": {
      marginRight: theme.spacing(2),
    },
  },
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: "#fff",
  },
}));

function Account({ snackbarShowMessage }) {
  const { accountContainer, buttonGroup, backdrop } = useStyles();
  const [accountInfo, setAccountInfo] = useState();
  const [isDirty, setIsDirty] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  async function fetchAndAssign() {
    const personalInfo = await getPersonalInfo();
    setAccountInfo(personalInfo);
    setIsDirty(false);
  }

  async function saveChanges() {
    await updatePersonalInfo(accountInfo);
    snackbarShowMessage("Account updated");
    setIsDirty(false);
  }

  useEffect(() => {
    fetchAndAssign();
  }, []);

  return (
    <div className={accountContainer}>
      <div>
        <div>
          <Typography variant="h3">My account</Typography>
          <Typography variant="subtitle2" color="textSecondary">
            Make changes to your account
          </Typography>
        </div>
        <Divider />
      </div>
      <div>
        <div>
          <Typography variant="h4">Personal information</Typography>
          <Typography variant="subtitle2" color="textSecondary">
            Make changes to your personal information
          </Typography>
        </div>

        {!accountInfo ? (
          <CircularProgress color="inherit" />
        ) : (
          <>
            <div>
              <Typography color="textSecondary">Email</Typography>
              <TextField
                value={accountInfo.email || ""}
                onChange={(e) => {
                  setAccountInfo({ ...accountInfo, email: e.target.value });
                  setIsDirty(true);
                }}
              />
            </div>
            <div>
              <Typography color="textSecondary">First name</Typography>
              <TextField
                value={accountInfo.firstName || ""}
                onChange={(e) => {
                  setAccountInfo({ ...accountInfo, firstName: e.target.value });
                  setIsDirty(true);
                }}
              />
            </div>
            <div>
              <Typography color="textSecondary">Last name</Typography>
              <TextField
                value={accountInfo.lastName || ""}
                onChange={(e) => {
                  setAccountInfo({ ...accountInfo, lastName: e.target.value });
                  setIsDirty(true);
                }}
              />
            </div>
            <div>
              <Typography color="textSecondary">Organization name</Typography>
              <TextField
                size="small"
                value={accountInfo.organizationName || ""}
                onChange={(e) => {
                  setAccountInfo({
                    ...accountInfo,
                    organizationName: e.target.value,
                  });
                  setIsDirty(true);
                }}
              />
            </div>
          </>
        )}
        <Divider />
      </div>
      <Certificate
        snackbarShowMessage={snackbarShowMessage}
      />
      <div className={buttonGroup}>
        <Button
          disabled={!isDirty}
          color="primary"
          variant="contained"
          onClick={() => saveChanges()}
        >
          Save
        </Button>
        <Button onClick={() => fetchAndAssign()}>Cancel</Button>
      </div>
      <Backdrop className={backdrop} open={isLoading}>
        <CircularProgress color="inherit" />
      </Backdrop>
    </div>
  );
}

export default withSnackbar(Account);
