using Microsoft.EntityFrameworkCore;

public interface IBidRepository
{
    Task<List<BidDto>> GetBidsForHouse(int houseId);
    Task<BidDto> Add(BidDto bid);
}

public class BidRepository:IBidRepository
{
    private readonly HouseDbContext context;
    public BidRepository(HouseDbContext context)
    {
        this.context = context;
    }

    public async Task<List<BidDto>> GetBidsForHouse(int houseId)
    {
        return await context.Bids.Where(b => b.HouseId == houseId).Select(b => new BidDto(b.Id, b.Bidder, b.Amount, b.HouseId)).ToListAsync();
    }

    public async Task<BidDto> Add(BidDto bid)
    {
        var e = new BidEntity();
        DtoToEntity(bid, e);

        context.Bids.Add(e);
        await context.SaveChangesAsync();

        return EntityToDto(e);
    }

    private static BidDto EntityToDto(BidEntity e)
    {
        return new BidDto(e.Id, e.Bidder, e.Amount, e.HouseId);
    }

    private static void DtoToEntity(BidDto dto, BidEntity entity)
    {
        entity.Bidder = dto.Bidder;
        entity.Amount = dto.Amount;
        entity.HouseId = dto.HouseId;
        // entity.HouseId = dto.Id;
    }
}