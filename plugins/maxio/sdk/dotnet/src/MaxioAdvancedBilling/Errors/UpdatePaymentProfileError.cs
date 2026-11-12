using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdatePaymentProfileError : ApiError
{
    private readonly Optional<ErrorStringMapResponse1> _errorStringMapResponse1Value;

    private readonly Optional<RawError> _noContentValue;

    private UpdatePaymentProfileError(Optional<ErrorStringMapResponse1> errorStringMapResponse1Value,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorStringMapResponse1Value = errorStringMapResponse1Value;
        _noContentValue = noContentValue;
    }

    private static UpdatePaymentProfileError AsErrorStringMapResponse1(ErrorStringMapResponse1 value) =>
        new(Optional<ErrorStringMapResponse1>.Some(value), default, default);

    private static UpdatePaymentProfileError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static UpdatePaymentProfileError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorStringMapResponse1([NotNullWhen(true)] out ErrorStringMapResponse1? value) =>
        _errorStringMapResponse1Value.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<ErrorStringMapResponse1, TResult> onErrorStringMapResponse1,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value) => onErrorStringMapResponse1(
                errorStringMapResponse1Value),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(UpdatePaymentProfileError)} holds no value.")
        };

    public void Match(Action<ErrorStringMapResponse1> onErrorStringMapResponse1,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetErrorStringMapResponse1(out var errorStringMapResponse1Value)) onErrorStringMapResponse1(
            errorStringMapResponse1Value);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(UpdatePaymentProfileError)} holds no value.");
    }

    private static Task<UpdatePaymentProfileError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<ErrorStringMapResponse1>().As(AsErrorStringMapResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdatePaymentProfileError> Response { get; } = new(Create);
}
