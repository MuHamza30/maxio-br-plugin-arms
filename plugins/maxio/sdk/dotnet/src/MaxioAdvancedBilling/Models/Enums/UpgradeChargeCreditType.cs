using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of credit to be created when upgrading/downgrading. Defaults to the component and then site setting if one is not provided. Values are:
/// <para>
/// <c>full</c> - A charge is added for the full price of the component.
/// </para>
/// <para>
/// <c>prorated</c> - A charge is added for the prorated price of the component change.
/// </para>
/// <para>
/// <c>none</c> - No charge is added.
/// </para>
/// </summary>
[JsonConverter(typeof(StringEnumConverter<UpgradeChargeCreditType>))]
public sealed record UpgradeChargeCreditType : OpenStringEnum<UpgradeChargeCreditType>
{
    private UpgradeChargeCreditType(string value) : base(value)
    {
    }

    public static readonly UpgradeChargeCreditType Full = new("full");

    public static readonly UpgradeChargeCreditType Prorated = new("prorated");

    public static readonly UpgradeChargeCreditType None = new("none");

    public TResult Match<TResult>(Func<TResult> onFull,
        Func<TResult> onProrated,
        Func<TResult> onNone,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Full => onFull(),
            _ when this == Prorated => onProrated(),
            _ when this == None => onNone(),
            _ => otherwise(Value)
        };

    public void Match(Action onFull, Action onProrated, Action onNone, Action<string> otherwise)
    {
        if (this == Full) onFull();
        else if (this == Prorated) onProrated();
        else if (this == None) onNone();
        else otherwise(Value);
    }
}
