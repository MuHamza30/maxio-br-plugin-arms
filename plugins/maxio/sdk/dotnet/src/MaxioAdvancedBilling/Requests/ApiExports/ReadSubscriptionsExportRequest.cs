namespace MaxioAdvancedBilling.Requests.ApiExports;

/// <summary>
/// The inputs of the ReadSubscriptionsExport operation.
/// </summary>
public sealed record ReadSubscriptionsExportRequest
{
    /// <summary>
    /// Id of a Batch Job.
    /// </summary>
    public required string BatchId { get; init; }
}
