using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.OneOf;

namespace MaxioAdvancedBilling.Models;

public record GetOneTimeTokenRequest
{
    [JsonPropertyName("payment_profile")]
    public required PaymentProfileModel PaymentProfile { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
