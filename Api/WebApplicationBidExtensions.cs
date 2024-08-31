using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationBidExtensions
{
    public static void MapBidEndpoints(this WebApplication app)
    {
        app.MapGet("houses/{houseId:int}/bids", async (IBidRepository bidRepository, IHouseRepository houseRepository, int houseId) => {

            if(await houseRepository.Get(houseId) == null)
            {
                return Results.Problem($"House with ID {houseId} not found", statusCode: StatusCodes.Status404NotFound);
            }
            var bids = await bidRepository.GetBidsForHouse(houseId);

            return Results.Ok(bids);
        }).Produces<List<BidDto>>(StatusCodes.Status200OK);

        app.MapPost("houses/{houseId:int}/bids", async (IBidRepository bidRepository, IHouseRepository houseRepository, int houseId, [FromBody]BidDto bid) => {

            if(bid.HouseId != houseId)
            {
                return Results.Problem($"House ID in URL ({houseId}) does not match House ID in body ({bid.HouseId})", statusCode: StatusCodes.Status400BadRequest);
            }

            if(await houseRepository.Get(houseId) == null)
            {
                return Results.Problem($"House with ID {houseId} not found", statusCode: 404);
            }

            if(!MiniValidator.TryValidate(bid, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var newBid = await bidRepository.Add(bid);

            return Results.Created($"/houses/{newBid.HouseId}/bids/{newBid.Id}", newBid);
        }).Produces<BidDto>(StatusCodes.Status201Created).ProducesProblem(400).ProducesValidationProblem();

    }
}