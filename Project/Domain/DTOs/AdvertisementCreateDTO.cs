
namespace BlocketChallenge.Project.Domain.DTOs;
public class AdvertisementCreateDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
}