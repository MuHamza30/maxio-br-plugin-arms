using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReadBillingPortalLinkError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<TooManyManagementLinkRequestsError1> _tooManyManagementLinkRequestsError1Value;

    private ReadBillingPortalLinkError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<TooManyManagementLinkRequestsError1> tooManyManagementLinkRequestsError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
        _tooManyManagementLinkRequestsError1Value = tooManyManagementLinkRequestsError1Value;
    }

    private static ReadBillingPortalLinkError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default, default);

    private static ReadBillingPortalLinkError AsTooManyManagementLinkRequestsError1(TooManyManagementLinkRequestsError1 value) =>
        new(default, Optional<TooManyManagementLinkRequestsError1>.Some(value), default);

    private static ReadBillingPortalLinkError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1([NotNullWhen(true)] out ErrorListResponse1? value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetTooManyManagementLinkRequestsError1([NotNullWhen(true)] out TooManyManagementLinkRequestsError1? value) =>
        _tooManyManagementLinkRequestsError1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorListResponse1, TResult> onErrorListResponse1,
        Func<TooManyManagementLinkRequestsError1, TResult> onTooManyManagementLinkRequestsError1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorListResponse1(out var errorListResponse1Value) => onErrorListResponse1(
                errorListResponse1Value),
            _ when TryGetTooManyManagementLinkRequestsError1(
                out var tooManyManagementLinkRequestsError1Value) => onTooManyManagementLinkRequestsError1(
                tooManyManagementLinkRequestsError1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(ReadBillingPortalLinkError)} holds no value.")
        };

    public void Match(Action<ErrorListResponse1> onErrorListResponse1,
        Action<TooManyManagementLinkRequestsError1> onTooManyManagementLinkRequestsError1,
        Action<RawError> otherwise)
    {
        if (TryGetErrorListResponse1(out var errorListResponse1Value)) onErrorListResponse1(errorListResponse1Value);
        else if (TryGetTooManyManagementLinkRequestsError1(
            out var tooManyManagementLinkRequestsError1Value)) onTooManyManagementLinkRequestsError1(
            tooManyManagementLinkRequestsError1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(ReadBillingPortalLinkError)} holds no value.");
    }

    private static Task<ReadBillingPortalLinkError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            429 => response.Json<TooManyManagementLinkRequestsError1>().As(AsTooManyManagementLinkRequestsError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReadBillingPortalLinkError> Response { get; } = new(Create);
}
