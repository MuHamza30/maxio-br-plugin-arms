namespace MaxioAdvancedBilling.Requests.ProductFeatures;

/// <summary>
/// The inputs of the RemoveProductFeature operation.
/// </summary>
public sealed record RemoveProductFeatureRequest
{
    /// <summary>
    /// The Advanced Billing id of the product.
    /// </summary>
    public required int ProductId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the feature catalog item.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// When <c>true</c>, permanently deletes this feature catalog item and every entitlement it created, revoking subscriber access immediately. When <c>false</c> (default), the feature catalog item is archived and existing entitlements are preserved.
    /// </summary>
    public bool DestroyEntitlements { get; init; } = false;
}
