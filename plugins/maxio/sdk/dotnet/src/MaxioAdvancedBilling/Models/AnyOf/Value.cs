using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// The aggregated value, coerced according to <c>type</c>: a boolean for <c>access_right</c> (and boolean <c>service_right</c>), a number for <c>usage_limit</c> (and numeric <c>service_right</c>), or a string for text <c>service_right</c>.
/// </summary>
[JsonConverter(typeof(ValueConverter))]
public record Value
{
    private readonly Optional<bool> _boolValue;

    private readonly Optional<double> _doubleValue;

    private readonly Optional<string> _stringValue;

    private Value(Optional<bool> boolValue, Optional<double> doubleValue, Optional<string> stringValue)
    {
        _boolValue = boolValue;
        _doubleValue = doubleValue;
        _stringValue = stringValue;
    }

    public static Value Bool(bool value) => new(Optional<bool>.Some(value), default, default);

    public static Value Double(double value) => new(default, Optional<double>.Some(value), default);

    public static Value String(string value) => new(default, default, Optional<string>.Some(value));

    public bool TryGetBool(out bool value) => _boolValue.TryGetValue(out value);

    public bool TryGetDouble(out double value) => _doubleValue.TryGetValue(out value);

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public static implicit operator Value(bool value) => Bool(value);

    public static implicit operator Value(double value) => Double(value);

    public static implicit operator Value(string value) => String(value);
}

file sealed class ValueConverter : JsonConverter<Value>
{
    public override Value Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<bool>(root, options, out var boolValue))
        {
            return Value.Bool(boolValue);
        }
        if (JsonSerializer.TryDeserialize<double>(root, options, out var doubleValue))
        {
            return Value.Double(doubleValue);
        }
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return Value.String(value);
        }
        throw new JsonException($"JSON does not match bool or double or string schemas: {root.ToString()}");
    }

    public override void Write(Utf8JsonWriter writer, Value value, JsonSerializerOptions options)
    {
        if (value.TryGetBool(out var boolValue))
        {
            JsonSerializer.Serialize(writer, boolValue, options);
        }
        else if (value.TryGetDouble(out var doubleValue))
        {
            JsonSerializer.Serialize(writer, doubleValue, options);
        }
        else if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else
        {
            throw new JsonException($"{nameof(Value)} contains no valid value to serialize.");
        }
    }
}
