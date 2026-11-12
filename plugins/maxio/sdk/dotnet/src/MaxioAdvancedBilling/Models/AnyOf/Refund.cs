using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(RefundConverter))]
public record Refund
{
    private readonly Optional<RefundInvoice> _refundInvoiceValue;

    private readonly Optional<RefundConsolidatedInvoice> _refundConsolidatedInvoiceValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private Refund(Optional<RefundInvoice> refundInvoiceValue,
        Optional<RefundConsolidatedInvoice> refundConsolidatedInvoiceValue,
        Optional<UnknownArm> unknownValue)
    {
        _refundInvoiceValue = refundInvoiceValue;
        _refundConsolidatedInvoiceValue = refundConsolidatedInvoiceValue;
        _unknownValue = unknownValue;
    }

    public static Refund RefundInvoice(RefundInvoice value) =>
        new(Optional<RefundInvoice>.Some(value), default, default);

    public static Refund RefundConsolidatedInvoice(RefundConsolidatedInvoice value) =>
        new(default, Optional<RefundConsolidatedInvoice>.Some(value), default);

    internal static Refund Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetRefundInvoice([NotNullWhen(true)] out RefundInvoice? value) =>
        _refundInvoiceValue.TryGetValue(out value);

    public bool TryGetRefundConsolidatedInvoice([NotNullWhen(true)] out RefundConsolidatedInvoice? value) =>
        _refundConsolidatedInvoiceValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator Refund(RefundInvoice value) => RefundInvoice(value);

    public static implicit operator Refund(RefundConsolidatedInvoice value) => RefundConsolidatedInvoice(value);

    public TResult Match<TResult>(Func<RefundInvoice, TResult> onRefundInvoice,
        Func<RefundConsolidatedInvoice, TResult> onRefundConsolidatedInvoice,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetRefundInvoice(out var refundInvoiceValue) => onRefundInvoice(refundInvoiceValue),
            _ when TryGetRefundConsolidatedInvoice(
                out var refundConsolidatedInvoiceValue) => onRefundConsolidatedInvoice(refundConsolidatedInvoiceValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(Refund)} holds no value.")
        };

    public void Match(Action<RefundInvoice> onRefundInvoice,
        Action<RefundConsolidatedInvoice> onRefundConsolidatedInvoice,
        Action<JsonElement> otherwise)
    {
        if (TryGetRefundInvoice(out var refundInvoiceValue)) onRefundInvoice(refundInvoiceValue);
        else if (TryGetRefundConsolidatedInvoice(out var refundConsolidatedInvoiceValue)) onRefundConsolidatedInvoice(
            refundConsolidatedInvoiceValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(Refund)} holds no value.");
    }
}

file sealed class RefundConverter : JsonConverter<Refund>
{
    public override Refund Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<RefundInvoice>(root, options, out var refundInvoiceValue))
        {
            return Refund.RefundInvoice(refundInvoiceValue);
        }
        if (JsonSerializer.TryDeserialize<RefundConsolidatedInvoice>(
            root,
            options,
            out var refundConsolidatedInvoiceValue))
        {
            return Refund.RefundConsolidatedInvoice(refundConsolidatedInvoiceValue);
        }
        return Refund.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, Refund value, JsonSerializerOptions options)
    {
        if (value.TryGetRefundInvoice(out var refundInvoiceValue))
        {
            JsonSerializer.Serialize(writer, refundInvoiceValue, options);
        }
        else if (value.TryGetRefundConsolidatedInvoice(out var refundConsolidatedInvoiceValue))
        {
            JsonSerializer.Serialize(writer, refundConsolidatedInvoiceValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(Refund)} contains no valid value to serialize.");
        }
    }
}
