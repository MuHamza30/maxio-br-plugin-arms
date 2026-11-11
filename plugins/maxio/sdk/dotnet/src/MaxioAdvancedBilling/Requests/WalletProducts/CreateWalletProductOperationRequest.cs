using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.WalletProducts;

/// <summary>
/// The inputs of the CreateWalletProduct operation.
/// </summary>
public sealed record CreateWalletProductOperationRequest
{
    public CreateWalletProductRequest? Body { get; init; }
}
