import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import { withRouter } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
  },
  title: {
    flexGrow: 1,
    padding: theme.spacing(2),
  },
  titleContainer: {
    "&:hover": {
      cursor: "pointer",
    },
  },
}));

function Navbar({ history }) {
  const classes = useStyles();

  return (
    <AppBar position="fixed" className={classes.appBar}>
      <Toolbar>
        <div
          onClick={() => history.push("/")}
          className={classes.titleContainer}
        >
          <Typography variant="h6" className={classes.title}>
            Digital signature
          </Typography>
        </div>
      </Toolbar>
    </AppBar>
  );
}

export default withRouter(Navbar);
