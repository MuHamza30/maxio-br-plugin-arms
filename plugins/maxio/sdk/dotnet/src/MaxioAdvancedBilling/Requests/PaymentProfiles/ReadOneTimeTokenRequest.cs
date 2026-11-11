namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the ReadOneTimeToken operation.
/// </summary>
public sealed record ReadOneTimeTokenRequest
{
    /// <summary>
    /// Advanced Billing Token
    /// </summary>
    public required string ChargifyToken { get; init; }
}
