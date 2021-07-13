import { Button, Typography } from "@material-ui/core";
import React, { useContext, useEffect, useState } from "react";
import { getPersonalInfo } from "../services/userManagementService";
import { Divider } from "@material-ui/core";
import { CircularProgress } from "@material-ui/core";
import { TextField } from "@material-ui/core";

function Account() {
  const [accountInfo, setAccountInfo] = useState();
  useEffect(() => {
    async function fetchAndAssign() {
      const personalInfo = await getPersonalInfo();
      setAccountInfo(personalInfo);
    }

    fetchAndAssign();
  }, []);

  return (
    <div>
      <Typography variant="h3">My account</Typography>
      <Typography variant="subtitle2" color="textSecondary">
        Make changes to your account
      </Typography>
      <Divider />
      <div>
        <Typography variant="h4">Personal information</Typography>
        <Typography variant="subtitle2" color="textSecondary">
          Make changes to your personal information
        </Typography>
        {!accountInfo ? (
          <CircularProgress color="inherit" />
        ) : (
          <>
            <Typography>Email</Typography>
            <TextField
              value={accountInfo.email}
              onChange={(e) =>
                setAccountInfo({ ...accountInfo, email: e.target.value })
              }
            />
            <Typography>First name</Typography>
            <TextField
              value={accountInfo.firstName}
              onChange={(e) =>
                setAccountInfo({ ...accountInfo, firstName: e.target.value })
              }
            />
            <Typography>Last name</Typography>
            <TextField
              value={accountInfo.lastName}
              onChange={(e) =>
                setAccountInfo({ ...accountInfo, lastName: e.target.value })
              }
            />
            <Typography>Organization name</Typography>
            <TextField
              size="small"
              value={accountInfo.organizationName}
              onChange={(e) =>
                setAccountInfo({
                  ...accountInfo,
                  organizationName: e.target.value,
                })
              }
            />
          </>
        )}
      </div>
      <Divider />
      <div>
        <Typography variant="h4">Certificate</Typography>
        <Typography variant="subtitle2" color="textSecondary">
          Import/Export certificate used for digital signature services
        </Typography>
        <Button>Import</Button>
        <Button>Export</Button>
      </div>
    </div>
  );
}

export default Account;
