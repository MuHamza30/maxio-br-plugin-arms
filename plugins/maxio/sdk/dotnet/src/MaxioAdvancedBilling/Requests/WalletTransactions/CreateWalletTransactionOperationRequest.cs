using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WalletTransactions;

/// <summary>
/// The inputs of the CreateWalletTransaction operation.
/// </summary>
public sealed record CreateWalletTransactionOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int WalletId { get; init; }

    public CreateWalletTransactionRequest? Body { get; init; }
}
