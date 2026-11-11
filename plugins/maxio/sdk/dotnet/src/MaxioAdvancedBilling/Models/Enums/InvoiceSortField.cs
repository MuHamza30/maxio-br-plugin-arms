using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<InvoiceSortField>))]
public sealed record InvoiceSortField : OpenStringEnum<InvoiceSortField>
{
    private InvoiceSortField(string value) : base(value)
    {
    }

    public static readonly InvoiceSortField Status = new("status");

    public static readonly InvoiceSortField TotalAmount = new("total_amount");

    public static readonly InvoiceSortField DueAmount = new("due_amount");

    public static readonly InvoiceSortField CreatedAt = new("created_at");

    public static readonly InvoiceSortField UpdatedAt = new("updated_at");

    public static readonly InvoiceSortField IssueDate = new("issue_date");

    public static readonly InvoiceSortField DueDate = new("due_date");

    public static readonly InvoiceSortField Number = new("number");

    public TResult Match<TResult>(Func<TResult> onStatus,
        Func<TResult> onTotalAmount,
        Func<TResult> onDueAmount,
        Func<TResult> onCreatedAt,
        Func<TResult> onUpdatedAt,
        Func<TResult> onIssueDate,
        Func<TResult> onDueDate,
        Func<TResult> onNumber,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Status => onStatus(),
            _ when this == TotalAmount => onTotalAmount(),
            _ when this == DueAmount => onDueAmount(),
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ when this == IssueDate => onIssueDate(),
            _ when this == DueDate => onDueDate(),
            _ when this == Number => onNumber(),
            _ => otherwise(Value)
        };

    public void Match(Action onStatus,
        Action onTotalAmount,
        Action onDueAmount,
        Action onCreatedAt,
        Action onUpdatedAt,
        Action onIssueDate,
        Action onDueDate,
        Action onNumber,
        Action<string> otherwise)
    {
        if (this == Status) onStatus();
        else if (this == TotalAmount) onTotalAmount();
        else if (this == DueAmount) onDueAmount();
        else if (this == CreatedAt) onCreatedAt();
        else if (this == UpdatedAt) onUpdatedAt();
        else if (this == IssueDate) onIssueDate();
        else if (this == DueDate) onDueDate();
        else if (this == Number) onNumber();
        else otherwise(Value);
    }
}
