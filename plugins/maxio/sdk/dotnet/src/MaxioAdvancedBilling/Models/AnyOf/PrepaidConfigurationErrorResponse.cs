using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(PrepaidConfigurationErrorResponseConverter))]
public record PrepaidConfigurationErrorResponse
{
    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<UnknownArm> _unknownValue;

    private PrepaidConfigurationErrorResponse(Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<UnknownArm> unknownValue)
    {
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
        _errorListResponse1Value = errorListResponse1Value;
        _unknownValue = unknownValue;
    }

    public static PrepaidConfigurationErrorResponse ErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(Optional<ErrorStringMapResponse1>.Some(value), default, default);

    public static PrepaidConfigurationErrorResponse ErrorListResponse1(ErrorListResponse1 value) =>
        new(default, Optional<ErrorListResponse1>.Some(value), default);

    internal static PrepaidConfigurationErrorResponse Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetErrorStringMapResponse1([NotNullWhen(true)] out ErrorStringMapResponse1? value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator PrepaidConfigurationErrorResponse(ErrorStringMapResponse1 value) =>
        ErrorStringMapResponse1(value);

    public static implicit operator PrepaidConfigurationErrorResponse(ErrorListResponse1 value) =>
        ErrorListResponse1(value);

    public TResult Match<TResult>(Func<ErrorStringMapResponse1, TResult> onErrorStringMapResponse1,
        Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value) => onErrorStringMapResponse1(
                errorStringMapResponse1Value),
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(PrepaidConfigurationErrorResponse)} holds no value.")
        };

    public void Match(Action<ErrorStringMapResponse1> onErrorStringMapResponse1,
        Action<ErrorListResponse1> onErrorListResponse1,
        Action<JsonElement> otherwise)
    {
        if (TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value)) onErrorStringMapResponse1(
            errorStringMapResponse1Value);
        else if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(
            errorListResponse1Value);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(PrepaidConfigurationErrorResponse)} holds no value.");
    }
}

file sealed class PrepaidConfigurationErrorResponseConverter : JsonConverter<PrepaidConfigurationErrorResponse>
{
    public override PrepaidConfigurationErrorResponse Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<ErrorStringMapResponse1>(root, options, out var errorStringMapResponse1Value))
        {
            return PrepaidConfigurationErrorResponse.ErrorStringMapResponse1(errorStringMapResponse1Value);
        }
        if (JsonSerializer.TryDeserialize<ErrorListResponse1>(root, options, out var errorListResponse1Value))
        {
            return PrepaidConfigurationErrorResponse.ErrorListResponse1(errorListResponse1Value);
        }
        return PrepaidConfigurationErrorResponse.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer,
        PrepaidConfigurationErrorResponse value,
        JsonSerializerOptions options)
    {
        if (value.TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value))
        {
            JsonSerializer.Serialize(writer, errorStringMapResponse1Value, options);
        }
        else if (value.TryGetErrorListResponse1(out var errorListResponse1Value))
        {
            JsonSerializer.Serialize(writer, errorListResponse1Value, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException(
                $"{nameof(PrepaidConfigurationErrorResponse)} contains no valid value to serialize.");
        }
    }
}
