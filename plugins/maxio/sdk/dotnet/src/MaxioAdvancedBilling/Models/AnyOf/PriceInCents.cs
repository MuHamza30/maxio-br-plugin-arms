using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// Required if using <c>custom_price</c> attribute.
/// </summary>
[JsonConverter(typeof(PriceInCentsConverter))]
public record PriceInCents
{
    private readonly Optional<string> _stringValue;

    private readonly Optional<long> _longValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private PriceInCents(Optional<string> stringValue, Optional<long> longValue, Optional<UnknownArm> unknownValue)
    {
        _stringValue = stringValue;
        _longValue = longValue;
        _unknownValue = unknownValue;
    }

    public static PriceInCents String(string value) => new(Optional<string>.Some(value), default, default);

    public static PriceInCents Long(long value) => new(default, Optional<long>.Some(value), default);

    internal static PriceInCents Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetString([NotNullWhen(true)] out string? value) => _stringValue.TryGetValue(out value);

    public bool TryGetLong(out long value) => _longValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator PriceInCents(string value) => String(value);

    public static implicit operator PriceInCents(long value) => Long(value);

    public TResult Match<TResult>(Func<string, TResult> onString,
        Func<long, TResult> onLong,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetString(out var stringValue) => onString(stringValue),
            _ when TryGetLong(out var longValue) => onLong(longValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(PriceInCents)} holds no value.")
        };

    public void Match(Action<string> onString, Action<long> onLong, Action<JsonElement> otherwise)
    {
        if (TryGetString(out var stringValue)) onString(stringValue);
        else if (TryGetLong(out var longValue)) onLong(longValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(PriceInCents)} holds no value.");
    }
}

file sealed class PriceInCentsConverter : JsonConverter<PriceInCents>
{
    public override PriceInCents Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return PriceInCents.String(value);
        }
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt64(out var longValue))
            {
                return PriceInCents.Long(longValue);
            }
        }
        return PriceInCents.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, PriceInCents value, JsonSerializerOptions options)
    {
        if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else if (value.TryGetLong(out var longValue))
        {
            JsonSerializer.Serialize(writer, longValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(PriceInCents)} contains no valid value to serialize.");
        }
    }
}
