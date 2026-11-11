using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.ReferralCodes;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Referral Code object represents a code attached to a subscription, used to track and validate referrals as part of a referral program. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/understand-referrals">Understand Referrals</see>.
/// </summary>
public sealed class ReferralCodes
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ReferralCodes(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Validate Referral Code
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ReferralValidationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ValidateReferralCodeError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Validates whether a referral code is valid and applicable within your site. This method is useful for validating referral codes that are entered by a customer.
    /// <para>
    /// For more information, see <see href="https://docs.maxio.com/hc/en-us/articles/24286981223693-Understanding-Referrals">Understanding Referrals</see> in the product documentation.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The referral code is not valid or not applicable within the site → Maxio answers 404 with "Referral code is invalid.". Treat 404 as an invalid code and do not pass it as ref when creating the subscription.</description></item>
    /// </list>
    /// </remarks>
    public Task<ReferralValidationResponse> ValidateReferralCode(ValidateReferralCodeRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/referral_codes/validate.json"),
            [],
            [new Param("code", request.Code)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ReferralValidationResponse>(),
            ValidateReferralCodeError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
