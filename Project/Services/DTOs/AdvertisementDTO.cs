namespace BlocketChallenge.Services.DTOs;

public class AdvertisementDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string SellerName { get; set; } = null!;

    public int? SellerId { get; set; }
    public string CategoryName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; } // optional for frontend
}

