using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

[JsonConverter(typeof(PaymentProfileModelConverter))]
public record PaymentProfileModel
{
    private readonly Optional<GetOneTimeTokenPaymentProfile> _getOneTimeTokenPaymentProfileValue;

    private readonly Optional<GetOneTimeTokenBankAccountPaymentProfile> _getOneTimeTokenBankAccountPaymentProfileValue;

    private PaymentProfileModel(Optional<GetOneTimeTokenPaymentProfile> getOneTimeTokenPaymentProfileValue,
        Optional<GetOneTimeTokenBankAccountPaymentProfile> getOneTimeTokenBankAccountPaymentProfileValue)
    {
        _getOneTimeTokenPaymentProfileValue = getOneTimeTokenPaymentProfileValue;
        _getOneTimeTokenBankAccountPaymentProfileValue = getOneTimeTokenBankAccountPaymentProfileValue;
    }

    public static PaymentProfileModel GetOneTimeTokenPaymentProfile(GetOneTimeTokenPaymentProfile value) =>
        new(Optional<GetOneTimeTokenPaymentProfile>.Some(value), default);

    public static PaymentProfileModel GetOneTimeTokenBankAccountPaymentProfile(GetOneTimeTokenBankAccountPaymentProfile value) =>
        new(default, Optional<GetOneTimeTokenBankAccountPaymentProfile>.Some(value));

    public bool TryGetGetOneTimeTokenPaymentProfile(out GetOneTimeTokenPaymentProfile value) =>
        _getOneTimeTokenPaymentProfileValue.TryGetValue(out value);

    public bool TryGetGetOneTimeTokenBankAccountPaymentProfile(out GetOneTimeTokenBankAccountPaymentProfile value) =>
        _getOneTimeTokenBankAccountPaymentProfileValue.TryGetValue(out value);

    public static implicit operator PaymentProfileModel(GetOneTimeTokenPaymentProfile value) =>
        GetOneTimeTokenPaymentProfile(value);

    public static implicit operator PaymentProfileModel(GetOneTimeTokenBankAccountPaymentProfile value) =>
        GetOneTimeTokenBankAccountPaymentProfile(value);
}

file sealed class PaymentProfileModelConverter : JsonConverter<PaymentProfileModel>
{
    public override PaymentProfileModel Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (!root.TryGetProperty("payment_type", out var typeProperty))
        {
            throw new JsonException("Missing required 'payment_type' discriminator field");
        }
        var discriminator = typeProperty.GetString();
        return discriminator switch
        {
            "credit_card" => PaymentProfileModel.GetOneTimeTokenPaymentProfile(
                root.Deserialize<GetOneTimeTokenPaymentProfile>(options)!),
            "bank_account" => PaymentProfileModel.GetOneTimeTokenBankAccountPaymentProfile(
                root.Deserialize<GetOneTimeTokenBankAccountPaymentProfile>(options)!),
            _ => throw new JsonException(
                $"JSON does not match GetOneTimeTokenPaymentProfile or GetOneTimeTokenBankAccountPaymentProfile schemas: {root.ToString()}")
        };
    }

    public override void Write(Utf8JsonWriter writer, PaymentProfileModel value, JsonSerializerOptions options)
    {
        if (value.TryGetGetOneTimeTokenPaymentProfile(out var getOneTimeTokenPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, getOneTimeTokenPaymentProfileValue, options);
        }
        else if (value.TryGetGetOneTimeTokenBankAccountPaymentProfile(
            out var getOneTimeTokenBankAccountPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, getOneTimeTokenBankAccountPaymentProfileValue, options);
        }
        else
        {
            throw new JsonException($"{nameof(PaymentProfileModel)} contains no valid value to serialize.");
        }
    }
}
