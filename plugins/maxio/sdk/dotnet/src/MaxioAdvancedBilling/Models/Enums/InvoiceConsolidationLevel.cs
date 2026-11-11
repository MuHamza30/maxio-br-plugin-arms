using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Consolidation level of the invoice, which is applicable to invoice consolidation. It will hold one of the following values:
/// <list type="bullet">
///   <item><description>"none": A normal invoice with no consolidation.</description></item>
///   <item><description>"child": An invoice segment which has been combined into a consolidated invoice.</description></item>
///   <item><description>"parent": A consolidated invoice, whose contents are composed of invoice segments.</description></item>
/// </list>
/// <para>
/// "Parent" invoices do not have lines of their own, but they have subtotals and totals which aggregate the member invoice segments.
/// </para>
/// <para>
/// See also the <see href="https://maxio.zendesk.com/hc/en-us/articles/24252269909389-Invoice-Consolidation">invoice consolidation documentation</see>.
/// </para>
/// </summary>
[JsonConverter(typeof(StringEnumConverter<InvoiceConsolidationLevel>))]
public sealed record InvoiceConsolidationLevel : OpenStringEnum<InvoiceConsolidationLevel>
{
    private InvoiceConsolidationLevel(string value) : base(value)
    {
    }

    public static readonly InvoiceConsolidationLevel None = new("none");

    public static readonly InvoiceConsolidationLevel Child = new("child");

    public static readonly InvoiceConsolidationLevel Parent = new("parent");

    public TResult Match<TResult>(Func<TResult> onNone,
        Func<TResult> onChild,
        Func<TResult> onParent,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == None => onNone(),
            _ when this == Child => onChild(),
            _ when this == Parent => onParent(),
            _ => otherwise(Value)
        };

    public void Match(Action onNone, Action onChild, Action onParent, Action<string> otherwise)
    {
        if (this == None) onNone();
        else if (this == Child) onChild();
        else if (this == Parent) onParent();
        else otherwise(Value);
    }
}
