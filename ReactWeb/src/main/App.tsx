import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HouseList from '../house/HouseList.tsx';
import './App.css';
import Header from './Header.tsx';
import HouseDetail from '../house/HouseDetail.tsx';

function App() {

  return (
    <BrowserRouter>
      <div className="container">
        <Header subtitle="BLAH"/>
        <Routes>
          <Route path="/" element={<HouseList />} />
          <Route path="/house/:id" element={<HouseDetail />} />
        </Routes> 
      </div>
    </BrowserRouter>
  )
}

export default App