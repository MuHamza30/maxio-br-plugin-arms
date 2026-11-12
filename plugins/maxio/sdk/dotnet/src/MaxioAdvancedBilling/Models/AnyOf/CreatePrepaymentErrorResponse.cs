using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(CreatePrepaymentErrorResponseConverter))]
public record CreatePrepaymentErrorResponse
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private readonly Optional<UnknownArm> _unknownValue;

    private CreatePrepaymentErrorResponse(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<UnknownArm> unknownValue)
    {
        _errorListResponse1Value = errorListResponse1Value;
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
        _unknownValue = unknownValue;
    }

    public static CreatePrepaymentErrorResponse ErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default, default);

    public static CreatePrepaymentErrorResponse ErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(default, Optional<ErrorStringMapResponse1>.Some(value), default);

    internal static CreatePrepaymentErrorResponse Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetErrorStringMapResponse1([NotNullWhen(true)] out ErrorStringMapResponse1? value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator CreatePrepaymentErrorResponse(ErrorListResponse1 value) =>
        ErrorListResponse1(value);

    public static implicit operator CreatePrepaymentErrorResponse(ErrorStringMapResponse1 value) =>
        ErrorStringMapResponse1(value);

    public TResult Match<TResult>(Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<ErrorStringMapResponse1, TResult> onErrorStringMapResponse1,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value) => onErrorStringMapResponse1(
                errorStringMapResponse1Value),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(CreatePrepaymentErrorResponse)} holds no value.")
        };

    public void Match(Action<ErrorListResponse1> onErrorListResponse1,
        Action<ErrorStringMapResponse1> onErrorStringMapResponse1,
        Action<JsonElement> otherwise)
    {
        if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(errorListResponse1Value);
        else if (TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value)) onErrorStringMapResponse1(
            errorStringMapResponse1Value);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(CreatePrepaymentErrorResponse)} holds no value.");
    }
}

file sealed class CreatePrepaymentErrorResponseConverter : JsonConverter<CreatePrepaymentErrorResponse>
{
    public override CreatePrepaymentErrorResponse Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<ErrorListResponse1>(root, options, out var errorListResponse1Value))
        {
            return CreatePrepaymentErrorResponse.ErrorListResponse1(errorListResponse1Value);
        }
        if (JsonSerializer.TryDeserialize<ErrorStringMapResponse1>(root, options, out var errorStringMapResponse1Value))
        {
            return CreatePrepaymentErrorResponse.ErrorStringMapResponse1(errorStringMapResponse1Value);
        }
        return CreatePrepaymentErrorResponse.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer,
        CreatePrepaymentErrorResponse value,
        JsonSerializerOptions options)
    {
        if (value.TryGetErrorListResponse1(out var errorListResponse1Value))
        {
            JsonSerializer.Serialize(writer, errorListResponse1Value, options);
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
            throw new JsonException($"{nameof(CreatePrepaymentErrorResponse)} contains no valid value to serialize.");
        }
    }
}
