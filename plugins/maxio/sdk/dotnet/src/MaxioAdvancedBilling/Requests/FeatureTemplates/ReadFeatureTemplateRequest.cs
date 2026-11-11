namespace MaxioAdvancedBilling.Requests.FeatureTemplates;

/// <summary>
/// The inputs of the ReadFeatureTemplate operation.
/// </summary>
public sealed record ReadFeatureTemplateRequest
{
    /// <summary>
    /// The Advanced Billing id of the feature template.
    /// </summary>
    public required int Id { get; init; }
}
