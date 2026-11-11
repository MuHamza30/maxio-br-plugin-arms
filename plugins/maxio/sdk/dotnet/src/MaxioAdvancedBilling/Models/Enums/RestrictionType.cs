using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<RestrictionType>))]
public sealed record RestrictionType : OpenStringEnum<RestrictionType>
{
    private RestrictionType(string value) : base(value)
    {
    }

    public static readonly RestrictionType Component = new("Component");

    public static readonly RestrictionType Product = new("Product");

    public TResult Match<TResult>(Func<TResult> onComponent,
        Func<TResult> onProduct,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Component => onComponent(),
            _ when this == Product => onProduct(),
            _ => otherwise(Value)
        };

    public void Match(Action onComponent, Action onProduct, Action<string> otherwise)
    {
        if (this == Component) onComponent();
        else if (this == Product) onProduct();
        else otherwise(Value);
    }
}
