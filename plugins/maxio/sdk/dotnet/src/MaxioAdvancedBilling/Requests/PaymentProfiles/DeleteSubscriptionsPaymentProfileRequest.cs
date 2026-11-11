namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the DeleteSubscriptionsPaymentProfile operation.
/// </summary>
public sealed record DeleteSubscriptionsPaymentProfileRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Chargify id of the payment profile
    /// </summary>
    public required int PaymentProfileId { get; init; }
}
