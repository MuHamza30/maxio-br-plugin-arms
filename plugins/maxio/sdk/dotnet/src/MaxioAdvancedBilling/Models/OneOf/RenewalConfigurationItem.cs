using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

[JsonConverter(typeof(RenewalConfigurationItemConverter))]
public record RenewalConfigurationItem
{
    private readonly Optional<ScheduledRenewalItemRequestBodyComponent> _scheduledRenewalItemRequestBodyComponentValue;

    private readonly Optional<ScheduledRenewalItemRequestBodyProduct> _scheduledRenewalItemRequestBodyProductValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private RenewalConfigurationItem(Optional<ScheduledRenewalItemRequestBodyComponent> scheduledRenewalItemRequestBodyComponentValue,
        Optional<ScheduledRenewalItemRequestBodyProduct> scheduledRenewalItemRequestBodyProductValue,
        Optional<UnknownArm> unknownValue)
    {
        _scheduledRenewalItemRequestBodyComponentValue = scheduledRenewalItemRequestBodyComponentValue;
        _scheduledRenewalItemRequestBodyProductValue = scheduledRenewalItemRequestBodyProductValue;
        _unknownValue = unknownValue;
    }

    public static RenewalConfigurationItem ScheduledRenewalItemRequestBodyComponent(ScheduledRenewalItemRequestBodyComponent value) =>
        new(Optional<ScheduledRenewalItemRequestBodyComponent>.Some(value), default, default);

    public static RenewalConfigurationItem ScheduledRenewalItemRequestBodyProduct(ScheduledRenewalItemRequestBodyProduct value) =>
        new(default, Optional<ScheduledRenewalItemRequestBodyProduct>.Some(value), default);

    internal static RenewalConfigurationItem Unknown(JsonElement value) =>
        new(default, default, Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetScheduledRenewalItemRequestBodyComponent([NotNullWhen(true)] out ScheduledRenewalItemRequestBodyComponent? value) =>
        _scheduledRenewalItemRequestBodyComponentValue.TryGetValue(out value);

    public bool TryGetScheduledRenewalItemRequestBodyProduct([NotNullWhen(true)] out ScheduledRenewalItemRequestBodyProduct? value) =>
        _scheduledRenewalItemRequestBodyProductValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator RenewalConfigurationItem(ScheduledRenewalItemRequestBodyComponent value) =>
        ScheduledRenewalItemRequestBodyComponent(value);

    public static implicit operator RenewalConfigurationItem(ScheduledRenewalItemRequestBodyProduct value) =>
        ScheduledRenewalItemRequestBodyProduct(value);

    public TResult Match<TResult>(Func<ScheduledRenewalItemRequestBodyComponent, TResult> onScheduledRenewalItemRequestBodyComponent,
        Func<ScheduledRenewalItemRequestBodyProduct, TResult> onScheduledRenewalItemRequestBodyProduct,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetScheduledRenewalItemRequestBodyComponent(
                out var scheduledRenewalItemRequestBodyComponentValue) => onScheduledRenewalItemRequestBodyComponent(
                scheduledRenewalItemRequestBodyComponentValue),
            _ when TryGetScheduledRenewalItemRequestBodyProduct(
                out var scheduledRenewalItemRequestBodyProductValue) => onScheduledRenewalItemRequestBodyProduct(
                scheduledRenewalItemRequestBodyProductValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(RenewalConfigurationItem)} holds no value.")
        };

    public void Match(Action<ScheduledRenewalItemRequestBodyComponent> onScheduledRenewalItemRequestBodyComponent,
        Action<ScheduledRenewalItemRequestBodyProduct> onScheduledRenewalItemRequestBodyProduct,
        Action<JsonElement> otherwise)
    {
        if (TryGetScheduledRenewalItemRequestBodyComponent(
            out var scheduledRenewalItemRequestBodyComponentValue)) onScheduledRenewalItemRequestBodyComponent(
            scheduledRenewalItemRequestBodyComponentValue);
        else if (TryGetScheduledRenewalItemRequestBodyProduct(
            out var scheduledRenewalItemRequestBodyProductValue)) onScheduledRenewalItemRequestBodyProduct(
            scheduledRenewalItemRequestBodyProductValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(RenewalConfigurationItem)} holds no value.");
    }
}

file sealed class RenewalConfigurationItemConverter : JsonConverter<RenewalConfigurationItem>
{
    public override RenewalConfigurationItem Read(ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (!root.TryGetProperty("item_type", out var typeProperty) || typeProperty.ValueKind != JsonValueKind.String)
        {
            throw new JsonException("Missing or non-string 'item_type' discriminator field");
        }
        return typeProperty.GetString() switch
        {
            "Component" => RenewalConfigurationItem.ScheduledRenewalItemRequestBodyComponent(
                root.Deserialize<ScheduledRenewalItemRequestBodyComponent>(options)!),
            "Product" => RenewalConfigurationItem.ScheduledRenewalItemRequestBodyProduct(
                root.Deserialize<ScheduledRenewalItemRequestBodyProduct>(options)!),
            _ => RenewalConfigurationItem.Unknown(root)
        };
    }

    public override void Write(Utf8JsonWriter writer, RenewalConfigurationItem value, JsonSerializerOptions options)
    {
        if (value.TryGetScheduledRenewalItemRequestBodyComponent(out var scheduledRenewalItemRequestBodyComponentValue))
        {
            JsonSerializer.Serialize(writer, scheduledRenewalItemRequestBodyComponentValue, options);
        }
        else if (value.TryGetScheduledRenewalItemRequestBodyProduct(
            out var scheduledRenewalItemRequestBodyProductValue))
        {
            JsonSerializer.Serialize(writer, scheduledRenewalItemRequestBodyProductValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(RenewalConfigurationItem)} contains no valid value to serialize.");
        }
    }
}
