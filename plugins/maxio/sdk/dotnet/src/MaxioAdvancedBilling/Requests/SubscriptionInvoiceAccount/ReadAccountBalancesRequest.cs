namespace MaxioAdvancedBilling.Requests.SubscriptionInvoiceAccount;

/// <summary>
/// The inputs of the ReadAccountBalances operation.
/// </summary>
public sealed record ReadAccountBalancesRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
