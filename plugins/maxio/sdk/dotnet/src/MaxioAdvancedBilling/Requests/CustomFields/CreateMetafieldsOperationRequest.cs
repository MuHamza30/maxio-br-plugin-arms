using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.CustomFields;

/// <summary>
/// The inputs of the CreateMetafields operation.
/// </summary>
public sealed record CreateMetafieldsOperationRequest
{
    /// <summary>
    /// The resource type to which the metafields belong.
    /// </summary>
    public required ResourceType ResourceType { get; init; }

    public CreateMetafieldsRequest? Body { get; init; }
}
