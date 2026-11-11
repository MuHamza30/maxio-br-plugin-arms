using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The kind of tax or business identifier held by the customer:
/// - <c>vat_eu</c>: an EU VAT number. Requires <c>vat_country</c> to be an EU member state code or <c>GB</c>.
/// - <c>national_tax</c>: a national tax ID registered outside the EU. Requires <c>vat_country</c> to be one of <c>AL</c>, <c>AM</c>, <c>AR</c>, <c>AU</c>, <c>BR</c>, <c>CA</c>, <c>CH</c>, <c>DZ</c>, <c>IN</c>, <c>MX</c>, <c>NO</c>, <c>NZ</c>, or <c>ZA</c>.
/// - <c>company_reg</c>: a company registration number, such as a French SIREN. No <c>vat_country</c> is required.
/// - <c>gln</c>: a Global Location Number. The value must be 13 digits.
/// - <c>duns</c>: a D-U-N-S Number. The value must be 9 digits.
/// - <c>lei</c>: a Legal Entity Identifier. The value must be 20 characters: 18 letters or digits followed by 2 digits.
/// <para>
/// A customer holds one identifier at a time. Saving an identifier of a different kind replaces the existing one.
/// </para>
/// </summary>
[JsonConverter(typeof(StringEnumConverter<EntityIdentifierKind>))]
public sealed record EntityIdentifierKind : OpenStringEnum<EntityIdentifierKind>
{
    private EntityIdentifierKind(string value) : base(value)
    {
    }

    public static readonly EntityIdentifierKind VatEu = new("vat_eu");

    public static readonly EntityIdentifierKind NationalTax = new("national_tax");

    public static readonly EntityIdentifierKind CompanyReg = new("company_reg");

    public static readonly EntityIdentifierKind Gln = new("gln");

    public static readonly EntityIdentifierKind Duns = new("duns");

    public static readonly EntityIdentifierKind Lei = new("lei");

    public TResult Match<TResult>(Func<TResult> onVatEu,
        Func<TResult> onNationalTax,
        Func<TResult> onCompanyReg,
        Func<TResult> onGln,
        Func<TResult> onDuns,
        Func<TResult> onLei,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == VatEu => onVatEu(),
            _ when this == NationalTax => onNationalTax(),
            _ when this == CompanyReg => onCompanyReg(),
            _ when this == Gln => onGln(),
            _ when this == Duns => onDuns(),
            _ when this == Lei => onLei(),
            _ => otherwise(Value)
        };

    public void Match(Action onVatEu,
        Action onNationalTax,
        Action onCompanyReg,
        Action onGln,
        Action onDuns,
        Action onLei,
        Action<string> otherwise)
    {
        if (this == VatEu) onVatEu();
        else if (this == NationalTax) onNationalTax();
        else if (this == CompanyReg) onCompanyReg();
        else if (this == Gln) onGln();
        else if (this == Duns) onDuns();
        else if (this == Lei) onLei();
        else otherwise(Value);
    }
}
