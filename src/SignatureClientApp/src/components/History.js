import React, { useEffect, useState } from "react";
import { Typography } from "@material-ui/core";
import { getHistory } from "../services/historyService";
import { Pagination } from "@material-ui/lab";
import { CircularProgress } from "@material-ui/core";
import { Grid } from "@material-ui/core";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import CheckIcon from "@material-ui/icons/Check";
import ErrorIcon from "@material-ui/icons/Error";
import green from "@material-ui/core/colors/green";
import red from "@material-ui/core/colors/red";
import { makeStyles } from "@material-ui/core/styles";

const useStyles = makeStyles({
  table: {
    minWidth: 700,
  },
  tableHeaderText: {
    fontWeight: "bold",
  },
});

function History(props) {
  const [history, setHistory] = useState();
  const [paging, setPaging] = useState({ page: 1, pageSize: 10 });
  const classes = useStyles();

  useEffect(async () => {
    const history = await getHistory(paging.page, paging.pageSize);

    setHistory(history);
  }, [paging]);

  const handlePageChange = (_, value) => {
    if (value !== paging.page) {
      setPaging({ ...paging, page: value });
    }
  };

  return (
    <Grid container direction="row" spacing={4} justify="center">
      <Grid item>
        <Typography variant="h5" component="h3" align="center">
          History
        </Typography>
      </Grid>
      <Grid item xs={12}>
        <TableContainer>
          <Table className={classes.table} aria-label="customized table">
            <TableHead>
              <TableRow>
                <TableCell className={classes.tableHeaderText}>
                  Document name
                </TableCell>
                <TableCell className={classes.tableHeaderText}>
                  Action
                </TableCell>
                <TableCell className={classes.tableHeaderText}>Date</TableCell>
                <TableCell className={classes.tableHeaderText}>
                  Success
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {!history ? (
                <TableRow>
                  <TableCell colSpan={4} align="center">
                    <CircularProgress color="inherit" />
                  </TableCell>
                </TableRow>
              ) : (
                history.items.map((h) => (
                  <TableRow key={h.id}>
                    <TableCell>{h.inputDocumentName}</TableCell>

                    <TableCell>{h.type}</TableCell>
                    <TableCell>{h.created}</TableCell>
                    <TableCell>
                      {h.isSuccessful ? (
                        <CheckIcon style={{ color: green[500] }} />
                      ) : (
                        <ErrorIcon style={{ color: red[500] }} />
                      )}
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Grid>
      <Grid item>
        {!!history && (
          <Pagination
            count={Math.ceil(history.count / paging.pageSize)}
            page={paging.page}
            onChange={handlePageChange}
          />
        )}
      </Grid>
    </Grid>
  );
}

export default History;
