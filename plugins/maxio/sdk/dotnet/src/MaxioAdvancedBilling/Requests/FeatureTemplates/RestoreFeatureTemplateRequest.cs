namespace MaxioAdvancedBilling.Requests.FeatureTemplates;

/// <summary>
/// The inputs of the RestoreFeatureTemplate operation.
/// </summary>
public sealed record RestoreFeatureTemplateRequest
{
    /// <summary>
    /// The Advanced Billing id of the feature template.
    /// </summary>
    public required int Id { get; init; }
}
