import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import axios, { AxiosError, AxiosResponse } from "axios";
import { useNavigate } from "react-router-dom";
import { Bid } from "../types/bid";
import config from "../config";
import Problem from "../types/problem";

const useFetchBids = (houseId: number) => {

    return useQuery<Bid[], AxiosError>({
        queryKey: ['bids', houseId],
        queryFn: () =>
            axios.get(`${config.baseApiUrl}/houses/${houseId}/bids`)
        .then((res) => res.data),
    });
}

const useAddBid = (houseId: number) => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Bid, null>({
        mutationFn: (bid: Bid) =>
            axios.post(`${config.baseApiUrl}/houses/${houseId}/bids`, bid),
        onSuccess: (resp, bid) => {
            queryClient.invalidateQueries({ queryKey: ['bids'] });
            nav('/');
        },
    });
}

export { useFetchBids, useAddBid };