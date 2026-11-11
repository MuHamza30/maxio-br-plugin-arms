using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CreditNoteDateField>))]
public sealed record CreditNoteDateField : OpenStringEnum<CreditNoteDateField>
{
    private CreditNoteDateField(string value) : base(value)
    {
    }

    public static readonly CreditNoteDateField IssueDate = new("issue_date");

    public static readonly CreditNoteDateField AppliedDate = new("applied_date");

    public static readonly CreditNoteDateField CreatedAt = new("created_at");

    public static readonly CreditNoteDateField UpdatedAt = new("updated_at");

    public TResult Match<TResult>(Func<TResult> onIssueDate,
        Func<TResult> onAppliedDate,
        Func<TResult> onCreatedAt,
        Func<TResult> onUpdatedAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == IssueDate => onIssueDate(),
            _ when this == AppliedDate => onAppliedDate(),
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onIssueDate,
        Action onAppliedDate,
        Action onCreatedAt,
        Action onUpdatedAt,
        Action<string> otherwise)
    {
        if (this == IssueDate) onIssueDate();
        else if (this == AppliedDate) onAppliedDate();
        else if (this == CreatedAt) onCreatedAt();
        else if (this == UpdatedAt) onUpdatedAt();
        else otherwise(Value);
    }
}
