namespace MaxioAdvancedBilling.Requests.ReasonCodes;

/// <summary>
/// The inputs of the ReadReasonCode operation.
/// </summary>
public sealed record ReadReasonCodeRequest
{
    /// <summary>
    /// The Advanced Billing id of the reason code
    /// </summary>
    public required int ReasonCodeId { get; init; }
}
