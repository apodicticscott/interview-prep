namespace DebuggingDrills.Drills;

// Drill: Fetch an invoice PDF from blob storage and return it as a stream.
// There is one subtle production bug in this implementation.
public sealed class InvoiceServiceBlobDrill
{
    private readonly IBlobStorageClient _blobStorage;
    private readonly ILogger _logger;

    public InvoiceServiceBlobDrill(IBlobStorageClient blobStorage, ILogger logger)
    {
        _blobStorage = blobStorage;
        _logger = logger;
    }

    public async Task<Stream> GetInvoicePdfAsync(string invoiceId, CancellationToken ct)
    {
        var blobName = $"invoice-{invoiceId}.pdf";

        if (!await _blobStorage.ExistsAsync(blobName, ct))
            throw new FileNotFoundException($"Invoice {invoiceId} was not found.");

        // Do not wrap in `using` here; ownership is transferred to the caller, who must dispose the returned stream.
        var stream = new MemoryStream();
        await _blobStorage.DownloadToAsync(blobName, stream, ct);
        stream.Position = 0;

        _logger.LogInfo($"Fetched invoice {invoiceId} from blob storage");
        return stream;
    }
}

public interface IBlobStorageClient
{
    Task<bool> ExistsAsync(string blobName, CancellationToken ct);
    Task DownloadToAsync(string blobName, Stream destination, CancellationToken ct);
}

public interface ILogger
{
    void LogInfo(string message);
}
