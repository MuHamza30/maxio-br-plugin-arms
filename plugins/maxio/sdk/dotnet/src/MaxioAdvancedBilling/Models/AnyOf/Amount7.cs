using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// Signed: positive adds units to the grant, negative removes them. Bounded below by the grant's <c>posted_remaining</c> (a correction can't drive it negative) and above by the grant's <c>original</c> (a correction can't credit past what was originally granted). An out-of-bounds value is rejected rather than clamped. Zero is rejected. Without a <c>grant_id</c> (an overdraft correction), the amount must be positive, can't exceed the usage the overdraft entry still records, and can't exceed the overdraft the wallet still owes. Encoded as a decimal string with at most 8 fractional digits. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
/// </summary>
[JsonConverter(typeof(Amount7Converter))]
public record Amount7
{
    private readonly Optional<string> _stringValue;

    private readonly Optional<int> _intValue;

    private Amount7(Optional<string> stringValue, Optional<int> intValue)
    {
        _stringValue = stringValue;
        _intValue = intValue;
    }

    public static Amount7 String(string value) => new(Optional<string>.Some(value), default);

    public static Amount7 Int(int value) => new(default, Optional<int>.Some(value));

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public bool TryGetInt(out int value) => _intValue.TryGetValue(out value);

    public static implicit operator Amount7(string value) => String(value);

    public static implicit operator Amount7(int value) => Int(value);
}

file sealed class Amount7Converter : JsonConverter<Amount7>
{
    public override Amount7 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return Amount7.String(value);
        }
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt32(out var intValue))
            {
                return Amount7.Int(intValue);
            }
        }
        throw new JsonException($"JSON does not match string or int schemas: {root.ToString()}");
    }

    public override void Write(Utf8JsonWriter writer, Amount7 value, JsonSerializerOptions options)
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
            throw new JsonException($"{nameof(Amount7)} contains no valid value to serialize.");
        }
    }
}
