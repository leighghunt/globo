using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<HouseDbContext>(options =>
{
    options.UseQueryTrackingBehavior(Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking);
});
builder.Services.AddScoped<IHouseRepository, HouseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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

    var newHouse = await repository.Add(house);
    
    return Results.Created($"/house/{newHouse.Id}", newHouse);
}).Produces<HouseDto>(StatusCodes.Status201Created);

// app.MapPut("/houses/{houseId:int}", async (IHouseRepository repository, int houseId, [FromBody]HouseDetailDto house) => {
app.MapPut("/houses", async (IHouseRepository repository, [FromBody]HouseDetailDto house) => {
    // var e = await repository.Get(house.Id);

    if(await repository.Get(house.Id) == null)
    {
        return Results.Problem($"House with ID {house.Id} not found", statusCode: 404);
    }

    var updatedHouse = await repository.Update(house);

    return Results.Ok(updatedHouse);
}).Produces<HouseDto>(StatusCodes.Status200OK).ProducesProblem(404);

app.MapDelete("houses/{houseId:int}", async (IHouseRepository repository, int houseId) => {
    var e = await repository.Get(houseId);

    if(e == null)
    {
        return Results.Problem($"House with ID {houseId} not found", statusCode: 404);
    }

    await repository.Delete(houseId);

    return Results.NoContent();
}).Produces(StatusCodes.Status204NoContent).ProducesProblem(404);

app.Run();
