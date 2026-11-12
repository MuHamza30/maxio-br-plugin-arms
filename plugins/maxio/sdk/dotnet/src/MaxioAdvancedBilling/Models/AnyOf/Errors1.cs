using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(Errors1Converter))]
public record Errors1
{
    private readonly Optional<CustomerError> _customerErrorValue;

    private readonly Optional<IReadOnlyList<string>> _listOfStringValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private Errors1(Optional<CustomerError> customerErrorValue,
        Optional<IReadOnlyList<string>> listOfStringValue,
        Optional<UnknownArm> unknownValue)
    {
        _customerErrorValue = customerErrorValue;
        _listOfStringValue = listOfStringValue;
        _unknownValue = unknownValue;
    }

    public static Errors1 CustomerError(CustomerError value) =>
        new(Optional<CustomerError>.Some(value), default, default);

    public static Errors1 ListOfString(IReadOnlyList<string> value) =>
        new(default, Optional<IReadOnlyList<string>>.Some(value), default);

    internal static Errors1 Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetCustomerError([NotNullWhen(true)] out CustomerError? value) =>
        _customerErrorValue.TryGetValue(out value);

    public bool TryGetListOfString([NotNullWhen(true)] out IReadOnlyList<string>? value) =>
        _listOfStringValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator Errors1(CustomerError value) => CustomerError(value);

    public TResult Match<TResult>(Func<CustomerError, TResult> onCustomerError,
        Func<IReadOnlyList<string>, TResult> onListOfString,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCustomerError(out var customerErrorValue) => onCustomerError(customerErrorValue),
            _ when TryGetListOfString(out var listOfStringValue) => onListOfString(listOfStringValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(Errors1)} holds no value.")
        };

    public void Match(Action<CustomerError> onCustomerError,
        Action<IReadOnlyList<string>> onListOfString,
        Action<JsonElement> otherwise)
    {
        if (TryGetCustomerError(out var customerErrorValue)) onCustomerError(customerErrorValue);
        else if (TryGetListOfString(out var listOfStringValue)) onListOfString(listOfStringValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(Errors1)} holds no value.");
    }
}

file sealed class Errors1Converter : JsonConverter<Errors1>
{
    public override Errors1 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<CustomerError>(root, options, out var customerErrorValue))
        {
            return Errors1.CustomerError(customerErrorValue);
        }
        if (JsonSerializer.TryDeserialize<IReadOnlyList<string>>(root, options, out var listOfStringValue))
        {
            return Errors1.ListOfString(listOfStringValue);
        }
        return Errors1.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, Errors1 value, JsonSerializerOptions options)
    {
        if (value.TryGetCustomerError(out var customerErrorValue))
        {
            JsonSerializer.Serialize(writer, customerErrorValue, options);
        }
        else if (value.TryGetListOfString(out var listOfStringValue))
        {
            JsonSerializer.Serialize(writer, listOfStringValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(Errors1)} contains no valid value to serialize.");
        }
    }
}
