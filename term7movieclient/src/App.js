import React, { useEffect } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import axios from 'axios';
import Home from './Home';
import './App.css';

function App() {
  useEffect(() => {
    axios.get('/api/home/hello').then(res => {
      console.log(res.data);
      console.log('asdasdas');
    });
  }, []);
  return (
    <BrowserRouter basename='/Term7Movie'>
      <Routes>
        <Route path="/" element={<Home></Home>} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;