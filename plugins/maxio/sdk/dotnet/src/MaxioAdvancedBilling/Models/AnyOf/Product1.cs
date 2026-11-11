using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(Product1Converter))]
public record Product1
{
    private readonly Optional<int> _intValue;

    private readonly Optional<string> _stringValue;

    private Product1(Optional<int> intValue, Optional<string> stringValue)
    {
        _intValue = intValue;
        _stringValue = stringValue;
    }

    public static Product1 Int(int value) => new(Optional<int>.Some(value), default);

    public static Product1 String(string value) => new(default, Optional<string>.Some(value));

    public bool TryGetInt(out int value) => _intValue.TryGetValue(out value);

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public static implicit operator Product1(int value) => Int(value);

    public static implicit operator Product1(string value) => String(value);
}

file sealed class Product1Converter : JsonConverter<Product1>
{
    public override Product1 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt32(out var intValue))
            {
                return Product1.Int(intValue);
            }
        }
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return Product1.String(value);
        }
        throw new JsonException($"JSON does not match int or string schemas: {root.ToString()}");
    }

    public override void Write(Utf8JsonWriter writer, Product1 value, JsonSerializerOptions options)
    {
        if (value.TryGetInt(out var intValue))
        {
            JsonSerializer.Serialize(writer, intValue, options);
        }
        else if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else
        {
            throw new JsonException($"{nameof(Product1)} contains no valid value to serialize.");
        }
    }
}
