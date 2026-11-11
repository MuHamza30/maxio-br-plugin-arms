namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the ReadPaymentProfile operation.
/// </summary>
public sealed record ReadPaymentProfileRequest
{
    /// <summary>
    /// The Chargify id of the payment profile
    /// </summary>
    public required int PaymentProfileId { get; init; }
}
