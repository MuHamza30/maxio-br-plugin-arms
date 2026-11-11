using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WalletAdjustments;

/// <summary>
/// The inputs of the CreateWalletAdjustment operation.
/// </summary>
public sealed record CreateWalletAdjustmentOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int WalletId { get; init; }

    public CreateWalletAdjustmentRequest? Body { get; init; }
}
