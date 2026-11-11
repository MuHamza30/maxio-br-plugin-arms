using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// A completed unit purchase.
/// </summary>
public record WalletPurchase
{
    /// <summary>
    /// The id of the grant this purchase created. When the wallet was overdrawn by at least the purchased quantity, the units repay the debt and the grant is still created, with <c>remaining</c> of <c>0</c> and <c>original</c> equal to the purchased quantity.
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The price point the purchase was priced at.
    /// </summary>
    [JsonPropertyName("price_point_id")]
    public required int PricePointId { get; init; }

    /// <summary>
    /// The uid of the invoice created to collect payment for this purchase.
    /// </summary>
    [JsonPropertyName("invoice_uid")]
    public required string InvoiceUid { get; init; }

    /// <summary>
    /// The number of units purchased, as a decimal string.
    /// </summary>
    [JsonPropertyName("quantity")]
    public required string Quantity { get; init; }

    /// <summary>
    /// When the purchased units expire, following the funding entitlement's grant expiration policy. <c>null</c> when the wallet has no funding entitlement or its entitlements disagree on an expiration policy.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public required DateTimeOffset? ExpiresAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
