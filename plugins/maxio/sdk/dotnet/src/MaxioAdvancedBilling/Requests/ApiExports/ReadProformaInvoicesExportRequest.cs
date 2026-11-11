namespace MaxioAdvancedBilling.Requests.ApiExports;

/// <summary>
/// The inputs of the ReadProformaInvoicesExport operation.
/// </summary>
public sealed record ReadProformaInvoicesExportRequest
{
    /// <summary>
    /// Id of a Batch Job.
    /// </summary>
    public required string BatchId { get; init; }
}
