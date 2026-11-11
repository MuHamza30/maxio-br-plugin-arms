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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A payment taken by the call requires 3D Secure authentication under Strong Customer Authentication → Maxio answers 422 with "action_link". Treat a 422 carrying an action_link as a 3DS challenge: send the customer to the link and complete the post-authentication flow.</description></item>
    ///   <item><description>The subscription is not active or trialing (trial_ended is still accepted for backwards compatibility but not recommended) → the outcome is not stated. Migrate only active or trialing subscriptions; for trial_ended, cancel, change the product and reactivate.</description></item>
    ///   <item><description>The migration leaves a balance to pay and no credit card is on file → Maxio answers 422 with "No credit card was on file for the $200.00 balance". Attach a payment method before a migration that creates a payment due.</description></item>
    ///   <item><description>A prorated migration is requested for a subscription that is not eligible for one → Maxio answers 422 with "This subscription is not eligible for a prorated migration". Retry without proration (preserve_period false) when this 422 is returned.</description></item>
    ///   <item><description>The migration targets the subscription's current product → The migration fails. Check that the target product differs from the current one before migrating.</description></item>
    ///   <item><description>The migration names neither product_id nor product_handle → the outcome is not stated. Send product_id or product_handle in the migration object.</description></item>
    ///   <item><description>A migration changes the subscription's billing date → No billing_date_change webhook is sent for migrations. Read billing dates from the migration response or subscription_product_change, not billing_date_change.</description></item>
    ///   <item><description>No price point identifier is passed → The subscription is moved to the target product's default price point. Pass product_price_point_id or product_price_point_handle for a non-default price point.</description></item>
    ///   <item><description>The target product is in another product family → Components are not migrated. Re-create needed component allocations after a cross-family migration.</description></item>
    ///   <item><description>The target product is in a new product family → Coupons cannot migrate, even with include_coupons true. Re-apply valid coupons after a cross-family migration.</description></item>
    ///   <item><description>include_trial is sent while preserve_period is set → include_trial is ignored. Do not combine include_trial with preserve_period.</description></item>
    ///   <item><description>preserve_period is false or omitted → The billing period is reset to today and the full price of the new product is charged. Send preserve_period true to keep the billing date and charge a prorated amount instead.</description></item>
    ///   <item><description>preserve_period is true → The billing period does not change and a prorated charge is issued for the new product. Expect a prorated charge when preserving the period.</description></item>
    ///   <item><description>The call attempts a payment (signup, retry after failure, reactivation, resumption or migration charge) → A payment_success or payment_failure webhook is sent for every payment attempted. Handle payment_success and payment_failure events caused by these calls.</description></item>
    ///   <item><description>The payment is made by ACH/eCheck or Direct Debit → A payment reported as successful may fail later. Do not treat payment_success for ACH or Direct Debit as final; watch for later failure events.</description></item>
    ///   <item><description>A subscription's product (or product version) changes successfully → A subscription_product_change webhook is sent. Handle subscription_product_change for product changes, migrations and version changes.</description></item>
    /// </list>
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
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>The migration names neither product_id nor product_handle → the outcome is not stated. Send product_id or product_handle in the migration object.</description></item>
    ///   <item><description>The subscription is not active → Maxio answers 422 with "Subscription must be active". Preview migrations only for active subscriptions.</description></item>
    ///   <item><description>proration_date is outside the subscription's current billing period → the outcome is not stated. Pick a proration_date within the current billing period.</description></item>
    ///   <item><description>No price point identifier is passed → The subscription is moved to the target product's default price point. Pass product_price_point_id or product_price_point_handle for a non-default price point.</description></item>
    ///   <item><description>The target product is in another product family → Components are not migrated. Re-create needed component allocations after a cross-family migration.</description></item>
    ///   <item><description>The target product is in a new product family → Coupons cannot migrate, even with include_coupons true. Re-apply valid coupons after a cross-family migration.</description></item>
    ///   <item><description>include_trial is sent while preserve_period is set → include_trial is ignored. Do not combine include_trial with preserve_period.</description></item>
    ///   <item><description>preserve_period is false or omitted → The billing period is reset to today and the full price of the new product is charged. Send preserve_period true to keep the billing date and charge a prorated amount instead.</description></item>
    ///   <item><description>preserve_period is true → The billing period does not change and a prorated charge is issued for the new product. Expect a prorated charge when preserving the period.</description></item>
    ///   <item><description>proration_date is given → Prorated adjustment, charge, payment due and credit applied are calculated as if the migration happened at that future date instead of now. Pass the planned migration date to get matching preview amounts.</description></item>
    ///   <item><description>The subscription has allocated components → The preview preserves the period, matching the actual migration (since 2022-12-07). Expect preview amounts for subscriptions with allocations to match the real migration.</description></item>
    /// </list>
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
