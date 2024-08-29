type Args = {
    status: "success" | "error" | "pending";
}

const ApiStatus = ({ status }: Args) => {
    switch(status){
        case "pending":
            return <h2>Loading...</h2>;
        case "error":
            return <h2>Something went wrong</h2>;
        default:
            throw Error("Unknown API State");
    }
}

export default ApiStatus;