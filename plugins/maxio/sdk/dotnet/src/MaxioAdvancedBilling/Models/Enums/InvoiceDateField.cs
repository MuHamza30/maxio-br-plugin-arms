using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<InvoiceDateField>))]
public sealed record InvoiceDateField : OpenStringEnum<InvoiceDateField>
{
    private InvoiceDateField(string value) : base(value)
    {
    }

    public static readonly InvoiceDateField CreatedAt = new("created_at");

    public static readonly InvoiceDateField DueDate = new("due_date");

    public static readonly InvoiceDateField IssueDate = new("issue_date");

    public static readonly InvoiceDateField UpdatedAt = new("updated_at");

    public static readonly InvoiceDateField PaidDate = new("paid_date");

    public TResult Match<TResult>(Func<TResult> onCreatedAt,
        Func<TResult> onDueDate,
        Func<TResult> onIssueDate,
        Func<TResult> onUpdatedAt,
        Func<TResult> onPaidDate,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == DueDate => onDueDate(),
            _ when this == IssueDate => onIssueDate(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ when this == PaidDate => onPaidDate(),
            _ => otherwise(Value)
        };

    public void Match(Action onCreatedAt,
        Action onDueDate,
        Action onIssueDate,
        Action onUpdatedAt,
        Action onPaidDate,
        Action<string> otherwise)
    {
        if (this == CreatedAt) onCreatedAt();
        else if (this == DueDate) onDueDate();
        else if (this == IssueDate) onIssueDate();
        else if (this == UpdatedAt) onUpdatedAt();
        else if (this == PaidDate) onPaidDate();
        else otherwise(Value);
    }
}
