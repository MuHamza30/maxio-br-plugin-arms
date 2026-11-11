namespace MaxioAdvancedBilling.Requests.ApiExports;

/// <summary>
/// The inputs of the ReadInvoicesExport operation.
/// </summary>
public sealed record ReadInvoicesExportRequest
{
    /// <summary>
    /// Id of a Batch Job.
    /// </summary>
    public required string BatchId { get; init; }
}
