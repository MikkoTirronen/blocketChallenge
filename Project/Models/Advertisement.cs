
namespace BlocketChallenge.Models;

public class Advertisement
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int SellerId { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public User? Seller { get; set; }
    public Category? Category { get; set; }

    public string GetSummary() => $"{Title} - {Price:C}";
}
