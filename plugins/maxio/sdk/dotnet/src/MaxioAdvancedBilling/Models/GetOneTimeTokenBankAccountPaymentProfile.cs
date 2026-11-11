using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Models;

public record GetOneTimeTokenBankAccountPaymentProfile
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("first_name")]
    [MinLength(1)]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    [MinLength(1)]
    public required string LastName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    /// <summary>
    /// The vault that stores the payment profile with the provided vault_token. Use <c>bogus</c> for testing.
    /// </summary>
    [JsonPropertyName("current_vault")]
    public required BankAccountVault CurrentVault { get; init; }

    [JsonPropertyName("vault_token")]
    [MinLength(1)]
    public required string VaultToken { get; init; }

    [JsonPropertyName("billing_address")]
    [MinLength(1)]
    public required string BillingAddress { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("billing_address_2")]
    public string? BillingAddress2 { get; init; }

    [JsonPropertyName("billing_city")]
    [MinLength(1)]
    public required string BillingCity { get; init; }

    [JsonPropertyName("billing_country")]
    [MinLength(1)]
    public required string BillingCountry { get; init; }

    [JsonPropertyName("billing_state")]
    [MinLength(1)]
    public required string BillingState { get; init; }

    [JsonPropertyName("billing_zip")]
    [MinLength(1)]
    public required string BillingZip { get; init; }

    [JsonPropertyName("bank_name")]
    [MinLength(1)]
    public required string BankName { get; init; }

    [JsonPropertyName("masked_bank_routing_number")]
    [MinLength(1)]
    public required string MaskedBankRoutingNumber { get; init; }

    [JsonPropertyName("masked_bank_account_number")]
    [MinLength(1)]
    public required string MaskedBankAccountNumber { get; init; }

    /// <summary>
    /// Defaults to checking
    /// </summary>
    [JsonPropertyName("bank_account_type")]
    public required BankAccountType BankAccountType { get; init; }

    /// <summary>
    /// Defaults to personal
    /// </summary>
    [JsonPropertyName("bank_account_holder_type")]
    public required BankAccountHolderType BankAccountHolderType { get; init; }

    [JsonPropertyName("payment_type")]
    [MinLength(1)]
    public required string PaymentType { get; init; }

    [JsonPropertyName("disabled")]
    public required bool Disabled { get; init; }

    [JsonPropertyName("site_gateway_setting_id")]
    public required int SiteGatewaySettingId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("customer_vault_token")]
    public string? CustomerVaultToken { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gateway_handle")]
    public string? GatewayHandle { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("verified")]
    public bool? Verified { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
