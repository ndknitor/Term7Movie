import React, { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import axios from 'axios';
import Home from './Home';
import './App.css';

function App() {
  useEffect(() => {
    axios.get('/api/public/hello').then(res => {
      console.log(res.data);
      console.log('asdasdas');
    });
  }, []);
  return (
    <Routes>
      <Route path="/" element={<Home></Home>} />
    </Routes>
  );
};

export default App;