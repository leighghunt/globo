import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HouseList from '../house/HouseList.tsx';
import './App.css';
import Header from './Header.tsx';
import HouseDetail from '../house/HouseDetail.tsx';
import HouseAdd from '../house/HouseAdd.tsx';
import HouseEdit from '../house/HouseEdit.tsx';

function App() {

  return (
    <BrowserRouter>
      <div className="container">
        <Header subtitle="BLAH"/>
        <Routes>
          <Route path="/" element={<HouseList />} />
          <Route path="/houses/:id" element={<HouseDetail />} />
          <Route path="/houses/add/" element={<HouseAdd />} />
          <Route path="/houses/edit/:id" element={<HouseEdit />} />
        </Routes> 
      </div>
    </BrowserRouter>
  )
}

export default App