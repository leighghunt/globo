public class BidEntity
{
    public int Id { get; set; }
    public int HouseId { get; set; }
    public HouseEntity? House { get; set; }
    public int Amount { get; set; }
    public string Bidder { get; set; } = string.Empty;
    // public DateTime Date { get; set; }

}
