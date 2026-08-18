using ApiPractice.Models;
using ApiPractice.Services.Interfaces;

namespace ApiPractice.Services;

public class ShippingProviderClient : IShippingProviderClient
{
    private static readonly HashSet<string> SupportedCarriers = new(StringComparer.OrdinalIgnoreCase)
    {
        "UPS",
        "FedEx",
        "USPS"
    };

    public async Task<ProviderShipmentResult> CreateShipmentAsync(CreateShipmentDto dto, CancellationToken ct)
    {
        // Simulate outbound latency to a third-party API.
        await Task.Delay(150, ct);

        if (string.Equals(dto.Carrier, "timeout", StringComparison.OrdinalIgnoreCase))
            throw new TimeoutException("Shipping provider timed out.");

        if (!SupportedCarriers.Contains(dto.Carrier))
            throw new HttpRequestException($"Carrier '{dto.Carrier}' is currently unavailable.");

        return new ProviderShipmentResult
        {
            Provider = dto.Carrier,
            ShipmentId = Guid.NewGuid().ToString("N"),
            TrackingUrl = $"https://tracking.example.com/{dto.Carrier}/{Guid.NewGuid():N}",
            Status = "Created"
        };
    }
}
