using System.ComponentModel.DataAnnotations;

namespace ApiPractice.Models;

public class CreateShipmentDto
{
    [Required]
    public required string OrderId { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    public required string Carrier { get; set; }
}
