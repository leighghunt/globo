using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationHouseExtensions
{
    public static void MapHouseEndpoints(this WebApplication app)
    {
        app.MapGet("/houses", (IHouseRepository repository) => repository.GetAll()).Produces<List<HouseDto>>(StatusCodes.Status200OK);
        app.MapGet("/house/{houseId:int}", async (IHouseRepository repository, int houseId) => {
            var house = await repository.Get(houseId);

            if(house == null)
            {
                return Results.Problem($"House with ID {houseId} not found", statusCode: 404);
            }

            return Results.Ok(house);
        }).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);

        app.MapPost("/houses", async (IHouseRepository repository, [FromBody]HouseDetailDto house) => {

            if(!MiniValidator.TryValidate(house, out var errors))
            {
                return Results.ValidationProblem(errors);
            }
            var newHouse = await repository.Add(house);
            
            return Results.Created($"/house/{newHouse.Id}", newHouse);
        }).Produces<HouseDto>(StatusCodes.Status201Created).ProducesValidationProblem();

        // app.MapPut("/houses/{houseId:int}", async (IHouseRepository repository, int houseId, [FromBody]HouseDetailDto house) => {
        app.MapPut("/houses", async (IHouseRepository repository, [FromBody]HouseDetailDto house) => {
            // var e = await repository.Get(house.Id);

            if(!MiniValidator.TryValidate(house, out var errors))
            {
                return Results.ValidationProblem(errors);
            }
            if(await repository.Get(house.Id) == null)
            {
                return Results.Problem($"House with ID {house.Id} not found", statusCode: 404);
            }

            var updatedHouse = await repository.Update(house);

            return Results.Ok(updatedHouse);
        }).Produces<HouseDto>(StatusCodes.Status200OK).ProducesProblem(404).ProducesValidationProblem();

        app.MapDelete("houses/{houseId:int}", async (IHouseRepository repository, int houseId) => {
            var e = await repository.Get(houseId);

            if(e == null)
            {
                return Results.Problem($"House with ID {houseId} not found", statusCode: 404);
            }

            await repository.Delete(houseId);

            return Results.NoContent();
        }).Produces(StatusCodes.Status204NoContent).ProducesProblem(404);
    }
}