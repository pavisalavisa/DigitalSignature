import Typography from "@material-ui/core/Typography";
import React from "react";
import { Link } from "react-router-dom";

export default () => {
  return (
    <Typography variant="body2" color="textSecondary" align="center">
      {"Copyright © "}
      <Link color="inherit" href="https://material-ui.com/">
        Digital signature
      </Link>{" "}
      {new Date().getFullYear()}
      {"."}
    </Typography>
  );
};
