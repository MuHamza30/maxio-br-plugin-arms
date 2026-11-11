using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Wallets;

/// <summary>
/// The inputs of the UpdateWallet operation.
/// </summary>
public sealed record UpdateWalletOperationRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int Id { get; init; }

    public UpdateWalletRequest? Body { get; init; }
}
