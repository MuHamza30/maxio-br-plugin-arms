using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WebhooksApi;

/// <summary>
/// The inputs of the CreateEndpoint operation.
/// </summary>
public sealed record CreateEndpointRequest
{
    public CreateOrUpdateEndpointRequest? Body { get; init; }
}
