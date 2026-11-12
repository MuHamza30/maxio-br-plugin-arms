using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(SegmentProperty3Value1Converter))]
public record SegmentProperty3Value1
{
    private readonly Optional<string> _stringValue;

    private readonly Optional<double> _doubleValue;

    private readonly Optional<int> _intValue;

    private readonly Optional<bool> _boolValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private SegmentProperty3Value1(Optional<string> stringValue,
        Optional<double> doubleValue,
        Optional<int> intValue,
        Optional<bool> boolValue,
        Optional<UnknownArm> unknownValue)
    {
        _stringValue = stringValue;
        _doubleValue = doubleValue;
        _intValue = intValue;
        _boolValue = boolValue;
        _unknownValue = unknownValue;
    }

    public static SegmentProperty3Value1 String(string value) =>
        new(Optional<string>.Some(value), default, default, default, default);

    public static SegmentProperty3Value1 Double(double value) =>
        new(default, Optional<double>.Some(value), default, default, default);

    public static SegmentProperty3Value1 Int(int value) =>
        new(default, default, Optional<int>.Some(value), default, default);

    public static SegmentProperty3Value1 Bool(bool value) =>
        new(default, default, default, Optional<bool>.Some(value), default);

    internal static SegmentProperty3Value1 Unknown(JsonElement value) =>
        new(default, default, default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetString([NotNullWhen(true)] out string? value) => _stringValue.TryGetValue(out value);

    public bool TryGetDouble(out double value) => _doubleValue.TryGetValue(out value);

    public bool TryGetInt(out int value) => _intValue.TryGetValue(out value);

    public bool TryGetBool(out bool value) => _boolValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator SegmentProperty3Value1(string value) => String(value);

    public static implicit operator SegmentProperty3Value1(double value) => Double(value);

    public static implicit operator SegmentProperty3Value1(int value) => Int(value);

    public static implicit operator SegmentProperty3Value1(bool value) => Bool(value);

    public TResult Match<TResult>(Func<string, TResult> onString,
        Func<double, TResult> onDouble,
        Func<int, TResult> onInt,
        Func<bool, TResult> onBool,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetString(out var stringValue) => onString(stringValue),
            _ when TryGetDouble(out var doubleValue) => onDouble(doubleValue),
            _ when TryGetInt(out var intValue) => onInt(intValue),
            _ when TryGetBool(out var boolValue) => onBool(boolValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(SegmentProperty3Value1)} holds no value.")
        };

    public void Match(Action<string> onString,
        Action<double> onDouble,
        Action<int> onInt,
        Action<bool> onBool,
        Action<JsonElement> otherwise)
    {
        if (TryGetString(out var stringValue)) onString(stringValue);
        else if (TryGetDouble(out var doubleValue)) onDouble(doubleValue);
        else if (TryGetInt(out var intValue)) onInt(intValue);
        else if (TryGetBool(out var boolValue)) onBool(boolValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(SegmentProperty3Value1)} holds no value.");
    }
}

file sealed class SegmentProperty3Value1Converter : JsonConverter<SegmentProperty3Value1>
{
    public override SegmentProperty3Value1 Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return SegmentProperty3Value1.String(value);
        }
        if (JsonSerializer.TryDeserialize<double>(root, options, out var doubleValue))
        {
            return SegmentProperty3Value1.Double(doubleValue);
        }
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt32(out var intValue))
            {
                return SegmentProperty3Value1.Int(intValue);
            }
        }
        if (JsonSerializer.TryDeserialize<bool>(root, options, out var boolValue))
        {
            return SegmentProperty3Value1.Bool(boolValue);
        }
        return SegmentProperty3Value1.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, SegmentProperty3Value1 value, JsonSerializerOptions options)
    {
        if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else if (value.TryGetDouble(out var doubleValue))
        {
            JsonSerializer.Serialize(writer, doubleValue, options);
        }
        else if (value.TryGetInt(out var intValue))
        {
            JsonSerializer.Serialize(writer, intValue, options);
        }
        else if (value.TryGetBool(out var boolValue))
        {
            JsonSerializer.Serialize(writer, boolValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(SegmentProperty3Value1)} contains no valid value to serialize.");
        }
    }
}
