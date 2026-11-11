using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionInvoiceAccount;

/// <summary>
/// The inputs of the RefundPrepayment operation.
/// </summary>
public sealed record RefundPrepaymentOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// id of prepayment
    /// </summary>
    public required long PrepaymentId { get; init; }

    public RefundPrepaymentRequest? Body { get; init; }
}
