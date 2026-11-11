namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the ChangeSubscriptionGroupDefaultPaymentProfile operation.
/// </summary>
public sealed record ChangeSubscriptionGroupDefaultPaymentProfileRequest
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
