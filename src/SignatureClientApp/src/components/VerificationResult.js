import React from "react";
import { withStyles, makeStyles } from "@material-ui/core/styles";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import { Typography } from "@material-ui/core";
import { Slide } from "@material-ui/core";
import CheckIcon from "@material-ui/icons/Check";
import ErrorIcon from "@material-ui/icons/Error";
import green from "@material-ui/core/colors/green";
import red from "@material-ui/core/colors/red";

const StyledTableCell = withStyles((theme) => ({
  head: {
    backgroundColor: theme.palette.primary,
    color: theme.palette.common.black,
    fontWeight: "bold"
  },
  body: {
    fontSize: 14,
  },
}))(TableCell);

const StyledTableRow = withStyles((theme) => ({
  root: {
    "&:nth-of-type(odd)": {
      backgroundColor: theme.palette.action.hover,
    },
  },
}))(TableRow);

const useStyles = makeStyles({
  table: {
    minWidth: 700,
  },
});

function VerificationResult({ verificationResult }) {
  const classes = useStyles();

  return !verificationResult ? null : (
    <Slide
      direction="right"
      in={!!verificationResult}
      mountOnEnter
      unmountOnExit
    >
      <div>
        {verificationResult.signaturesCount > 0 ? (
          <TableContainer>
            <Table className={classes.table} aria-label="customized table">
              <TableHead>
                <TableRow>
                  <StyledTableCell>Signed by</StyledTableCell>
                  <StyledTableCell>Signature time</StyledTableCell>
                  <StyledTableCell>Result</StyledTableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {verificationResult &&
                  verificationResult.signatures.map((sig) => (
                    <StyledTableRow key={sig.id}>
                      <StyledTableCell>{sig.signedBy}</StyledTableCell>
                      <StyledTableCell>{sig.signatureTime}</StyledTableCell>
                      <StyledTableCell>
                        {sig.result === "TOTAL_PASSED" ? (
                          <CheckIcon style={{ color: green[500] }} />
                        ) : (
                          <ErrorIcon style={{ color: red[500] }} />
                        )}
                      </StyledTableCell>
                    </StyledTableRow>
                  ))}
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Typography variant="h5" component="h3" align="center">
            This file has no signatures.
          </Typography>
        )}
      </div>
    </Slide>
  );
}

export default VerificationResult;
