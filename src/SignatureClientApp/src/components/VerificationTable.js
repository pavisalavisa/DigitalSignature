import React from "react";
import { withStyles, makeStyles } from "@material-ui/core/styles";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import red from '@material-ui/core/colors/red';

const StyledTableCell = withStyles((theme) => ({
  head: {
    backgroundColor: theme.palette.primary,
    color: theme.palette.common.black,
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

function VerificationTable({ verificationResult }) {
  const classes = useStyles();

  console.log(verificationResult);
  
  return (
    <TableContainer component={Paper}>
      <Table className={classes.table} aria-label="customized table">
        <TableHead>
          <TableRow>
            <StyledTableCell>Signature</StyledTableCell>
            <StyledTableCell align="right">Result</StyledTableCell>
            <StyledTableCell align="right">Format</StyledTableCell>
            <StyledTableCell align="right">Signed by</StyledTableCell>
            <StyledTableCell align="right">Signature time</StyledTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {verificationResult.signatures.map((sig) => (
            <StyledTableRow key={sig.id}>
              <StyledTableCell component="th" scope="row">
                {sig.id.substring(0,8)}
              </StyledTableCell>
              <StyledTableCell align="right">{sig.result}</StyledTableCell>
              <StyledTableCell align="right">{sig.signatureFormat}</StyledTableCell>
              <StyledTableCell align="right">{sig.signedBy}</StyledTableCell>
              <StyledTableCell align="right">{sig.signatureTime}</StyledTableCell>
            </StyledTableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default VerificationTable;
