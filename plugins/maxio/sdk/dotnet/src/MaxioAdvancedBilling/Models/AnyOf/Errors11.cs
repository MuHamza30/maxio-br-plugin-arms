using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.AnyOf;

[JsonConverter(typeof(Errors11Converter))]
public record Errors11
{
    private readonly Optional<SubscriptionGroupMembersArrayError> _subscriptionGroupMembersArrayErrorValue;

    private readonly Optional<SubscriptionGroupSingleError> _subscriptionGroupSingleErrorValue;

    private readonly Optional<string> _stringValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private Errors11(Optional<SubscriptionGroupMembersArrayError> subscriptionGroupMembersArrayErrorValue,
        Optional<SubscriptionGroupSingleError> subscriptionGroupSingleErrorValue,
        Optional<string> stringValue,
        Optional<UnknownArm> unknownValue)
    {
        _subscriptionGroupMembersArrayErrorValue = subscriptionGroupMembersArrayErrorValue;
        _subscriptionGroupSingleErrorValue = subscriptionGroupSingleErrorValue;
        _stringValue = stringValue;
        _unknownValue = unknownValue;
    }

    public static Errors11 SubscriptionGroupMembersArrayError(SubscriptionGroupMembersArrayError value) =>
        new(Optional<SubscriptionGroupMembersArrayError>.Some(value), default, default, default);

    public static Errors11 SubscriptionGroupSingleError(SubscriptionGroupSingleError value) =>
        new(default, Optional<SubscriptionGroupSingleError>.Some(value), default, default);

    public static Errors11 String(string value) => new(default, default, Optional<string>.Some(value), default);

    internal static Errors11 Unknown(JsonElement value) =>
        new(default, default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetSubscriptionGroupMembersArrayError([NotNullWhen(true)] out SubscriptionGroupMembersArrayError? value) =>
        _subscriptionGroupMembersArrayErrorValue.TryGetValue(out value);

    public bool TryGetSubscriptionGroupSingleError([NotNullWhen(true)] out SubscriptionGroupSingleError? value) =>
        _subscriptionGroupSingleErrorValue.TryGetValue(out value);

    public bool TryGetString([NotNullWhen(true)] out string? value) => _stringValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator Errors11(SubscriptionGroupMembersArrayError value) =>
        SubscriptionGroupMembersArrayError(value);

    public static implicit operator Errors11(SubscriptionGroupSingleError value) => SubscriptionGroupSingleError(value);

    public static implicit operator Errors11(string value) => String(value);

    public TResult Match<TResult>(Func<SubscriptionGroupMembersArrayError, TResult> onSubscriptionGroupMembersArrayError,
        Func<SubscriptionGroupSingleError, TResult> onSubscriptionGroupSingleError,
        Func<string, TResult> onString,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionGroupMembersArrayError(
                out var subscriptionGroupMembersArrayErrorValue) => onSubscriptionGroupMembersArrayError(
                subscriptionGroupMembersArrayErrorValue),
            _ when TryGetSubscriptionGroupSingleError(
                out var subscriptionGroupSingleErrorValue) => onSubscriptionGroupSingleError(
                subscriptionGroupSingleErrorValue),
            _ when TryGetString(out var stringValue) => onString(stringValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(Errors11)} holds no value.")
        };

    public void Match(Action<SubscriptionGroupMembersArrayError> onSubscriptionGroupMembersArrayError,
        Action<SubscriptionGroupSingleError> onSubscriptionGroupSingleError,
        Action<string> onString,
        Action<JsonElement> otherwise)
    {
        if (TryGetSubscriptionGroupMembersArrayError(
            out var subscriptionGroupMembersArrayErrorValue)) onSubscriptionGroupMembersArrayError(
            subscriptionGroupMembersArrayErrorValue);
        else if (TryGetSubscriptionGroupSingleError(
            out var subscriptionGroupSingleErrorValue)) onSubscriptionGroupSingleError(
            subscriptionGroupSingleErrorValue);
        else if (TryGetString(out var stringValue)) onString(stringValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(Errors11)} holds no value.");
    }
}

file sealed class Errors11Converter : JsonConverter<Errors11>
{
    public override Errors11 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (JsonSerializer.TryDeserialize<SubscriptionGroupMembersArrayError>(
            root,
            options,
            out var subscriptionGroupMembersArrayErrorValue))
        {
            return Errors11.SubscriptionGroupMembersArrayError(subscriptionGroupMembersArrayErrorValue);
        }
        if (JsonSerializer.TryDeserialize<SubscriptionGroupSingleError>(
            root,
            options,
            out var subscriptionGroupSingleErrorValue))
        {
            return Errors11.SubscriptionGroupSingleError(subscriptionGroupSingleErrorValue);
        }
        if (root.ValueKind == JsonValueKind.String)
        {
            var value = root.GetString()!;
            return Errors11.String(value);
        }
        return Errors11.Unknown(root);
    }

    public override void Write(Utf8JsonWriter writer, Errors11 value, JsonSerializerOptions options)
    {
        if (value.TryGetSubscriptionGroupMembersArrayError(out var subscriptionGroupMembersArrayErrorValue))
        {
            JsonSerializer.Serialize(writer, subscriptionGroupMembersArrayErrorValue, options);
        }
        else if (value.TryGetSubscriptionGroupSingleError(out var subscriptionGroupSingleErrorValue))
        {
            JsonSerializer.Serialize(writer, subscriptionGroupSingleErrorValue, options);
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
            throw new JsonException($"{nameof(Errors11)} contains no valid value to serialize.");
        }
    }
}
