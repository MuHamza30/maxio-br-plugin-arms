using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Servers;

[JsonConverter(typeof(StringEnumConverter<ServerEnvironment>))]
public sealed record ServerEnvironment : ClosedStringEnum<ServerEnvironment>
{
    private ServerEnvironment(string value) : base(value)
    {
    }

    /// <summary>
    /// Default Advanced Billing environment hosted in US. Valid for the majority of our customers.
    /// </summary>
    public static readonly ServerEnvironment Us = new("US");

    /// <summary>
    /// Advanced Billing environment hosted in EU. Use only when you requested EU hosting for your AB account.
    /// </summary>
    public static readonly ServerEnvironment Eu = new("EU");

    public static ServerEnvironment Default() => Us;

    internal TResult Match<TResult>(Func<TResult> onUs, Func<TResult> onEu) =>
        this switch
        {
            _ when this == Us => onUs(),
            _ when this == Eu => onEu(),
            _ => throw new InvalidOperationException($"{nameof(ServerEnvironment)} holds no known value.")
        };
}
