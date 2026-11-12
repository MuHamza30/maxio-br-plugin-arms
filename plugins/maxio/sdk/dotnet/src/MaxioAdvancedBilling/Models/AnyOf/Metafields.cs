using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(MetafieldsConverter))]
public record Metafields
{
    private readonly Optional<CreateMetafield> _createMetafieldValue;

    private readonly Optional<IReadOnlyList<CreateMetafield>> _listOfCreateMetafieldValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private Metafields(Optional<CreateMetafield> createMetafieldValue,
        Optional<IReadOnlyList<CreateMetafield>> listOfCreateMetafieldValue,
        Optional<UnknownArm> unknownValue)
    {
        _createMetafieldValue = createMetafieldValue;
        _listOfCreateMetafieldValue = listOfCreateMetafieldValue;
        _unknownValue = unknownValue;
    }

    public static Metafields CreateMetafield(CreateMetafield value) =>
        new(Optional<CreateMetafield>.Some(value), default, default);

    public static Metafields ListOfCreateMetafield(IReadOnlyList<CreateMetafield> value) =>
        new(default, Optional<IReadOnlyList<CreateMetafield>>.Some(value), default);

    internal static Metafields Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetCreateMetafield([NotNullWhen(true)] out CreateMetafield? value) =>
        _createMetafieldValue.TryGetValue(out value);

    public bool TryGetListOfCreateMetafield([NotNullWhen(true)] out IReadOnlyList<CreateMetafield>? value) =>
        _listOfCreateMetafieldValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator Metafields(CreateMetafield value) => CreateMetafield(value);

    public TResult Match<TResult>(Func<CreateMetafield, TResult> onCreateMetafield,
        Func<IReadOnlyList<CreateMetafield>, TResult> onListOfCreateMetafield,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCreateMetafield(out var createMetafieldValue) => onCreateMetafield(createMetafieldValue),
            _ when TryGetListOfCreateMetafield(out var listOfCreateMetafieldValue) => onListOfCreateMetafield(
                listOfCreateMetafieldValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(Metafields)} holds no value.")
        };

    public void Match(Action<CreateMetafield> onCreateMetafield,
        Action<IReadOnlyList<CreateMetafield>> onListOfCreateMetafield,
        Action<JsonElement> otherwise)
    {
        if (TryGetCreateMetafield(out var createMetafieldValue)) onCreateMetafield(createMetafieldValue);
        else if (TryGetListOfCreateMetafield(out var listOfCreateMetafieldValue)) onListOfCreateMetafield(
            listOfCreateMetafieldValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(Metafields)} holds no value.");
    }
}

file sealed class MetafieldsConverter : JsonConverter<Metafields>
{
    public override Metafields Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<CreateMetafield>(root, options, out var createMetafieldValue))
        {
            return Metafields.CreateMetafield(createMetafieldValue);
        }
        if (JsonSerializer.TryDeserialize<IReadOnlyList<CreateMetafield>>(
            root,
            options,
            out var listOfCreateMetafieldValue))
        {
            return Metafields.ListOfCreateMetafield(listOfCreateMetafieldValue);
        }
        return Metafields.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, Metafields value, JsonSerializerOptions options)
    {
        if (value.TryGetCreateMetafield(out var createMetafieldValue))
        {
            JsonSerializer.Serialize(writer, createMetafieldValue, options);
        }
        else if (value.TryGetListOfCreateMetafield(out var listOfCreateMetafieldValue))
        {
            JsonSerializer.Serialize(writer, listOfCreateMetafieldValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(Metafields)} contains no valid value to serialize.");
        }
    }
}
