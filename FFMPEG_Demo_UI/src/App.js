import React, { Component } from 'react';
import { HashRouter as Router } from 'react-router-dom'; // BrowserRouter  HashRouter

import Routes from './Routes';

class App extends Component {  
  render() {
    return (
      <Router>
        <div className="App">
          <Routes />          
        </div>
      </Router>
    );
  }
}

export default App;
