using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

/// <summary>
/// A nested data structure detailing the method of payment
/// </summary>
[JsonConverter(typeof(InvoiceEventPaymentConverter))]
public record InvoiceEventPayment
{
    private readonly Optional<PaymentMethodApplePay> _paymentMethodApplePayValue;

    private readonly Optional<PaymentMethodBankAccount> _paymentMethodBankAccountValue;

    private readonly Optional<PaymentMethodCreditCard> _paymentMethodCreditCardValue;

    private readonly Optional<PaymentMethodExternal> _paymentMethodExternalValue;

    private readonly Optional<PaymentMethodPaypal> _paymentMethodPaypalValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private InvoiceEventPayment(Optional<PaymentMethodApplePay> paymentMethodApplePayValue,
        Optional<PaymentMethodBankAccount> paymentMethodBankAccountValue,
        Optional<PaymentMethodCreditCard> paymentMethodCreditCardValue,
        Optional<PaymentMethodExternal> paymentMethodExternalValue,
        Optional<PaymentMethodPaypal> paymentMethodPaypalValue,
        Optional<UnknownArm> unknownValue)
    {
        _paymentMethodApplePayValue = paymentMethodApplePayValue;
        _paymentMethodBankAccountValue = paymentMethodBankAccountValue;
        _paymentMethodCreditCardValue = paymentMethodCreditCardValue;
        _paymentMethodExternalValue = paymentMethodExternalValue;
        _paymentMethodPaypalValue = paymentMethodPaypalValue;
        _unknownValue = unknownValue;
    }

    public static InvoiceEventPayment PaymentMethodApplePay(PaymentMethodApplePay value) =>
        new(Optional<PaymentMethodApplePay>.Some(value), default, default, default, default, default);

    public static InvoiceEventPayment PaymentMethodBankAccount(PaymentMethodBankAccount value) =>
        new(default, Optional<PaymentMethodBankAccount>.Some(value), default, default, default, default);

    public static InvoiceEventPayment PaymentMethodCreditCard(PaymentMethodCreditCard value) =>
        new(default, default, Optional<PaymentMethodCreditCard>.Some(value), default, default, default);

    public static InvoiceEventPayment PaymentMethodExternal(PaymentMethodExternal value) =>
        new(default, default, default, Optional<PaymentMethodExternal>.Some(value), default, default);

    public static InvoiceEventPayment PaymentMethodPaypal(PaymentMethodPaypal value) =>
        new(default, default, default, default, Optional<PaymentMethodPaypal>.Some(value), default);

    internal static InvoiceEventPayment Unknown(JsonElement value) =>
        new(default, default, default, default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetPaymentMethodApplePay([NotNullWhen(true)] out PaymentMethodApplePay? value) =>
        _paymentMethodApplePayValue.TryGetValue(out value);

    public bool TryGetPaymentMethodBankAccount([NotNullWhen(true)] out PaymentMethodBankAccount? value) =>
        _paymentMethodBankAccountValue.TryGetValue(out value);

    public bool TryGetPaymentMethodCreditCard([NotNullWhen(true)] out PaymentMethodCreditCard? value) =>
        _paymentMethodCreditCardValue.TryGetValue(out value);

    public bool TryGetPaymentMethodExternal([NotNullWhen(true)] out PaymentMethodExternal? value) =>
        _paymentMethodExternalValue.TryGetValue(out value);

    public bool TryGetPaymentMethodPaypal([NotNullWhen(true)] out PaymentMethodPaypal? value) =>
        _paymentMethodPaypalValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator InvoiceEventPayment(PaymentMethodApplePay value) => PaymentMethodApplePay(value);

    public static implicit operator InvoiceEventPayment(PaymentMethodBankAccount value) =>
        PaymentMethodBankAccount(value);

    public static implicit operator InvoiceEventPayment(PaymentMethodCreditCard value) =>
        PaymentMethodCreditCard(value);

    public static implicit operator InvoiceEventPayment(PaymentMethodExternal value) => PaymentMethodExternal(value);

    public static implicit operator InvoiceEventPayment(PaymentMethodPaypal value) => PaymentMethodPaypal(value);

    public TResult Match<TResult>(Func<PaymentMethodApplePay, TResult> onPaymentMethodApplePay,
        Func<PaymentMethodBankAccount, TResult> onPaymentMethodBankAccount,
        Func<PaymentMethodCreditCard, TResult> onPaymentMethodCreditCard,
        Func<PaymentMethodExternal, TResult> onPaymentMethodExternal,
        Func<PaymentMethodPaypal, TResult> onPaymentMethodPaypal,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetPaymentMethodApplePay(out var paymentMethodApplePayValue) => onPaymentMethodApplePay(
                paymentMethodApplePayValue),
            _ when TryGetPaymentMethodBankAccount(out var paymentMethodBankAccountValue) => onPaymentMethodBankAccount(
                paymentMethodBankAccountValue),
            _ when TryGetPaymentMethodCreditCard(out var paymentMethodCreditCardValue) => onPaymentMethodCreditCard(
                paymentMethodCreditCardValue),
            _ when TryGetPaymentMethodExternal(out var paymentMethodExternalValue) => onPaymentMethodExternal(
                paymentMethodExternalValue),
            _ when TryGetPaymentMethodPaypal(out var paymentMethodPaypalValue) => onPaymentMethodPaypal(
                paymentMethodPaypalValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(InvoiceEventPayment)} holds no value.")
        };

    public void Match(Action<PaymentMethodApplePay> onPaymentMethodApplePay,
        Action<PaymentMethodBankAccount> onPaymentMethodBankAccount,
        Action<PaymentMethodCreditCard> onPaymentMethodCreditCard,
        Action<PaymentMethodExternal> onPaymentMethodExternal,
        Action<PaymentMethodPaypal> onPaymentMethodPaypal,
        Action<JsonElement> otherwise)
    {
        if (TryGetPaymentMethodApplePay(out var paymentMethodApplePayValue)) onPaymentMethodApplePay(
            paymentMethodApplePayValue);
        else if (TryGetPaymentMethodBankAccount(out var paymentMethodBankAccountValue)) onPaymentMethodBankAccount(
            paymentMethodBankAccountValue);
        else if (TryGetPaymentMethodCreditCard(out var paymentMethodCreditCardValue)) onPaymentMethodCreditCard(
            paymentMethodCreditCardValue);
        else if (TryGetPaymentMethodExternal(out var paymentMethodExternalValue)) onPaymentMethodExternal(
            paymentMethodExternalValue);
        else if (TryGetPaymentMethodPaypal(out var paymentMethodPaypalValue)) onPaymentMethodPaypal(
            paymentMethodPaypalValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(InvoiceEventPayment)} holds no value.");
    }
}

file sealed class InvoiceEventPaymentConverter : JsonConverter<InvoiceEventPayment>
{
    public override InvoiceEventPayment Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (!root.TryGetProperty("type", out var typeProperty) || typeProperty.ValueKind != JsonValueKind.String)
        {
            throw new JsonException("Missing or non-string 'type' discriminator field");
        }
        return typeProperty.GetString() switch
        {
            "apple_pay" => InvoiceEventPayment.PaymentMethodApplePay(root.Deserialize<PaymentMethodApplePay>(options)!),
            "bank_account" => InvoiceEventPayment.PaymentMethodBankAccount(root.Deserialize<PaymentMethodBankAccount>(
                options)!),
            "credit_card" => InvoiceEventPayment.PaymentMethodCreditCard(root.Deserialize<PaymentMethodCreditCard>(
                options)!),
            "external" => InvoiceEventPayment.PaymentMethodExternal(root.Deserialize<PaymentMethodExternal>(options)!),
            "paypal_account" => InvoiceEventPayment.PaymentMethodPaypal(root.Deserialize<PaymentMethodPaypal>(
                options)!),
            _ => InvoiceEventPayment.Unknown(root)
        };
    }

    public override void Write(Utf8JsonWriter writer, InvoiceEventPayment value, JsonSerializerOptions options)
    {
        if (value.TryGetPaymentMethodApplePay(out var paymentMethodApplePayValue))
        {
            JsonSerializer.Serialize(writer, paymentMethodApplePayValue, options);
        }
        else if (value.TryGetPaymentMethodBankAccount(out var paymentMethodBankAccountValue))
        {
            JsonSerializer.Serialize(writer, paymentMethodBankAccountValue, options);
        }
        else if (value.TryGetPaymentMethodCreditCard(out var paymentMethodCreditCardValue))
        {
            JsonSerializer.Serialize(writer, paymentMethodCreditCardValue, options);
        }
        else if (value.TryGetPaymentMethodExternal(out var paymentMethodExternalValue))
        {
            JsonSerializer.Serialize(writer, paymentMethodExternalValue, options);
        }
        else if (value.TryGetPaymentMethodPaypal(out var paymentMethodPaypalValue))
        {
            JsonSerializer.Serialize(writer, paymentMethodPaypalValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(InvoiceEventPayment)} contains no valid value to serialize.");
        }
    }
}
