namespace MaxioAdvancedBilling.Requests.Wallets;

/// <summary>
/// The inputs of the ArchiveWallet operation.
/// </summary>
public sealed record ArchiveWalletRequest
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    public required int Id { get; init; }
}
