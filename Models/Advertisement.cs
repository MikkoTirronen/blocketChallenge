//- Följande information skall kunna registreras och sparas i databasen för en annons, titel,
//beskrivning, pris och kategori (typ av annons). 


namespace BlocketChallenge.Models;

public class Advertisement
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public decimal Price { get; set; }
    public int SellerId { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }

    public string SellerUsername { get; set; }
    public string CategoryName { get; set; }

    public string GetSummary() => $"{Title} - {Price:C}";
}
