import React, {Component} from 'react';
import './App.css';
import UnityApp from "./comp/UnityApp";
import CssBaseline from "@material-ui/core/es/CssBaseline/CssBaseline";
import MyAppBar from "./comp/MyAppBar";
import AppDrawer from "./comp/AppDrawer";

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
		return (
				<React.Fragment>
					<CssBaseline/>
					<MyAppBar
							onMenuClick = { this.handleTouchMap.bind(this) }
					/>
					<AppDrawer open={this.state.open} handler={this.drawerHandler} />
					<UnityApp/>
					{/*<h1>API Test</h1>*/}
					{/*{this.state.users.map(user =>*/}
							{/*<div key={user.id}>{user.username}</div>*/}
					{/*)}*/}
				</React.Fragment>
		);
	}
}

export default App;
