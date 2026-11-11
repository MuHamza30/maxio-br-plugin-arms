using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ProformaInvoiceDiscountSourceType>))]
public sealed record ProformaInvoiceDiscountSourceType : OpenStringEnum<ProformaInvoiceDiscountSourceType>
{
    private ProformaInvoiceDiscountSourceType(string value) : base(value)
    {
    }

    public static readonly ProformaInvoiceDiscountSourceType Coupon = new("Coupon");

    public static readonly ProformaInvoiceDiscountSourceType Referral = new("Referral");

    public TResult Match<TResult>(Func<TResult> onCoupon, Func<TResult> onReferral, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Coupon => onCoupon(),
            _ when this == Referral => onReferral(),
            _ => otherwise(Value)
        };

    public void Match(Action onCoupon, Action onReferral, Action<string> otherwise)
    {
        if (this == Coupon) onCoupon();
        else if (this == Referral) onReferral();
        else otherwise(Value);
    }
}
