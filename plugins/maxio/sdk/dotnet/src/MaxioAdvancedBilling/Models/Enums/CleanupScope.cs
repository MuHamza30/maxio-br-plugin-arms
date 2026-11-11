using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// all: Will clear all products, customers, and related subscriptions from the site. customers: Will clear only customers and related subscriptions (leaving the products untouched) for the site. Revenue will also be reset to 0.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CleanupScope>))]
public sealed record CleanupScope : OpenStringEnum<CleanupScope>
{
    private CleanupScope(string value) : base(value)
    {
    }

    public static readonly CleanupScope All = new("all");

    public static readonly CleanupScope Customers = new("customers");

    public TResult Match<TResult>(Func<TResult> onAll, Func<TResult> onCustomers, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == All => onAll(),
            _ when this == Customers => onCustomers(),
            _ => otherwise(Value)
        };

    public void Match(Action onAll, Action onCustomers, Action<string> otherwise)
    {
        if (this == All) onAll();
        else if (this == Customers) onCustomers();
        else otherwise(Value);
    }
}
