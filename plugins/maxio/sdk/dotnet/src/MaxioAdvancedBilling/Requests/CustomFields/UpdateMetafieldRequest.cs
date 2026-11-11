using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.CustomFields;

/// <summary>
/// The inputs of the UpdateMetafield operation.
/// </summary>
public sealed record UpdateMetafieldRequest
{
    /// <summary>
    /// The resource type to which the metafields belong.
    /// </summary>
    public required ResourceType ResourceType { get; init; }

    public UpdateMetafieldsRequest? Body { get; init; }
}
