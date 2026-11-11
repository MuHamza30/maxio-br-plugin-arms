using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Wallets;

/// <summary>
/// The inputs of the CreateWallet operation.
/// </summary>
public sealed record CreateWalletOperationRequest
{
    public CreateWalletRequest? Body { get; init; }
}
