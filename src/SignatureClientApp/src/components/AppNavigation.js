import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Navbar from "./Navbar";
import Drawer from "./Drawer";

const navbarMinHeight = "64px";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
  },
  content: {
    flexGrow: 1,
    marginTop: navbarMinHeight,
    overflow: "hidden"
  },
}));

export default function AppNavigation({ children }) {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <Navbar />
      <Drawer />
      <main className={classes.content}>{children}</main>
    </div>
  );
}
