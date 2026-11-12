using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

[JsonConverter(typeof(PaymentProfileConverter))]
public record PaymentProfile
{
    private readonly Optional<ApplePayPaymentProfile> _applePayPaymentProfileValue;

    private readonly Optional<BankAccountPaymentProfile> _bankAccountPaymentProfileValue;

    private readonly Optional<CreditCardPaymentProfile> _creditCardPaymentProfileValue;

    private readonly Optional<PaypalPaymentProfile> _paypalPaymentProfileValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private PaymentProfile(Optional<ApplePayPaymentProfile> applePayPaymentProfileValue,
        Optional<BankAccountPaymentProfile> bankAccountPaymentProfileValue,
        Optional<CreditCardPaymentProfile> creditCardPaymentProfileValue,
        Optional<PaypalPaymentProfile> paypalPaymentProfileValue,
        Optional<UnknownArm> unknownValue)
    {
        _applePayPaymentProfileValue = applePayPaymentProfileValue;
        _bankAccountPaymentProfileValue = bankAccountPaymentProfileValue;
        _creditCardPaymentProfileValue = creditCardPaymentProfileValue;
        _paypalPaymentProfileValue = paypalPaymentProfileValue;
        _unknownValue = unknownValue;
    }

    public static PaymentProfile ApplePayPaymentProfile(ApplePayPaymentProfile value) =>
        new(Optional<ApplePayPaymentProfile>.Some(value), default, default, default, default);

    public static PaymentProfile BankAccountPaymentProfile(BankAccountPaymentProfile value) =>
        new(default, Optional<BankAccountPaymentProfile>.Some(value), default, default, default);

    public static PaymentProfile CreditCardPaymentProfile(CreditCardPaymentProfile value) =>
        new(default, default, Optional<CreditCardPaymentProfile>.Some(value), default, default);

    public static PaymentProfile PaypalPaymentProfile(PaypalPaymentProfile value) =>
        new(default, default, default, Optional<PaypalPaymentProfile>.Some(value), default);

    internal static PaymentProfile Unknown(JsonElement value) =>
        new(default, default, default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetApplePayPaymentProfile([NotNullWhen(true)] out ApplePayPaymentProfile? value) =>
        _applePayPaymentProfileValue.TryGetValue(out value);

    public bool TryGetBankAccountPaymentProfile([NotNullWhen(true)] out BankAccountPaymentProfile? value) =>
        _bankAccountPaymentProfileValue.TryGetValue(out value);

    public bool TryGetCreditCardPaymentProfile([NotNullWhen(true)] out CreditCardPaymentProfile? value) =>
        _creditCardPaymentProfileValue.TryGetValue(out value);

    public bool TryGetPaypalPaymentProfile([NotNullWhen(true)] out PaypalPaymentProfile? value) =>
        _paypalPaymentProfileValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator PaymentProfile(ApplePayPaymentProfile value) => ApplePayPaymentProfile(value);

    public static implicit operator PaymentProfile(BankAccountPaymentProfile value) => BankAccountPaymentProfile(value);

    public static implicit operator PaymentProfile(CreditCardPaymentProfile value) => CreditCardPaymentProfile(value);

    public static implicit operator PaymentProfile(PaypalPaymentProfile value) => PaypalPaymentProfile(value);

    public TResult Match<TResult>(Func<ApplePayPaymentProfile, TResult> onApplePayPaymentProfile,
        Func<BankAccountPaymentProfile, TResult> onBankAccountPaymentProfile,
        Func<CreditCardPaymentProfile, TResult> onCreditCardPaymentProfile,
        Func<PaypalPaymentProfile, TResult> onPaypalPaymentProfile,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetApplePayPaymentProfile(out var applePayPaymentProfileValue) => onApplePayPaymentProfile(
                applePayPaymentProfileValue),
            _ when TryGetBankAccountPaymentProfile(
                out var bankAccountPaymentProfileValue) => onBankAccountPaymentProfile(bankAccountPaymentProfileValue),
            _ when TryGetCreditCardPaymentProfile(out var creditCardPaymentProfileValue) => onCreditCardPaymentProfile(
                creditCardPaymentProfileValue),
            _ when TryGetPaypalPaymentProfile(out var paypalPaymentProfileValue) => onPaypalPaymentProfile(
                paypalPaymentProfileValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(PaymentProfile)} holds no value.")
        };

    public void Match(Action<ApplePayPaymentProfile> onApplePayPaymentProfile,
        Action<BankAccountPaymentProfile> onBankAccountPaymentProfile,
        Action<CreditCardPaymentProfile> onCreditCardPaymentProfile,
        Action<PaypalPaymentProfile> onPaypalPaymentProfile,
        Action<JsonElement> otherwise)
    {
        if (TryGetApplePayPaymentProfile(out var applePayPaymentProfileValue)) onApplePayPaymentProfile(
            applePayPaymentProfileValue);
        else if (TryGetBankAccountPaymentProfile(out var bankAccountPaymentProfileValue)) onBankAccountPaymentProfile(
            bankAccountPaymentProfileValue);
        else if (TryGetCreditCardPaymentProfile(out var creditCardPaymentProfileValue)) onCreditCardPaymentProfile(
            creditCardPaymentProfileValue);
        else if (TryGetPaypalPaymentProfile(out var paypalPaymentProfileValue)) onPaypalPaymentProfile(
            paypalPaymentProfileValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(PaymentProfile)} holds no value.");
    }
}

file sealed class PaymentProfileConverter : JsonConverter<PaymentProfile>
{
    public override PaymentProfile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "apple_pay" => PaymentProfile.ApplePayPaymentProfile(root.Deserialize<ApplePayPaymentProfile>(options)!),
            "bank_account" => PaymentProfile.BankAccountPaymentProfile(root.Deserialize<BankAccountPaymentProfile>(
                options)!),
            "credit_card" => PaymentProfile.CreditCardPaymentProfile(root.Deserialize<CreditCardPaymentProfile>(
                options)!),
            "paypal_account" => PaymentProfile.PaypalPaymentProfile(root.Deserialize<PaypalPaymentProfile>(options)!),
            _ => PaymentProfile.Unknown(root)
        };
    }

    public override void Write(Utf8JsonWriter writer, PaymentProfile value, JsonSerializerOptions options)
    {
        if (value.TryGetApplePayPaymentProfile(out var applePayPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, applePayPaymentProfileValue, options);
        }
        else if (value.TryGetBankAccountPaymentProfile(out var bankAccountPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, bankAccountPaymentProfileValue, options);
        }
        else if (value.TryGetCreditCardPaymentProfile(out var creditCardPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, creditCardPaymentProfileValue, options);
        }
        else if (value.TryGetPaypalPaymentProfile(out var paypalPaymentProfileValue))
        {
            JsonSerializer.Serialize(writer, paypalPaymentProfileValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(PaymentProfile)} contains no valid value to serialize.");
        }
    }
}
