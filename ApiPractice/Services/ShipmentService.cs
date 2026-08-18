using ApiPractice.Models;
using ApiPractice.Services.Interfaces;

namespace ApiPractice.Services;

public class ShipmentService : IShipmentService
{
    private readonly IShippingProviderClient _shippingProviderClient;
    private readonly ILogger<ShipmentService> _logger;
    private readonly List<Shipment> _shipments = [];

    public ShipmentService(IShippingProviderClient shippingProviderClient, ILogger<ShipmentService> logger)
    {
        _shippingProviderClient = shippingProviderClient;
        _logger = logger;
    }

    public Task<IEnumerable<Shipment>> GetAllShipmentsAsync(CancellationToken ct)
        => Task.FromResult<IEnumerable<Shipment>>(_shipments);

    public Task<Shipment?> GetShipmentByIdAsync(string id, CancellationToken ct)
        => Task.FromResult(_shipments.FirstOrDefault(s => s.Id == id));

    public async Task<Shipment> AddShipmentAsync(CreateShipmentDto dto, CancellationToken ct)
    {
        if (_shipments.Any(s => s.OrderId == dto.OrderId))
            throw new InvalidOperationException($"Shipment already exists for order '{dto.OrderId}'.");

        ProviderShipmentResult providerResult;
        try
        {
            providerResult = await _shippingProviderClient.CreateShipmentAsync(dto, ct);
        }
        catch (TimeoutException ex)
        {
            _logger.LogWarning(ex, "Timeout from provider while creating shipment for order {OrderId}", dto.OrderId);
            throw new ShippingProviderUnavailableException("Shipping provider timed out.", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Provider request failed while creating shipment for order {OrderId}", dto.OrderId);
            throw new ShippingProviderUnavailableException("Shipping provider is unavailable.", ex);
        }

        var shipment = new Shipment
        {
            Id = providerResult.ShipmentId,
            OrderId = dto.OrderId,
            Address = dto.Address,
            Carrier = dto.Carrier,
            Status = providerResult.Status
        };

        _shipments.Add(shipment);
        _logger.LogInformation("Created shipment {ShipmentId} for order {OrderId}", shipment.Id, shipment.OrderId);

        return shipment;
    }
}
