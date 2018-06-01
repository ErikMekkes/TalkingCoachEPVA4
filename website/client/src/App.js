import React, {Component} from 'react';
import './App.css';
import Unity from 'react-unity-webgl';

class App extends Component {
    state = {users: []};

    componentDidMount() {
        fetch('/users')
            .then(res => res.json())
            .then(users => this.setState({users}));
    }

    render() {
        return (
            <div className="App">
                <div className="unity-wrapper">
                    <Unity className="unity-container"
                           src="unity/Build/Build.json"
                           loader="unity/Build/UnityLoader.js"
                    />
                </div>
                <h1>API Demo</h1>
                {this.state.users.map(user =>
                    <div key={user.id}>{user.username}</div>
                )}
            </div>
        );
    }
}

export default App;
