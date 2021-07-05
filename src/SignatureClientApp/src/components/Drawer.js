import React, {useContext} from "react";
import { makeStyles } from "@material-ui/core/styles";
import Drawer from "@material-ui/core/Drawer";
import Toolbar from "@material-ui/core/Toolbar";
import List from "@material-ui/core/List";
import Divider from "@material-ui/core/Divider";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";
import FolderIcon from "@material-ui/icons/Folder";
import HistoryIcon from "@material-ui/icons/History";
import AccountBoxIcon from "@material-ui/icons/AccountBox";
import ExitToAppIcon from "@material-ui/icons/ExitToApp";
import { withRouter } from "react-router";
import AuthContext from "../context/auth/authContext";

const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
  drawer: {
    width: drawerWidth,
    flexShrink: 0,
  },
  drawerPaper: {
    width: drawerWidth,
  },
  drawerContainer: {
    overflow: "auto",
  },
}));

const primaryDrawerItems = [
  {
    text: "PDF",
    DrawerIcon: PictureAsPdfIcon,
    onClick: (history) => {
      history.push("/pdf");
    },
  },
  {
    text: "Other Files",
    DrawerIcon: FolderIcon,
    onClick: (history) => {
      history.push("/other-files");
    },
  },
  {
    text: "History",
    DrawerIcon: HistoryIcon,
    onClick: (history) => {
      history.push("/history");
    },
  },
];

function AppDrawer({ history }) {
  const classes = useStyles();
  const { logout } = useContext(AuthContext);

  return (
    <Drawer
      className={classes.drawer}
      variant="permanent"
      classes={{
        paper: classes.drawerPaper,
      }}
    >
      <Toolbar />
      <div className={classes.drawerContainer}>
        <List>
          {primaryDrawerItems.map(({ text, DrawerIcon, onClick }) => (
            <ListItem button key={text} onClick={() => onClick(history)}>
              <ListItemIcon>
                <DrawerIcon />
              </ListItemIcon>
              <ListItemText primary={text} />
            </ListItem>
          ))}
        </List>
        <Divider />
        <List>
          <ListItem button onClick={() => history.push("/account")}>
            <ListItemIcon>
              <AccountBoxIcon />
            </ListItemIcon>
            <ListItemText primary="My account" />
          </ListItem>
          <ListItem button onClick={() => logout()}>
            <ListItemIcon>
              <ExitToAppIcon />
            </ListItemIcon>
            <ListItemText primary="Log out" />
          </ListItem>
        </List>
      </div>
    </Drawer>
  );
}

export default withRouter(AppDrawer);
