import config from "../config";
import { House } from "../types/house";
import axios, { AxiosError, AxiosResponse } from 'axios';
import { QueryClient, useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from "react-router-dom";
import Problem from "../types/problem";

const useFetchHouses = () => {
    return useQuery<House[], AxiosError>({
        queryKey: ['houses'],
        queryFn: () =>
            axios.get(`${config.baseApiUrl}/houses`)
        .then((res) => res.data),
    });
}

const useFetchHouse = (id: number) => {
    return useQuery<House, AxiosError>({
        queryKey: ['houses', id],
        queryFn: () =>
            axios.get(`${config.baseApiUrl}/houses/${id}`)
        .then((res) => res.data),
    });
}

const useAddHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, House, null>({
        mutationFn: (house: House) =>
            axios.post(`${config.baseApiUrl}/houses`, house),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['houses'] });
            nav('/');
        },
    });
}

const useUpdateHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, House, null>({
        mutationFn: (house: House) =>
            axios.put(`${config.baseApiUrl}/houses`, house),
        onSuccess: (_, house) => {
            queryClient.invalidateQueries({ queryKey: ['houses'] });
            nav(`/houses/${house.id}`);
        },
    });
}

const useDeleteHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, House, null>({
        mutationFn: (house: House) =>
            axios.delete(`${config.baseApiUrl}/houses/${house.id}`),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['houses'] });
            nav("/");
        },
        // .then((res) => res.data),
    });
}
export default useFetchHouses;
export { useFetchHouse, useAddHouse, useUpdateHouse, useDeleteHouse };