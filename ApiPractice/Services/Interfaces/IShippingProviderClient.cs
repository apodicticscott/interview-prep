using ApiPractice.Models;

namespace ApiPractice.Services.Interfaces;

public interface IShippingProviderClient
{
    Task<ProviderShipmentResult> CreateShipmentAsync(CreateShipmentDto dto, CancellationToken ct);
}
