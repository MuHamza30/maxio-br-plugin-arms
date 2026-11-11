using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// (Optional) A free opening balance, not a purchase and not reflected on any invoice. When non-zero, wallet creation also writes an initial-balance grant and a matching <c>initial_balance</c> ledger row. Omitted or exactly <c>0</c> creates a zero-balance wallet, not an error. Encoded as a decimal string with at most 8 fractional digits, and must be non-negative and below 10^16. Send fractional values as strings: a JSON number with a decimal point is rejected with <c>422</c>, because a double-precision float may already have lost precision before the request arrives.
/// </summary>
[JsonConverter(typeof(InitialBalanceConverter))]
public record InitialBalance
{
    private readonly Optional<string> _stringValue;

    private readonly Optional<int> _intValue;

    private InitialBalance(Optional<string> stringValue, Optional<int> intValue)
    {
        _stringValue = stringValue;
        _intValue = intValue;
    }

    public static InitialBalance String(string value) => new(Optional<string>.Some(value), default);

    public static InitialBalance Int(int value) => new(default, Optional<int>.Some(value));

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public bool TryGetInt(out int value) => _intValue.TryGetValue(out value);

    public static implicit operator InitialBalance(string value) => String(value);

    public static implicit operator InitialBalance(int value) => Int(value);
}

file sealed class InitialBalanceConverter : JsonConverter<InitialBalance>
{
    public override InitialBalance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return InitialBalance.String(value);
        }
        if (root.ValueKind == JsonValueKind.Number)
        {
            if (root.TryGetInt32(out var intValue))
            {
                return InitialBalance.Int(intValue);
            }
        }
        throw new JsonException($"JSON does not match string or int schemas: {root.ToString()}");
    }

    public override void Write(Utf8JsonWriter writer, InitialBalance value, JsonSerializerOptions options)
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
            throw new JsonException($"{nameof(InitialBalance)} contains no valid value to serialize.");
        }
    }
}
