namespace MaxioAdvancedBilling.Requests.Invoices;

/// <summary>
/// The inputs of the ReadCreditNote operation.
/// </summary>
public sealed record ReadCreditNoteRequest
{
    /// <summary>
    /// The unique identifier of the credit note
    /// </summary>
    public required string Uid { get; init; }
}
