using System.Collections.Generic;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record WalletTransactionsListResponse
{
    [JsonPropertyName("wallet_transactions")]
    public required IReadOnlyList<WalletTransaction> WalletTransactions { get; init; }

    [JsonPropertyName("meta")]
    public required ListPublicKeysMeta Meta { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
