import React from "react";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import {
  Card,
  CardActionArea,
  CardContent,
  CardActions,
} from "@material-ui/core";
import { Button } from "@material-ui/core";
import { Container } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  icon: {
    fontSize: "10em",
  },
  signatureTypes: {
    marginTop: theme.spacing(8),
  },
}));

export default (props) => {
  const classes = useStyles();
  const { history } = props;

  return (
    <Container component="main" maxWidth="md">
      <div className={classes.paper}>
        <Typography component="h1" variant="h4">
          Choose the file type
        </Typography>
        <Grid container spacing={2} className={classes.signatureTypes}>
          <Grid item xs={12} sm={6} justify="center">
            <Card>
              <CardActionArea>
                <CardContent>
                  <Typography gutterBottom variant="h5" component="h2">
                    PDF
                  </Typography>
                  <Typography
                    variant="body2"
                    color="textSecondary"
                    component="p"
                  >
                    PDF files are signed with PAdES (PDF Advanced Electronic
                    Signatures) compliant signature and are easily verified by
                    any PDF reader
                  </Typography>
                </CardContent>
              </CardActionArea>
              <CardActions>
                <Button
                  size="medium"
                  color="primary"
                  onClick={() => history.push("/pdf")}
                >
                  Sign or verify
                </Button>
              </CardActions>
            </Card>
          </Grid>

          <Grid item xs={12} sm={6} justify="center">
            <Card>
              <CardActionArea>
                <CardContent>
                  <Typography gutterBottom variant="h5" component="h2">
                    Any other
                  </Typography>
                  <Typography
                    variant="body2"
                    color="textSecondary"
                    component="p"
                  >
                    Files are treated as plain binary files. The file is signed
                    with XAdES compliant signature. The result signature is
                    detached from the file.
                  </Typography>
                </CardContent>
              </CardActionArea>
              <CardActions>
                <Button
                  size="medium"
                  color="primary"
                  onClick={() => history.push("/binary")}
                >
                  Sign or verify
                </Button>
              </CardActions>
            </Card>
          </Grid>
        </Grid>
      </div>
    </Container>
  );
};
