using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Defaults to personal
/// </summary>
[JsonConverter(typeof(StringEnumConverter<BankAccountHolderType>))]
public sealed record BankAccountHolderType : OpenStringEnum<BankAccountHolderType>
{
    private BankAccountHolderType(string value) : base(value)
    {
    }

    public static readonly BankAccountHolderType Personal = new("personal");

    public static readonly BankAccountHolderType Business = new("business");

    public TResult Match<TResult>(Func<TResult> onPersonal,
        Func<TResult> onBusiness,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Personal => onPersonal(),
            _ when this == Business => onBusiness(),
            _ => otherwise(Value)
        };

    public void Match(Action onPersonal, Action onBusiness, Action<string> otherwise)
    {
        if (this == Personal) onPersonal();
        else if (this == Business) onBusiness();
        else otherwise(Value);
    }
}
