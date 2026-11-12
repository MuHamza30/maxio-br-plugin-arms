using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ValidateReferralCodeError : ApiError
{
    private readonly Optional<SingleStringErrorResponse1> _singleStringErrorResponse1Value;

    private ValidateReferralCodeError(Optional<SingleStringErrorResponse1> singleStringErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _singleStringErrorResponse1Value = singleStringErrorResponse1Value;
    }

    private static ValidateReferralCodeError AsSingleStringErrorResponse1(SingleStringErrorResponse1 value) =>
        new(Optional<SingleStringErrorResponse1>.Some(value), default);

    private static ValidateReferralCodeError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSingleStringErrorResponse1([NotNullWhen(true)] out SingleStringErrorResponse1? value) =>
        _singleStringErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SingleStringErrorResponse1, TResult> onSingleStringErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSingleStringErrorResponse1(
                out var singleStringErrorResponse1Value) => onSingleStringErrorResponse1(
                singleStringErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(ValidateReferralCodeError)} holds no value.")
        };

    public void Match(Action<SingleStringErrorResponse1> onSingleStringErrorResponse1, Action<RawError> otherwise)
    {
        if (TryGetSingleStringErrorResponse1(out var singleStringErrorResponse1Value)) onSingleStringErrorResponse1(
            singleStringErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(ValidateReferralCodeError)} holds no value.");
    }

    private static Task<ValidateReferralCodeError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.Json<SingleStringErrorResponse1>().As(AsSingleStringErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ValidateReferralCodeError> Response { get; } = new(Create);
}
