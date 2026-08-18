namespace ApiPractice.Models;

public class Shipment
{
    public required string Id { get; set; }
    public required string OrderId { get; set; }
    public required string Address { get; set; }
    public required string Carrier { get; set; }
    public required string Status { get; set; }
}
