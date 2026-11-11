using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// Only <c>name</c>, <c>description</c>, <c>taxable</c>, <c>tax_code</c>, <c>accounting_code</c>, and <c>item_category</c> are mutable. <c>unit_name</c>, <c>handle</c>, <c>price_point</c>, and <c>price_points</c> are not accepted here: the request still returns <c>200</c>, but those fields are silently ignored and nothing about them changes. <c>handle</c> stays fixed because it's the Wallet Product's only stable identifier on a surface with no product family in the path; <c>unit_name</c> stays fixed because a wallet's behavior snapshots it into the wallet's own <c>unit</c> field and never re-derives it.
/// </summary>
public record UpdateWalletProductRequest
{
    [JsonPropertyName("wallet_product")]
    public required WalletProduct1 WalletProduct { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
