using Microsoft.EntityFrameworkCore;

public interface IHouseRepository
{
    Task<List<HouseDto>> GetAll();
    Task<HouseDetailDto?> Get(int houseId);
    Task<HouseDetailDto> Add(HouseDetailDto house);
    Task<HouseDetailDto> Update(HouseDetailDto house);
    Task Delete(int houseId);
}

public class HouseRepository:IHouseRepository
{
    private readonly HouseDbContext context;
    public HouseRepository(HouseDbContext context)
    {
        this.context = context;
    }

    public async Task<List<HouseDto>> GetAll()
    {
        return await context.Houses.Select(h => new HouseDto(h.Id, h.Address, h.Country, h.Price)).ToListAsync();
    }

    public async Task<HouseDetailDto?> Get(int id)
    {
        var e = await context.Houses.SingleOrDefaultAsync(h => h.Id == id);

        if (e == null)
        {
            return null;
        }

        return EntityToDto(e);
    }

    private static HouseDetailDto EntityToDto(HouseEntity e)
    {
        return new HouseDetailDto(e.Id, e.Address, e.Country, e.Price, e.Description, e.Photo);
    }

    private static void DtoToEntity(HouseDetailDto dto, HouseEntity entity)
    {
        entity.Address = dto.Address;
        entity.Country = dto.Country;
        entity.Price = dto.Price;
        entity.Description = dto.Description;
        entity.Photo = dto.Photo;
    }

    public async Task<HouseDetailDto> Add(HouseDetailDto house)
    {
        var e = new HouseEntity();
        DtoToEntity(house, e);

        context.Houses.Add(e);
        await context.SaveChangesAsync();

        return EntityToDto(e);
    }

    public async Task<HouseDetailDto> Update(HouseDetailDto dto)
    {
        // var e = await context.Houses.SingleOrDefaultAsync(h => h.Id == house.Id);
        var entity = await context.Houses.FindAsync(dto.Id);

        if (entity == null)
        {
            throw new ArgumentException($"Error upating house - id {dto.Id} not found");
        }

        DtoToEntity(dto, entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return EntityToDto(entity);
    }

    public async Task Delete(int id)
    {
        var entity = await context.Houses.FindAsync(id);

        if (entity == null)
        {
            throw new ArgumentException($"Error deleting house - id {id} not found");
        }

        context.Houses.Remove(entity);
        await context.SaveChangesAsync();
    }
}
