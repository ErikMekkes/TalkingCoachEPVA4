import React, {Component} from 'react';
import logo from './logo.svg';
import './App.css';
import Unity from "react-unity-webgl";

class App extends Component {
    render() {
        return (
            <div className="App">
                <header className="App-header">
                    <img src={logo} className="App-logo" alt="logo"/>
                    <h1 className="App-title">Welcome to React</h1>
                </header>
                <p className="App-intro">
                    To get startd, edit <code>src/App.js</code> and save to reload.
                </p>
                <div>
                    <Unity
                        src="unity/Build/Build.json"
                        loader="unity/Build/UnityLoader.js"
                    />
                </div>
            </div>
        );
    }
}

export default App;
