using System;
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
using MaxioAdvancedBilling.Requests.SubscriptionProducts;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Use the Subscription Products object to migrate a subscription from one product or price point to another, including previewing the effects of a migration before committing to it. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/management/subscription-actions/upgrade-or-downgrade-a-subscription">Upgrade or Downgrade a Subscription</see>.
/// </summary>
public sealed class SubscriptionProducts
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionProducts(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Migrate Subscription Product
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="MigrateSubscriptionProductError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Migrates a subscription to a different product.
    /// <para>
    /// To create a migration, you must pass the <c>product_id</c> or <c>product_handle</c> in the object when you send a POST request. You can also pass either a <c>product_price_point_id</c> or <c>product_price_point_handle</c> to choose which price point the subscription is moved to. If no price point identifier is passed, the subscription is moved to the product's default price point. The response is the updated subscription.
    /// </para>
    /// <para>
    /// ## Valid Subscriptions
    /// </para>
    /// <para>
    /// Subscriptions should be in the <c>active</c> or <c>trialing</c> state to be migrated.
    /// </para>
    /// <para>
    /// (For backwards compatibility reasons, it is possible to migrate a subscription that is in the <c>trial_ended</c> state via the API, however this is not recommended.  Since <c>trial_ended</c> is an end-of-life state, the subscription should be canceled, the product changed, and then the subscription can be reactivated.)
    /// </para>
    /// <para>
    /// For more information, see <see href="https://docs.maxio.com/hc/en-us/articles/24252069837581-Product-Changes-and-Migrations">Product Changes and Migrations</see>.
    /// </para>
    /// <para>
    /// ## Failed Migrations
    /// </para>
    /// <para>
    /// Important note: One of the most common ways that a migration can fail is when the attempt is made to migrate a subscription to its current product.
    /// </para>
    /// <para>
    /// ## 3D Secure (3DS) Authentication post-authentication flow
    /// </para>
    /// <para>
    /// When a payment requires 3DS Authentication to adhere to Strong Customer Authentication (SCA), the request enters a post-authentication flow where a 422 Unprocessable Entity status is returned with an action_link that will direct the customer through 3DS Authentication.
    /// </para>
    /// <para>
    /// See the <see href="https://docs.maxio.com/hc/en-us/articles/44277749524365-3D-Secure-Post-Authentication-Flow">3D Secure Post-Authentication Flow</see> article in the product documentation to learn how to manage the redirect flow.
    /// </para>
    /// </remarks>
    public Task<SubscriptionResponse> MigrateSubscriptionProduct(MigrateSubscriptionProductRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/migrations.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionResponse>(),
            MigrateSubscriptionProductError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Preview Subscription Product Migration
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SubscriptionMigrationPreviewResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="PreviewSubscriptionProductMigrationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Previews the charges resulting from migrating a subscription to a different product.
    /// <para>
    /// ## Previewing a future date
    /// It is also possible to preview the migration for a date in the future, as long as it's still within the subscription's current billing period, by passing a <c>proration_date</c> along with the request (e.g., <c>"proration_date": "2020-12-18T18:25:43.511Z"</c>).
    /// </para>
    /// <para>
    /// This will calculate the prorated adjustment, charge, payment and credit applied values assuming the migration is done at that date in the future as opposed to right now.
    /// </para>
    /// </remarks>
    public Task<SubscriptionMigrationPreviewResponse> PreviewSubscriptionProductMigration(PreviewSubscriptionProductMigrationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/migrations/preview.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SubscriptionMigrationPreviewResponse>(),
            PreviewSubscriptionProductMigrationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
