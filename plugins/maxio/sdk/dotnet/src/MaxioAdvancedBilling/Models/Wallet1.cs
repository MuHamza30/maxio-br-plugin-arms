using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record Wallet1
{
    /// <summary>
    /// The subscription that will own the wallet.
    /// </summary>
    [JsonPropertyName("subscription_id")]
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The <see href="$e/Wallet%20Products/createWalletProduct">Wallet Product</see> that will denominate the wallet: its numeric id, or its handle prefixed with <c>handle:</c> (for example <c>handle:token-pack</c>). A bare handle with no prefix is looked up as an id and returns <c>404</c>. Immutable once the wallet is created.
    /// </summary>
    [JsonPropertyName("wallet_product_id")]
    public required WalletProductId WalletProductId { get; init; }

    /// <summary>
    /// (Optional) A merchant-facing label for the wallet. Surrounding spaces are stripped; a blank value is stored as <c>null</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    [MaxLength(255)]
    public string? Name { get; init; }

    /// <summary>
    /// Defaults to <c>hard_stop</c>, currently the only supported value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("depletion_policy")]
    public string? DepletionPolicy { get; init; }

    /// <summary>
    /// Defaults to <c>discard</c>.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hard_stop_overflow")]
    public WalletHardStopOverflow? HardStopOverflow { get; init; }

    /// <summary>
    /// Pairs the wallet with a merchant-owned metering meter, so its balance factors in live usage (see the wallet resource's <c>meter</c> and <c>metered_through_at</c> fields). Only a <c>sum</c> or <c>count</c> meter can be paired. Requires metered wallet consumption to be enabled for the site, checked whenever this or <c>conversion_rate</c> is sent: <c>403</c> (plain text, not JSON) if it isn't. A meter id that doesn't resolve to a merchant-owned meter on this site, including a system-maintained shadow meter, is <c>404</c>. Immutable once the wallet is created.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("meter_id")]
    public int? MeterId { get; init; }

    /// <summary>
    /// How many wallet units one meter unit is worth, as a decimal string with at most 8 fractional digits, greater than <c>0</c> and below 10^16. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives. Only meaningful with <c>meter_id</c>. Defaults to <c>"1.0"</c>. Immutable once the wallet is created.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("conversion_rate")]
    public ConversionRate? ConversionRate { get; init; }

    /// <summary>
    /// The price point used to price <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see> calls against this wallet. Must belong to the Wallet Product, and can't be archived or a subscription-specific custom price point. Defaults to the Wallet Product's default price point. Immutable once the wallet is created.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_id")]
    public int? PricePointId { get; init; }

    /// <summary>
    /// (Optional) A free opening balance, not a purchase and not reflected on any invoice. When non-zero, wallet creation also writes an initial-balance grant and a matching <c>initial_balance</c> ledger row. Omitted or exactly <c>0</c> creates a zero-balance wallet, not an error. Encoded as a decimal string with at most 8 fractional digits, and must be non-negative and below 10^16. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("initial_balance")]
    public InitialBalance? InitialBalance { get; init; }

    /// <summary>
    /// An expiration for the initial-balance grant. Only accepted together with a non-zero <c>initial_balance</c>, and must be in the future. Accepts either a bare calendar date (<c>YYYY-MM-DD</c>), resolved to the end of that day in the site's time zone, or a full timestamp. A calendar day that doesn't exist, such as <c>2027-02-30</c>, is rejected with <c>422</c> rather than rolled into the next month.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("initial_balance_expires_at")]
    public string? InitialBalanceExpiresAt { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
