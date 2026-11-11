using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.SubscriptionStatus;

/// <summary>
/// The inputs of the ResumeSubscription operation.
/// </summary>
public sealed record ResumeSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// (For calendar billing subscriptions only) The way that the resumed subscription's charge should be handled.
    /// </summary>
    public ResumptionCharge CalendarBillingResumptionCharge { get; init; } = ResumptionCharge.Prorated;
}
