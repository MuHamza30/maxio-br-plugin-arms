using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionStatus;

/// <summary>
/// The inputs of the PreviewRenewal operation.
/// </summary>
public sealed record PreviewRenewalRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public RenewalPreviewRequest? Body { get; init; }
}
