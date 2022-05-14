import React, { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import axios from 'axios';
import Home from './Home';
import './App.css';

function App() {
  useEffect(() => {
    axios.get('/api/home/hello').then(res => {
      console.log(res.data);
      //dsfsdsd
    });
  }, []);
  return (
    <Routes>
      <Route path="/" element={<Home></Home>} />
    </Routes>
  );
};

export default App;