//- Följande information skall kunna registreras och sparas i databasen för en annons, titel,
//beskrivning, pris och kategori (typ av annons). 


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

    public  required User Seller { get; set; }
    public required Category Category { get; set; }

    public string GetSummary() => $"{Title} - {Price:C}";
}
