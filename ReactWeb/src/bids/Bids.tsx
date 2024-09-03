import { useState } from "react";
import ApiStatus from "../apiStatus";
import { useAddBid, useFetchBids } from "../hooks/BidHooks";
import { House } from "../types/house"
import { Bid } from "../types/bid";

type Args = {
    house: House
}

const Bids = ({ house }: Args) => {
    const { data, status, isSuccess } = useFetchBids(house.id);
    const addBidMutation = useAddBid(house.id);

    const emptyBid: Bid = {
        amount: 0,
        bidder: "",
        id: 0,
        houseId: house.id
    };

    const [bid, setBid] = useState<Bid>(emptyBid);

    if(!isSuccess) return <ApiStatus status={status} />;

    const onBidSubmitClick = () => {
        addBidMutation.mutate(bid);
        setBid(emptyBid);
    }
}

export default Bids;