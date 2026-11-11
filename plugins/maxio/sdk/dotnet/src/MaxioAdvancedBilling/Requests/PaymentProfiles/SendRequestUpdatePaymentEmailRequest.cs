namespace MaxioAdvancedBilling.Requests.PaymentProfiles;

/// <summary>
/// The inputs of the SendRequestUpdatePaymentEmail operation.
/// </summary>
public sealed record SendRequestUpdatePaymentEmailRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
