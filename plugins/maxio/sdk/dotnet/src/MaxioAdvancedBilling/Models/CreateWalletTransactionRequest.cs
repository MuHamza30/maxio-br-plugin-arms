using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

public record CreateWalletTransactionRequest
{
    [JsonPropertyName("wallet_transaction")]
    public required WalletTransaction1 WalletTransaction { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
