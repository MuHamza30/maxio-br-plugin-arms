using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionGroupInvoiceAccount;

/// <summary>
/// The inputs of the CreateSubscriptionGroupPrepayment operation.
/// </summary>
public sealed record CreateSubscriptionGroupPrepaymentRequest
{
    /// <summary>
    /// The uid of the subscription group
    /// </summary>
    public required string Uid { get; init; }

    public SubscriptionGroupPrepaymentRequest? Body { get; init; }
}
