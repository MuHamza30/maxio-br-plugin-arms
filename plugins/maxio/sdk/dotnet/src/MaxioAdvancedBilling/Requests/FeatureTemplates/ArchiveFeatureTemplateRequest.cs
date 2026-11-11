namespace MaxioAdvancedBilling.Requests.FeatureTemplates;

/// <summary>
/// The inputs of the ArchiveFeatureTemplate operation.
/// </summary>
public sealed record ArchiveFeatureTemplateRequest
{
    /// <summary>
    /// The Advanced Billing id of the feature template.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// When <c>true</c>, also destroys every feature catalog item created from this template and cascades to their entitlements, revoking subscriber access immediately. When <c>false</c> (default), the feature template and its feature catalog items are archived, and existing entitlements are preserved.
    /// </summary>
    public bool RemoveFromCatalog { get; init; } = false;
}
