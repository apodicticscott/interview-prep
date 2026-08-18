using ApiPractice.Models;

namespace ApiPractice.Services.Interfaces;

public interface IShipmentService
{
    Task<IEnumerable<Shipment>> GetAllShipmentsAsync(CancellationToken ct);
    Task<Shipment?> GetShipmentByIdAsync(string id, CancellationToken ct);
    Task<Shipment> AddShipmentAsync(CreateShipmentDto dto, CancellationToken ct);
}
