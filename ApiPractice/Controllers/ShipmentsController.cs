using Microsoft.AspNetCore.Mvc;
using ApiPractice.Models;
using ApiPractice.Services;
using ApiPractice.Services.Interfaces;

namespace ApiPractice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    private readonly IShipmentService _shipmentService = shipmentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipment>>> GetAllShipments(CancellationToken ct)
    {
        var shipments = await _shipmentService.GetAllShipmentsAsync(ct);
        return Ok(shipments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shipment>> GetShipmentById(string id, CancellationToken ct)
    {
        var shipment = await _shipmentService.GetShipmentByIdAsync(id, ct);
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpPost]
    public async Task<ActionResult<Shipment>> CreateShipment([FromBody] CreateShipmentDto dto, CancellationToken ct)
    {
        try
        {
            var shipment = await _shipmentService.AddShipmentAsync(dto, ct);
            return CreatedAtAction(nameof(GetShipmentById), new { id = shipment.Id }, shipment);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ShippingProviderUnavailableException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message });
        }
    }
}
