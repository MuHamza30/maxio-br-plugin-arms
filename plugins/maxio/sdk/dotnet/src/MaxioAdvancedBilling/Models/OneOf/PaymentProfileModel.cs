using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

[JsonConverter(typeof(PaymentProfileModelConverter))]
public record PaymentProfileModel
{
    private readonly Optional<GetOneTimeTokenPaymentProfile> _getOneTimeTokenPaymentProfileValue;

    private readonly Optional<GetOneTimeTokenBankAccountPaymentProfile> _getOneTimeTokenBankAccountPaymentProfileValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private PaymentProfileModel(Optional<GetOneTimeTokenPaymentProfile> getOneTimeTokenPaymentProfileValue,
        Optional<GetOneTimeTokenBankAccountPaymentProfile> getOneTimeTokenBankAccountPaymentProfileValue,
        Optional<UnknownArm> unknownValue)
    {
        _getOneTimeTokenPaymentProfileValue = getOneTimeTokenPaymentProfileValue;
        _getOneTimeTokenBankAccountPaymentProfileValue = getOneTimeTokenBankAccountPaymentProfileValue;
        _unknownValue = unknownValue;
    }

    public static PaymentProfileModel GetOneTimeTokenPaymentProfile(GetOneTimeTokenPaymentProfile value) =>
        new(Optional<GetOneTimeTokenPaymentProfile>.Some(value), default, default);

    public static PaymentProfileModel GetOneTimeTokenBankAccountPaymentProfile(GetOneTimeTokenBankAccountPaymentProfile value) =>
        new(default, Optional<GetOneTimeTokenBankAccountPaymentProfile>.Some(value), default);

    internal static PaymentProfileModel Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetGetOneTimeTokenPaymentProfile([NotNullWhen(true)] out GetOneTimeTokenPaymentProfile? value) =>
        _getOneTimeTokenPaymentProfileValue.TryGetValue(out value);

    public bool TryGetGetOneTimeTokenBankAccountPaymentProfile([NotNullWhen(true)] out GetOneTimeTokenBankAccountPaymentProfile? value) =>
        _getOneTimeTokenBankAccountPaymentProfileValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator PaymentProfileModel(GetOneTimeTokenPaymentProfile value) =>
        GetOneTimeTokenPaymentProfile(value);

    public static implicit operator PaymentProfileModel(GetOneTimeTokenBankAccountPaymentProfile value) =>
        GetOneTimeTokenBankAccountPaymentProfile(value);

    public TResult Match<TResult>(Func<GetOneTimeTokenPaymentProfile, TResult> onGetOneTimeTokenPaymentProfile,
        Func<GetOneTimeTokenBankAccountPaymentProfile, TResult> onGetOneTimeTokenBankAccountPaymentProfile,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetGetOneTimeTokenPaymentProfile(
                out var getOneTimeTokenPaymentProfileValue) => onGetOneTimeTokenPaymentProfile(
                getOneTimeTokenPaymentProfileValue),
            _ when TryGetGetOneTimeTokenBankAccountPaymentProfile(
                out var getOneTimeTokenBankAccountPaymentProfileValue) => onGetOneTimeTokenBankAccountPaymentProfile(
                getOneTimeTokenBankAccountPaymentProfileValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(PaymentProfileModel)} holds no value.")
        };

    public void Match(Action<GetOneTimeTokenPaymentProfile> onGetOneTimeTokenPaymentProfile,
        Action<GetOneTimeTokenBankAccountPaymentProfile> onGetOneTimeTokenBankAccountPaymentProfile,
        Action<JsonElement> otherwise)
    {
        if (TryGetGetOneTimeTokenPaymentProfile(
            out var getOneTimeTokenPaymentProfileValue)) onGetOneTimeTokenPaymentProfile(
            getOneTimeTokenPaymentProfileValue);
        else if (TryGetGetOneTimeTokenBankAccountPaymentProfile(
            out var getOneTimeTokenBankAccountPaymentProfileValue)) onGetOneTimeTokenBankAccountPaymentProfile(
            getOneTimeTokenBankAccountPaymentProfileValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(PaymentProfileModel)} holds no value.");
    }
}

file sealed class PaymentProfileModelConverter : JsonConverter<PaymentProfileModel>
{
    public override PaymentProfileModel Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (!root.TryGetProperty("payment_type", out var typeProperty) ||
            typeProperty.ValueKind != JsonValueKind.String)
        {
            throw new JsonException("Missing or non-string 'payment_type' discriminator field");
        }
        return typeProperty.GetString() switch
        {
            "credit_card" => PaymentProfileModel.GetOneTimeTokenPaymentProfile(
                root.Deserialize<GetOneTimeTokenPaymentProfile>(options)!),
            "bank_account" => PaymentProfileModel.GetOneTimeTokenBankAccountPaymentProfile(
                root.Deserialize<GetOneTimeTokenBankAccountPaymentProfile>(options)!),
            _ => PaymentProfileModel.Unknown(root)
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
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(PaymentProfileModel)} contains no valid value to serialize.");
        }
    }
}
