import Typography from "@material-ui/core/Typography";
import React from "react";
import { Link } from "react-router-dom";
import { BottomNavigation } from "@material-ui/core";

export default () => {
  return (
    <footer
      style={{
        position: "absolute",
        bottom: 0,
        display: "flex",
        justifyContent: "center",
      }}
    >
      <BottomNavigation >
        <Typography variant="body2" color="textSecondary" align="center">
          {"Copyright © "}
          <Link color="inherit" href="https://material-ui.com/">
            Digital signature
          </Link>{" "}
          {new Date().getFullYear()}
          {"."}
        </Typography>
      </BottomNavigation>
    </footer>
  );
};
