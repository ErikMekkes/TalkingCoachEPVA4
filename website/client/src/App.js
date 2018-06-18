import React, {Component} from 'react';
import './App.css';
import PropTypes from 'prop-types';
import UnityApp from "./comp/UnityApp";
import CssBaseline from "@material-ui/core/es/CssBaseline/CssBaseline";
import MyAppBar from "./comp/MyAppBar";
import AppDrawer from "./comp/AppDrawer";
import {withStyles} from "@material-ui/core/styles/index";
import TextInput from "./comp/TextInput";

const styles = {
	root: {
		flexGrow: 1,
	},
	flex: {
		flex: 1,
	},
	menuButton: {
		marginLeft: -12,
		marginRight: 20,
	},
};

class App extends Component {
	state = {users: []};

	constructor(props) {
		super(props);
		this.state = {open: false, users: []};
		this.drawerHandler = this.drawerHandler.bind(this)
	}

	handleTouchMap() {
		this.setState({open: !this.state.open});
		console.log("HandletouchMap")
	}

	componentDidMount() {
		fetch('/users')
				.then(res => res.json())
				.then(users => this.setState({users}));
	}

	drawerHandler(open) {
		this.setState({
			open: open
		})
	}

	render() {
		const {classes} = this.props;
		return (
				<div className={classes.root}>
					<CssBaseline/>
					<MyAppBar
							onMenuClick={this.handleTouchMap.bind(this)}
					/>
					<AppDrawer open={this.state.open} handler={this.drawerHandler}/>
					<TextInput />
					<UnityApp/>
					{/*<h1>API Test</h1>*/}
					{/*{this.state.users.map(user =>*/}
					{/*<div key={user.id}>{user.username}</div>*/}
					{/*)}*/}
				</div>
		);
	}
}

App.propTypes = {
	classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(App);
