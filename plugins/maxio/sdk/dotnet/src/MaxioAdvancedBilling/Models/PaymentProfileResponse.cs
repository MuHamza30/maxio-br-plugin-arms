using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.OneOf;

namespace MaxioAdvancedBilling.Models;

public record PaymentProfileResponse
{
    [JsonPropertyName("payment_profile")]
    public required PaymentProfile PaymentProfile { get; init; }

    [JsonExtensionData]
    public AdditionalProperties AdditionalProperties { get; init; } = [];
}
