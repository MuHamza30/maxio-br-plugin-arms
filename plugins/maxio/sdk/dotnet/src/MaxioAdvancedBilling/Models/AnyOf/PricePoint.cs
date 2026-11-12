using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(PricePointConverter))]
public record PricePoint
{
    private readonly Optional<CreateComponentPricePoint> _createComponentPricePointValue;

    private readonly Optional<CreatePrepaidUsageComponentPricePoint> _createPrepaidUsageComponentPricePointValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private PricePoint(Optional<CreateComponentPricePoint> createComponentPricePointValue,
        Optional<CreatePrepaidUsageComponentPricePoint> createPrepaidUsageComponentPricePointValue,
        Optional<UnknownArm> unknownValue)
    {
        _createComponentPricePointValue = createComponentPricePointValue;
        _createPrepaidUsageComponentPricePointValue = createPrepaidUsageComponentPricePointValue;
        _unknownValue = unknownValue;
    }

    public static PricePoint CreateComponentPricePoint(CreateComponentPricePoint value) =>
        new(Optional<CreateComponentPricePoint>.Some(value), default, default);

    public static PricePoint CreatePrepaidUsageComponentPricePoint(CreatePrepaidUsageComponentPricePoint value) =>
        new(default, Optional<CreatePrepaidUsageComponentPricePoint>.Some(value), default);

    internal static PricePoint Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetCreateComponentPricePoint([NotNullWhen(true)] out CreateComponentPricePoint? value) =>
        _createComponentPricePointValue.TryGetValue(out value);

    public bool TryGetCreatePrepaidUsageComponentPricePoint([NotNullWhen(true)] out CreatePrepaidUsageComponentPricePoint? value) =>
        _createPrepaidUsageComponentPricePointValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator PricePoint(CreateComponentPricePoint value) => CreateComponentPricePoint(value);

    public static implicit operator PricePoint(CreatePrepaidUsageComponentPricePoint value) =>
        CreatePrepaidUsageComponentPricePoint(value);

    public TResult Match<TResult>(Func<CreateComponentPricePoint, TResult> onCreateComponentPricePoint,
        Func<CreatePrepaidUsageComponentPricePoint, TResult> onCreatePrepaidUsageComponentPricePoint,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCreateComponentPricePoint(
                out var createComponentPricePointValue) => onCreateComponentPricePoint(createComponentPricePointValue),
            _ when TryGetCreatePrepaidUsageComponentPricePoint(
                out var createPrepaidUsageComponentPricePointValue) => onCreatePrepaidUsageComponentPricePoint(
                createPrepaidUsageComponentPricePointValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(PricePoint)} holds no value.")
        };

    public void Match(Action<CreateComponentPricePoint> onCreateComponentPricePoint,
        Action<CreatePrepaidUsageComponentPricePoint> onCreatePrepaidUsageComponentPricePoint,
        Action<JsonElement> otherwise)
    {
        if (TryGetCreateComponentPricePoint(out var createComponentPricePointValue)) onCreateComponentPricePoint(
            createComponentPricePointValue);
        else if (TryGetCreatePrepaidUsageComponentPricePoint(
            out var createPrepaidUsageComponentPricePointValue)) onCreatePrepaidUsageComponentPricePoint(
            createPrepaidUsageComponentPricePointValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(PricePoint)} holds no value.");
    }
}

file sealed class PricePointConverter : JsonConverter<PricePoint>
{
    public override PricePoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<CreateComponentPricePoint>(
            root,
            options,
            out var createComponentPricePointValue))
        {
            return PricePoint.CreateComponentPricePoint(createComponentPricePointValue);
        }
        if (JsonSerializer.TryDeserialize<CreatePrepaidUsageComponentPricePoint>(
            root,
            options,
            out var createPrepaidUsageComponentPricePointValue))
        {
            return PricePoint.CreatePrepaidUsageComponentPricePoint(createPrepaidUsageComponentPricePointValue);
        }
        return PricePoint.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, PricePoint value, JsonSerializerOptions options)
    {
        if (value.TryGetCreateComponentPricePoint(out var createComponentPricePointValue))
        {
            JsonSerializer.Serialize(writer, createComponentPricePointValue, options);
        }
        else if (value.TryGetCreatePrepaidUsageComponentPricePoint(out var createPrepaidUsageComponentPricePointValue))
        {
            JsonSerializer.Serialize(writer, createPrepaidUsageComponentPricePointValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(PricePoint)} contains no valid value to serialize.");
        }
    }
}
