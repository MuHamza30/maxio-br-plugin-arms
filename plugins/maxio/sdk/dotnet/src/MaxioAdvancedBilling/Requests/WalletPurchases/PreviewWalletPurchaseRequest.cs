using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WalletPurchases;

/// <summary>
/// The inputs of the PreviewWalletPurchase operation.
/// </summary>
public sealed record PreviewWalletPurchaseRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int WalletId { get; init; }

    public CreateWalletPurchaseRequest? Body { get; init; }
}
