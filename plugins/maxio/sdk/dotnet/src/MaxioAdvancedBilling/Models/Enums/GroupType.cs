using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<GroupType>))]
public sealed record GroupType : OpenStringEnum<GroupType>
{
    private GroupType(string value) : base(value)
    {
    }

    public static readonly GroupType SingleCustomer = new("single_customer");

    public static readonly GroupType MultipleCustomers = new("multiple_customers");

    public TResult Match<TResult>(Func<TResult> onSingleCustomer,
        Func<TResult> onMultipleCustomers,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == SingleCustomer => onSingleCustomer(),
            _ when this == MultipleCustomers => onMultipleCustomers(),
            _ => otherwise(Value)
        };

    public void Match(Action onSingleCustomer, Action onMultipleCustomers, Action<string> otherwise)
    {
        if (this == SingleCustomer) onSingleCustomer();
        else if (this == MultipleCustomers) onMultipleCustomers();
        else otherwise(Value);
    }
}
