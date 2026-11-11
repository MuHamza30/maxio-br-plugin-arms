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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Several products/components grant the same feature, or a usage_limit feature is granted with different periodicities → Entitlements are collapsed into one entry per feature key and periodicity window; different periodicities come back as separate entries sharing one feature_key. Key entitlements by periodicity_key, not by feature_key.</description></item>
    ///   <item><description>An aggregated entitlement has a periodicity window → periodicity_key is the prefixed feature key suffixed with :{interval}:{unit}; it equals feature_key when periodicity is null. Use periodicity_key as the unique id of an entry.</description></item>
    ///   <item><description>Several products or components on a subscription grant the same feature → Entitlements are collapsed into one entry per feature key and periodicity window, so different periodicities give separate entries with their own periodicity_key. Key entitlements by periodicity_key, not only by feature_key.</description></item>
    ///   <item><description>The subscription is not in a live state (active, trialing, assessing, past_due or soft_failure) → enabled is false regardless of the aggregated value; entitlements stay enabled while the subscription is in dunning. Check enabled before granting access in your application.</description></item>
    ///   <item><description>More than one contributor grants the same access_right feature key and periodicity → Values are combined with boolean OR: true if any contributor grants access; source_products lists only contributors that granted true. Treat an access_right as granted when any product or component grants it.</description></item>
    ///   <item><description>More than one contributor grants the same usage_limit feature key → Values are summed across contributors sharing the same periodicity window; grants with different periodicities are not summed and are returned as separate entries. Read the summed limit per periodicity_key; do not add entries of different periodicities yourself.</description></item>
    ///   <item><description>More than one contributor grants the same service_right feature key → Values are not combined; one contributor's value wins, and which one is not defined. Avoid granting the same service_right key from several products/components, or do not depend on which value is returned.</description></item>
    ///   <item><description>The subscription is not in a live state (active, trialing, assessing, past_due, soft_failure), e.g. awaiting_signup, canceled, expired or on hold → enabled is false for every entitlement, regardless of the aggregated value. Check enabled, not value, before granting access in your application.</description></item>
    ///   <item><description>The subscription is in dunning (past_due or soft_failure) → Entitlements stay enabled through dunning. Do not expect entitlements to switch off while the subscription is in dunning; restrict access yourself if your policy requires it.</description></item>
    ///   <item><description>The aggregated value is not truthy for the feature's kind (for example false or 0) → enabled is false even when the subscription is live. Check enabled rather than the presence of an entry.</description></item>
    ///   <item><description>An aggregated entitlement is returned → value is coerced by type: boolean for access_right (and boolean service_right), number for usage_limit (and numeric service_right), string for text service_right. Parse value according to type rather than as the string stored on the feature catalog item.</description></item>
    ///   <item><description>An aggregated entitlement is returned → feature_key is the template key prefixed by kind: feature.* for access_right, usage.* for usage_limit, service.* for service_right. Match entitlements on the prefixed key (e.g. feature.sso), not the bare template key.</description></item>
    /// </list>
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
