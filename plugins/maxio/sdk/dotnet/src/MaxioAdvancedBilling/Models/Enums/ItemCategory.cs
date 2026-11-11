using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// One of the following: Business Software, Consumer Software, Digital Services, Physical Goods, Other
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ItemCategory>))]
public sealed record ItemCategory : OpenStringEnum<ItemCategory>
{
    private ItemCategory(string value) : base(value)
    {
    }

    public static readonly ItemCategory BusinessSoftware = new("Business Software");

    public static readonly ItemCategory ConsumerSoftware = new("Consumer Software");

    public static readonly ItemCategory DigitalServices = new("Digital Services");

    public static readonly ItemCategory PhysicalGoods = new("Physical Goods");

    public static readonly ItemCategory Other = new("Other");

    public TResult Match<TResult>(Func<TResult> onBusinessSoftware,
        Func<TResult> onConsumerSoftware,
        Func<TResult> onDigitalServices,
        Func<TResult> onPhysicalGoods,
        Func<TResult> onOther,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == BusinessSoftware => onBusinessSoftware(),
            _ when this == ConsumerSoftware => onConsumerSoftware(),
            _ when this == DigitalServices => onDigitalServices(),
            _ when this == PhysicalGoods => onPhysicalGoods(),
            _ when this == Other => onOther(),
            _ => otherwise(Value)
        };

    public void Match(Action onBusinessSoftware,
        Action onConsumerSoftware,
        Action onDigitalServices,
        Action onPhysicalGoods,
        Action onOther,
        Action<string> otherwise)
    {
        if (this == BusinessSoftware) onBusinessSoftware();
        else if (this == ConsumerSoftware) onConsumerSoftware();
        else if (this == DigitalServices) onDigitalServices();
        else if (this == PhysicalGoods) onPhysicalGoods();
        else if (this == Other) onOther();
        else otherwise(Value);
    }
}
