using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models;

/// <summary>
/// One active wallet drawing on this Wallet Product.
/// </summary>
public record WalletProductActiveWallet
{
    /// <summary>
    /// The Advanced Billing id of the wallet.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("subscription_id")]
    public int? SubscriptionId { get; init; }

    /// <summary>
    /// The owning customer's organization name, falling back to their full name, then to <c>"Customer #&lt;id&gt;"</c>. <c>"Customer"</c> (with no id) when the wallet's subscription has no customer at all.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("company_name")]
    public string? CompanyName { get; init; }

    /// <summary>
    /// See the same field on <see href="$m/Wallet%20Product%20Usage%20Template">Wallet Product Usage Template</see>. The same caveats apply.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("purchase_mechanism")]
    public string? PurchaseMechanism { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price_point_name")]
    public string? PricePointName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>
    /// <c>false</c> when the wallet's Feature is archived.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("active")]
    public bool? Active { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
