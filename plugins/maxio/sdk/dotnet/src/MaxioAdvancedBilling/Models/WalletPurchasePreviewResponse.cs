using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletPurchasePreviewResponse
{
    [JsonPropertyName("wallet_purchase_previews")]
    public required IReadOnlyList<WalletPurchasePreview> WalletPurchasePreviews { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
