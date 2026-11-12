using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class IssueServiceCreditError : ApiError
{
    private readonly Optional<IssueServiceCreditErrorResponse> _issueServiceCreditErrorResponseValue;

    private IssueServiceCreditError(Optional<IssueServiceCreditErrorResponse> issueServiceCreditErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _issueServiceCreditErrorResponseValue = issueServiceCreditErrorResponseValue;
    }

    private static IssueServiceCreditError AsIssueServiceCreditErrorResponse(IssueServiceCreditErrorResponse value) =>
        new(Optional<IssueServiceCreditErrorResponse>.Some(value), default);

    private static IssueServiceCreditError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetIssueServiceCreditErrorResponse([NotNullWhen(true)] out IssueServiceCreditErrorResponse? value) =>
        _issueServiceCreditErrorResponseValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<IssueServiceCreditErrorResponse, TResult> onIssueServiceCreditErrorResponse,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetIssueServiceCreditErrorResponse(
                out var issueServiceCreditErrorResponseValue) => onIssueServiceCreditErrorResponse(
                issueServiceCreditErrorResponseValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(IssueServiceCreditError)} holds no value.")
        };

    public void Match(Action<IssueServiceCreditErrorResponse> onIssueServiceCreditErrorResponse,
        Action<RawError> otherwise)
    {
        if (TryGetIssueServiceCreditErrorResponse(
            out var issueServiceCreditErrorResponseValue)) onIssueServiceCreditErrorResponse(
            issueServiceCreditErrorResponseValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(IssueServiceCreditError)} holds no value.");
    }

    private static Task<IssueServiceCreditError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<IssueServiceCreditErrorResponse>().As(AsIssueServiceCreditErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<IssueServiceCreditError> Response { get; } = new(Create);
}
