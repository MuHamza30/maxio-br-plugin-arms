using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionInvoiceAccount;

/// <summary>
/// The inputs of the CreatePrepayment operation.
/// </summary>
public sealed record CreatePrepaymentOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public CreatePrepaymentRequest? Body { get; init; }
}
