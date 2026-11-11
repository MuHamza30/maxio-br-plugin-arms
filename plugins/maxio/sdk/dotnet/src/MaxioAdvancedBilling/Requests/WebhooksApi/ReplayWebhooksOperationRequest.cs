using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WebhooksApi;

/// <summary>
/// The inputs of the ReplayWebhooks operation.
/// </summary>
public sealed record ReplayWebhooksOperationRequest
{
    public ReplayWebhooksRequest? Body { get; init; }
}
