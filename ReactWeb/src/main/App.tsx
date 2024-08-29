import HouseList from '../house/HouseList.tsx';
import './App.css';
import Header from './Header.tsx';

function App() {

  return (
    <div className="container">
            <Header subtitle="BLAH"/>
            <HouseList />
    </div>
  )
}

export default App
