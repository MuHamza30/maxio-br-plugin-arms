using System.Collections.Generic;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the BulkRecordEvents operation.
/// </summary>
public sealed record BulkRecordEventsRequest
{
    /// <summary>
    /// Identifies the Stream for which the events should be published.
    /// </summary>
    public required string ApiHandle { get; init; }

    /// <summary>
    /// If you've attached your own Keen project as an Advanced Billing event data-store, use this parameter to indicate the data-store. This applies to Legacy Metering sites only — it has no effect on Maxio Metering sites.
    /// </summary>
    public string? StoreUid { get; init; }

    public IReadOnlyList<EbbEvent>? Body { get; init; }
}
