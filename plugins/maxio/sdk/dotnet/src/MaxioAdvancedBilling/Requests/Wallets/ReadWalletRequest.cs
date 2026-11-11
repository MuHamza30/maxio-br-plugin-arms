namespace MaxioAdvancedBilling.Requests.Wallets;

/// <summary>
/// The inputs of the ReadWallet operation.
/// </summary>
public sealed record ReadWalletRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int Id { get; init; }
}
