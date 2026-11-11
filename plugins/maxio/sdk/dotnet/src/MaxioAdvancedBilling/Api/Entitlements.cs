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
using MaxioAdvancedBilling.Requests.Entitlements;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Entitlements report what a subscription is allowed to do, and how much of it, based on the features granted by the subscription's products and components.
/// <para>
/// When more than one product or component grants the same feature, the values are aggregated into one entry per feature key and periodicity. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/read-subscription-entitlements-with-the-api">Read Subscription Entitlements with the API</see>.
/// </para>
/// </summary>
public sealed class Entitlements
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Entitlements(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Read Subscription Entitlements
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="AggregatedEntitlementsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadSubscriptionEntitlementsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns every feature a subscription is entitled to, collapsed into one entry per feature key and periodicity window across all products and components on the subscription. A <c>usage_limit</c> feature granted with two different periodicities comes back as two entries sharing one <c>feature_key</c>, each identified by its own <c>periodicity_key</c>.
    /// <para>
    /// When more than one product or component grants the same feature key and periodicity, the values are combined:
    /// - <b><c>access_right</c></b> features are combined with a boolean OR. If any contributor grants access, the aggregate is <c>true</c>. <c>source_products</c> only lists the contributors that granted <c>true</c>.
    /// - <b><c>usage_limit</c></b> features are summed across every contributor sharing the same periodicity window. <c>source_products</c> lists every contributor. Grants with different periodicities are not summed together. Each periodicity is returned as a separate entry.
    /// - <b><c>service_right</c></b> features are not combined: one contributor's value wins. Do not rely on which one when several grant the same feature key.
    /// </para>
    /// <para>
    /// <c>enabled</c> reflects both the aggregated value and the subscription's state. The field is <c>false</c> whenever the subscription is not in a live state (<c>active</c>, <c>trialing</c>, <c>assessing</c>, <c>past_due</c>, <c>soft_failure</c>), regardless of the aggregated value. Entitlements deliberately stay enabled through dunning.
    /// </para>
    /// </remarks>
    public Task<AggregatedEntitlementsResponse> ReadSubscriptionEntitlements(ReadSubscriptionEntitlementsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/entitlements.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<AggregatedEntitlementsResponse>(),
            ReadSubscriptionEntitlementsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
