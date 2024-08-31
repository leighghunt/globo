// import { Link } from "react-router-dom";
// import { useNavigate } from "react-router-dom";
import { Link, useParams } from "react-router-dom";
import ApiStatus from "../apiStatus";
import { currencyFormatter } from "../config";
import { useDeleteHouse, useFetchHouse} from "../hooks/HouseHooks";
// import { House } from "../types/house";
import defaultImage from "./defaultPhoto";

const HouseDetail = () => {
  const { id } = useParams();
  if(!id) { throw new Error('No house id provided'); }
  const houseId = parseInt(id);

  const { data, status, isSuccess } = useFetchHouse(houseId);
  const deleteHouseMutation = useDeleteHouse();

  if(!isSuccess) {
      return <ApiStatus status={status} />;
  }

  if(!data){
    return <div>House not found</div>
  }
  
  return (
    <div className="row">
      <div className="col-6">
        <div className="row">
          <img
            className="img-fluid"
            src={data.photo ? data.photo : defaultImage}
            alt="House pic"
          />
        </div>
        <div className="row mt-3">
          <div className="col-2">
            <Link
              className="btn btn-primary w-100"
              to={`/houses/edit/${data.id}`}
            >
              Edit
            </Link>
          </div>
          <div className="col-2">
            <button
              className="btn btn-danger w-100"
              onClick={() => {
                if (window.confirm("Are you sure?"))
                  deleteHouseMutation.mutate(data);
              }}
            >
              Delete
            </button>
          </div>
        </div>
      </div>
      <div className="col-6">
        <div className="row mt-2">
          <h5 className="col-12">{data.country}</h5>
        </div>
        <div className="row">
          <h3 className="col-12">{data.address}</h3>
        </div>
        <div className="row">
          <h2 className="themeFontColor col-12">
            {currencyFormatter.format(data.price)}
          </h2>
        </div>
        <div className="row">
          <div className="col-12 mt-3">{data.description}</div>
        </div>
        {/* <Bids house={data} /> */}
      </div>
    </div>
);
}

// const HouseDetail = (houseId: number) => {

    
//     return (
//         <div>
//           <div className="row mb-2">
//             <h5 className="themeFontColor text-center">
//               Houses currently on the market
//             </h5>
//           </div>
//           <table className="table table-hover">
//             <thead>
//               <tr>
//                 <th>Address</th>
//                 <th>Country</th>
//                 <th>Asking Price</th>
//               </tr>
//             </thead>
//             <tbody>
//               {
//                 data && data.map((h: House) => (
//                   <tr key={h.id}>
//                     <td>{h.address}</td>
//                     <td>{h.country}</td>
//                     <td>{currencyFormatter.format(h.price)}</td>
//                   </tr>
//                 ))}
//             </tbody>
//           </table>
//         </div>
//       );
//     }

export default HouseDetail;

