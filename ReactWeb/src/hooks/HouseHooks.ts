import config from "../config";
import { House } from "../types/house";
import axios, { AxiosError } from 'axios';
import { useQuery } from '@tanstack/react-query';

const useFetchHouses = () => {
    return useQuery<House[], AxiosError>({
        queryKey: ['houses'],
        queryFn: () =>
            axios.get(`${config.baseApiUrl}/houses`)
        .then((res) => res.data),
    });
}

export default useFetchHouses;