using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WebhooksApi;

/// <summary>
/// The inputs of the UpdateEndpoint operation.
/// </summary>
public sealed record UpdateEndpointRequest
{
    /// <summary>
    /// The Advanced Billing id for the endpoint that should be updated
    /// </summary>
    public required int EndpointId { get; init; }

    public CreateOrUpdateEndpointRequest? Body { get; init; }
}
