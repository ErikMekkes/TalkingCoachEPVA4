import React from 'react';
import PropTypes from 'prop-types';
import {withStyles} from '@material-ui/core/styles';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import ListItem from "@material-ui/core/es/ListItem/ListItem";
import ListItemIcon from "@material-ui/core/es/ListItemIcon/ListItemIcon";
import ListItemText from "@material-ui/core/es/ListItemText/ListItemText";
import InboxIcon from '@material-ui/icons/Inbox';
import DraftsIcon from '@material-ui/icons/Drafts';
import InsertComment from '@material-ui/icons/InsertComment'

const styles = {
	list: {
		width: 250,
	},
	fullList: {
		width: 'auto',
	},
};

class AppDrawer extends React.Component {

	state = {
		open: false
	};

	toggleDrawer = (open) => () => {
		console.log(this.props);
		this.props.handler(open);
	};

	render() {
		const {classes} = this.props;

		const sideList = (
				<div className={classes.list}>
					<ListItem button>
						<ListItemIcon>
							<InboxIcon/>
						</ListItemIcon>
						<ListItemText primary="Some"/>
					</ListItem>
					<ListItem button>
						<ListItemIcon>
							<DraftsIcon/>
						</ListItemIcon>
						<ListItemText primary="Debug"/>
					</ListItem>
					<ListItem button>
						<ListItemIcon>
							<InsertComment/>
						</ListItemIcon>
						<ListItemText primary="Options"/>
					</ListItem>
				</div>
		);

		return (
				<div>
					<SwipeableDrawer
							open={this.props.open}
							onClose={this.toggleDrawer(false)}
							onOpen={this.toggleDrawer(true)}
					>
						<div
								tabIndex={0}
								role="button"
								onClick={this.toggleDrawer(false)}
								onKeyDown={this.toggleDrawer(false)}
						>
							{sideList}
						</div>
					</SwipeableDrawer>
				</div>
		);
	}
}

AppDrawer.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(AppDrawer);