using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// Always positive; <c>kind</c> determines the sign written to the ledger. Encoded as a decimal string with at most 8 fractional digits. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
/// </summary>
[JsonConverter(typeof(Amount6Converter))]
public record Amount6
{
    private readonly Optional<string> _stringValue;

    private readonly Optional<int> _intValue;

    private Amount6(Optional<string> stringValue, Optional<int> intValue)
    {
        _stringValue = stringValue;
        _intValue = intValue;
    }

    public static Amount6 String(string value) => new(Optional<string>.Some(value), default);

    public static Amount6 Int(int value) => new(default, Optional<int>.Some(value));

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public bool TryGetInt(out int value) => _intValue.TryGetValue(out value);

    public static implicit operator Amount6(string value) => String(value);

    public static implicit operator Amount6(int value) => Int(value);
}

file sealed class Amount6Converter : JsonConverter<Amount6>
{
    public override Amount6 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return Amount6.String(value);
        }
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt32(out var intValue))
            {
                return Amount6.Int(intValue);
            }
        }
        throw new JsonException($"JSON does not match string or int schemas: {root.ToString()}");
    }

    public override void Write(Utf8JsonWriter writer, Amount6 value, JsonSerializerOptions options)
    {
        if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else if (value.TryGetInt(out var intValue))
        {
            JsonSerializer.Serialize(writer, intValue, options);
        }
        else
        {
            throw new JsonException($"{nameof(Amount6)} contains no valid value to serialize.");
        }
    }
}
