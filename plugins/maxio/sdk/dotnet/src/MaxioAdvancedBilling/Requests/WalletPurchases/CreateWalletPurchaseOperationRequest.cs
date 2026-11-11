using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WalletPurchases;

/// <summary>
/// The inputs of the CreateWalletPurchase operation.
/// </summary>
public sealed record CreateWalletPurchaseOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int WalletId { get; init; }

    public CreateWalletPurchaseRequest? Body { get; init; }
}
