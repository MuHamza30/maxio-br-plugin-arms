using System;

namespace MaxioAdvancedBilling.Requests.Insights;

/// <summary>
/// The inputs of the ReadMrr operation.
/// </summary>
public sealed record ReadMrrRequest
{
    /// <summary>
    /// submit a timestamp in ISO8601 format to request MRR for a historic time.
    /// </summary>
    public DateTimeOffset? AtTime { get; init; }

    /// <summary>
    /// submit the id of a subscription in order to limit results.
    /// </summary>
    public int? SubscriptionId { get; init; }
}
