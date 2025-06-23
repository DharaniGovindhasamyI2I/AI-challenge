import React from 'react';
import OldDashboard, { BrokenHookComponent } from './oldDashboard';
import NewDashboard, { BrokenHookComponent as NewBrokenHookComponent } from './newDashboard';

function App() {
  return (
    <div className="App">
      <OldDashboard />
      <NewDashboard />
      <NewBrokenHookComponent />
    </div>
  );
}

export default App;
