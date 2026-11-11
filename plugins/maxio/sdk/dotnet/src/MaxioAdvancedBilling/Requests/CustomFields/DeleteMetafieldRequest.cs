using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.CustomFields;

/// <summary>
/// The inputs of the DeleteMetafield operation.
/// </summary>
public sealed record DeleteMetafieldRequest
{
    /// <summary>
    /// The resource type to which the metafields belong.
    /// </summary>
    public required ResourceType ResourceType { get; init; }

    /// <summary>
    /// The name of the metafield to be deleted
    /// </summary>
    public string? Name { get; init; }
}
