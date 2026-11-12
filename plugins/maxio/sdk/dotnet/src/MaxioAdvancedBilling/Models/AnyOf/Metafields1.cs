using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(Metafields1Converter))]
public record Metafields1
{
    private readonly Optional<UpdateMetafield> _updateMetafieldValue;

    private readonly Optional<IReadOnlyList<UpdateMetafield>> _listOfUpdateMetafieldValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private Metafields1(Optional<UpdateMetafield> updateMetafieldValue,
        Optional<IReadOnlyList<UpdateMetafield>> listOfUpdateMetafieldValue,
        Optional<UnknownArm> unknownValue)
    {
        _updateMetafieldValue = updateMetafieldValue;
        _listOfUpdateMetafieldValue = listOfUpdateMetafieldValue;
        _unknownValue = unknownValue;
    }

    public static Metafields1 UpdateMetafield(UpdateMetafield value) =>
        new(Optional<UpdateMetafield>.Some(value), default, default);

    public static Metafields1 ListOfUpdateMetafield(IReadOnlyList<UpdateMetafield> value) =>
        new(default, Optional<IReadOnlyList<UpdateMetafield>>.Some(value), default);

    internal static Metafields1 Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetUpdateMetafield([NotNullWhen(true)] out UpdateMetafield? value) =>
        _updateMetafieldValue.TryGetValue(out value);

    public bool TryGetListOfUpdateMetafield([NotNullWhen(true)] out IReadOnlyList<UpdateMetafield>? value) =>
        _listOfUpdateMetafieldValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator Metafields1(UpdateMetafield value) => UpdateMetafield(value);

    public TResult Match<TResult>(Func<UpdateMetafield, TResult> onUpdateMetafield,
        Func<IReadOnlyList<UpdateMetafield>, TResult> onListOfUpdateMetafield,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetUpdateMetafield(out var updateMetafieldValue) => onUpdateMetafield(updateMetafieldValue),
            _ when TryGetListOfUpdateMetafield(out var listOfUpdateMetafieldValue) => onListOfUpdateMetafield(
                listOfUpdateMetafieldValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(Metafields1)} holds no value.")
        };

    public void Match(Action<UpdateMetafield> onUpdateMetafield,
        Action<IReadOnlyList<UpdateMetafield>> onListOfUpdateMetafield,
        Action<JsonElement> otherwise)
    {
        if (TryGetUpdateMetafield(out var updateMetafieldValue)) onUpdateMetafield(updateMetafieldValue);
        else if (TryGetListOfUpdateMetafield(out var listOfUpdateMetafieldValue)) onListOfUpdateMetafield(
            listOfUpdateMetafieldValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(Metafields1)} holds no value.");
    }
}

file sealed class Metafields1Converter : JsonConverter<Metafields1>
{
    public override Metafields1 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<UpdateMetafield>(root, options, out var updateMetafieldValue))
        {
            return Metafields1.UpdateMetafield(updateMetafieldValue);
        }
        if (JsonSerializer.TryDeserialize<IReadOnlyList<UpdateMetafield>>(
            root,
            options,
            out var listOfUpdateMetafieldValue))
        {
            return Metafields1.ListOfUpdateMetafield(listOfUpdateMetafieldValue);
        }
        return Metafields1.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, Metafields1 value, JsonSerializerOptions options)
    {
        if (value.TryGetUpdateMetafield(out var updateMetafieldValue))
        {
            JsonSerializer.Serialize(writer, updateMetafieldValue, options);
        }
        else if (value.TryGetListOfUpdateMetafield(out var listOfUpdateMetafieldValue))
        {
            JsonSerializer.Serialize(writer, listOfUpdateMetafieldValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(Metafields1)} contains no valid value to serialize.");
        }
    }
}
