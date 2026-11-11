namespace MaxioAdvancedBilling.Requests.WalletProducts;

/// <summary>
/// The inputs of the ReadWalletProductUsage operation.
/// </summary>
public sealed record ReadWalletProductUsageRequest
{
    /// <summary>
    /// The Advanced Billing id of the Wallet Product, or its handle prefixed with <c>handle:</c> (for example <c>handle:token-pack</c>). A bare handle returns <c>404</c>.
    /// </summary>
    public required string Id { get; init; }
}
