import {useState } from 'react';
import config from "../config";
import { House } from "../types/house";

const useFetchHouses = (): House[] => {
    const [houses, setHouses] = useState<House[]>([]);

    const fetchHouses = async () => {
        const res = await fetch(`${config.baseApiUrl}/houses`);
        const data = await res.json();
        setHouses(data);
    }

    fetchHouses();

    return houses;
}

export default useFetchHouses;