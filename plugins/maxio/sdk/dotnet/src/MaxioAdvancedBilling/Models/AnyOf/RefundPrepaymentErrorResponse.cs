using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(RefundPrepaymentErrorResponseConverter))]
public record RefundPrepaymentErrorResponse
{
    private readonly Optional<RefundPrepaymentAggregatedErrorsResponse> _refundPrepaymentAggregatedErrorsResponseValue;

    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private readonly Optional<UnknownArm> _unknownValue;

    private RefundPrepaymentErrorResponse(Optional<RefundPrepaymentAggregatedErrorsResponse> refundPrepaymentAggregatedErrorsResponseValue,
        Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<UnknownArm> unknownValue)
    {
        _refundPrepaymentAggregatedErrorsResponseValue = refundPrepaymentAggregatedErrorsResponseValue;
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
        _unknownValue = unknownValue;
    }

    public static RefundPrepaymentErrorResponse RefundPrepaymentAggregatedErrorsResponse(RefundPrepaymentAggregatedErrorsResponse value) =>
        new(Optional<RefundPrepaymentAggregatedErrorsResponse>.Some(value), default, default);

    public static RefundPrepaymentErrorResponse ErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(default, Optional<ErrorStringMapResponse1>.Some(value), default);

    internal static RefundPrepaymentErrorResponse Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetRefundPrepaymentAggregatedErrorsResponse([NotNullWhen(true)] out RefundPrepaymentAggregatedErrorsResponse? value) =>
        _refundPrepaymentAggregatedErrorsResponseValue.TryGetValue(out value);

    public bool TryGetErrorStringMapResponse1([NotNullWhen(true)] out ErrorStringMapResponse1? value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator RefundPrepaymentErrorResponse(RefundPrepaymentAggregatedErrorsResponse value) =>
        RefundPrepaymentAggregatedErrorsResponse(value);

    public static implicit operator RefundPrepaymentErrorResponse(ErrorStringMapResponse1 value) =>
        ErrorStringMapResponse1(value);

    public TResult Match<TResult>(Func<RefundPrepaymentAggregatedErrorsResponse, TResult> onRefundPrepaymentAggregatedErrorsResponse,
        Func<ErrorStringMapResponse1, TResult> onErrorStringMapResponse1,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetRefundPrepaymentAggregatedErrorsResponse(
                out var refundPrepaymentAggregatedErrorsResponseValue) => onRefundPrepaymentAggregatedErrorsResponse(
                refundPrepaymentAggregatedErrorsResponseValue),
            _ when TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value) => onErrorStringMapResponse1(
                errorStringMapResponse1Value),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(RefundPrepaymentErrorResponse)} holds no value.")
        };

    public void Match(Action<RefundPrepaymentAggregatedErrorsResponse> onRefundPrepaymentAggregatedErrorsResponse,
        Action<ErrorStringMapResponse1> onErrorStringMapResponse1,
        Action<JsonElement> otherwise)
    {
        if (TryGetRefundPrepaymentAggregatedErrorsResponse(
            out var refundPrepaymentAggregatedErrorsResponseValue)) onRefundPrepaymentAggregatedErrorsResponse(
            refundPrepaymentAggregatedErrorsResponseValue);
        else if (TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value)) onErrorStringMapResponse1(
            errorStringMapResponse1Value);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(RefundPrepaymentErrorResponse)} holds no value.");
    }
}

file sealed class RefundPrepaymentErrorResponseConverter : JsonConverter<RefundPrepaymentErrorResponse>
{
    public override RefundPrepaymentErrorResponse Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<RefundPrepaymentAggregatedErrorsResponse>(
            root,
            options,
            out var refundPrepaymentAggregatedErrorsResponseValue))
        {
            return RefundPrepaymentErrorResponse.RefundPrepaymentAggregatedErrorsResponse(
                refundPrepaymentAggregatedErrorsResponseValue);
        }
        if (JsonSerializer.TryDeserialize<ErrorStringMapResponse1>(root, options, out var errorStringMapResponse1Value))
        {
            return RefundPrepaymentErrorResponse.ErrorStringMapResponse1(errorStringMapResponse1Value);
        }
        return RefundPrepaymentErrorResponse.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer,
        RefundPrepaymentErrorResponse value,
        JsonSerializerOptions options)
    {
        if (value.TryGetRefundPrepaymentAggregatedErrorsResponse(out var refundPrepaymentAggregatedErrorsResponseValue))
        {
            JsonSerializer.Serialize(writer, refundPrepaymentAggregatedErrorsResponseValue, options);
        }
        else if (value.TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value))
        {
            JsonSerializer.Serialize(writer, errorStringMapResponse1Value, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(RefundPrepaymentErrorResponse)} contains no valid value to serialize.");
        }
    }
}
