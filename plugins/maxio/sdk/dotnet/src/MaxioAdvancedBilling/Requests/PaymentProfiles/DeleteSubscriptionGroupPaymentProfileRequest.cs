namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the DeleteSubscriptionGroupPaymentProfile operation.
/// </summary>
public sealed record DeleteSubscriptionGroupPaymentProfileRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    /// <summary>
    /// The Chargify id of the payment profile
    /// </summary>
    public required int PaymentProfileId { get; init; }
}
