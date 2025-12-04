import React from 'react';
import ProdutoList from './components/ProdutoList';
import CategoryList from './components/CategoryList';

function App() {
  return (

    <div style={{ padding: '20px' }}>
      <h1>Meu App</h1>
      <div style= {{display:'grid', gridTemplateColumns:'1fr 1fr'}}>
        <div style={{borderRight: '4px solid black', minHeight:'100vh', marginRight:'10px'}}><ProdutoList /></div>
        <div><CategoryList/></div>
      </div>
      
    </div>

  );
}

export default App;