using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

/// <summary>
/// An array of segment uids to refund or the string 'all' to indicate that all segments should be refunded
/// </summary>
[JsonConverter(typeof(SegmentUidsConverter))]
public record SegmentUids
{
    private readonly Optional<IReadOnlyList<string>> _listOfStringValue;

    private readonly Optional<string> _stringValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private SegmentUids(Optional<IReadOnlyList<string>> listOfStringValue,
        Optional<string> stringValue,
        Optional<UnknownArm> unknownValue)
    {
        _listOfStringValue = listOfStringValue;
        _stringValue = stringValue;
        _unknownValue = unknownValue;
    }

    public static SegmentUids ListOfString(IReadOnlyList<string> value) =>
        new(Optional<IReadOnlyList<string>>.Some(value), default, default);

    public static SegmentUids String(string value) => new(default, Optional<string>.Some(value), default);

    internal static SegmentUids Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetListOfString([NotNullWhen(true)] out IReadOnlyList<string>? value) =>
        _listOfStringValue.TryGetValue(out value);

    public bool TryGetString([NotNullWhen(true)] out string? value) => _stringValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator SegmentUids(string value) => String(value);

    public TResult Match<TResult>(Func<IReadOnlyList<string>, TResult> onListOfString,
        Func<string, TResult> onString,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetListOfString(out var listOfStringValue) => onListOfString(listOfStringValue),
            _ when TryGetString(out var stringValue) => onString(stringValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(SegmentUids)} holds no value.")
        };

    public void Match(Action<IReadOnlyList<string>> onListOfString,
        Action<string> onString,
        Action<JsonElement> otherwise)
    {
        if (TryGetListOfString(out var listOfStringValue)) onListOfString(listOfStringValue);
        else if (TryGetString(out var stringValue)) onString(stringValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(SegmentUids)} holds no value.");
    }
}

file sealed class SegmentUidsConverter : JsonConverter<SegmentUids>
{
    public override SegmentUids Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<IReadOnlyList<string>>(root, options, out var listOfStringValue))
        {
            return SegmentUids.ListOfString(listOfStringValue);
        }
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return SegmentUids.String(value);
        }
        return SegmentUids.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, SegmentUids value, JsonSerializerOptions options)
    {
        if (value.TryGetListOfString(out var listOfStringValue))
        {
            JsonSerializer.Serialize(writer, listOfStringValue, options);
        }
        else if (value.TryGetString(out var stringValue))
        {
            JsonSerializer.Serialize(writer, stringValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(SegmentUids)} contains no valid value to serialize.");
        }
    }
}
