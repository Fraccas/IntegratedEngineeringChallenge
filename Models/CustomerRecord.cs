namespace CsvParserChallenge.Models;

public class CustomerRecord
{
    public int CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? CarMake { get; set; }
    public string? CarModel { get; set; }
    public int CarYear { get; set; }
    public string? LicensePlate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}
