namespace MaxioAdvancedBilling.Requests.ComponentFeatures;

/// <summary>
/// The inputs of the ReadComponentFeature operation.
/// </summary>
public sealed record ReadComponentFeatureRequest
{
    /// <summary>
    /// The Advanced Billing id of the component.
    /// </summary>
    public required int ComponentId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the feature catalog item.
    /// </summary>
    public required int Id { get; init; }
}
