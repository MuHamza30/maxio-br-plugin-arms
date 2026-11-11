using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionGroupsListInclude>))]
public sealed record SubscriptionGroupsListInclude : OpenStringEnum<SubscriptionGroupsListInclude>
{
    private SubscriptionGroupsListInclude(string value) : base(value)
    {
    }

    public static readonly SubscriptionGroupsListInclude AccountBalances = new("account_balances");

    public TResult Match<TResult>(Func<TResult> onAccountBalances, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == AccountBalances => onAccountBalances(),
            _ => otherwise(Value)
        };

    public void Match(Action onAccountBalances, Action<string> otherwise)
    {
        if (this == AccountBalances) onAccountBalances();
        else otherwise(Value);
    }
}
