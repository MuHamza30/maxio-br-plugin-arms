using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.FeatureTemplates;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Feature Template object defines a feature once at the site level, such as Single Sign-On, a monthly API call limit, or a support tier.
/// <para>
/// A feature template is not billable on its own. Attach the template to products or components to grant the feature to subscribers. The kind (access_right, usage_limit, or service_right) determines how the feature's value behaves. For more information, see <see href="https://docs.maxio.com/billing/catalog/entitlements/configure-entitlements">Configure Entitlements</see>.
/// </para>
/// </summary>
public sealed class FeatureTemplates
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal FeatureTemplates(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Feature Template
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ArchiveFeatureTemplateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives a feature template. Archived feature templates are not addressable via <see href="$e/Feature%20Templates/readFeatureTemplate">Read Feature Template</see> or <see href="$e/Feature%20Templates/updateFeatureTemplate">Update Feature Template</see>. Both endpoints return <c>404</c> until the template is restored.
    /// <para>
    /// The feature template record itself is never hard-deleted, and can always be restored with <see href="$e/Feature%20Templates/restoreFeatureTemplate">Restore Feature Template</see>. Reversibility does not extend to <c>remove_from_catalog=true</c>: the feature catalog items and entitlements that parameter destroys are gone permanently, and restoring the template will not bring subscriber access back.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>archiveFeatureTemplate is called with remove_from_catalog=true → Every feature catalog item made from the template and their entitlements are destroyed permanently, revoking subscriber access at once; restoring the template does not bring them back. Use remove_from_catalog=true only when subscriber access must end now and for good.</description></item>
    ///   <item><description>A feature template is archived without remove_from_catalog=true → Existing entitlements are kept by default, as with removing a catalog item; remove_from_catalog=true overrides this. Pass remove_from_catalog=true when archiving should also revoke the feature.</description></item>
    ///   <item><description>archiveFeatureTemplate is called without remove_from_catalog (or with false) → The template and its feature catalog items are archived, and existing entitlements are kept. Expect current subscribers to keep the feature after a default archive.</description></item>
    ///   <item><description>A feature template is archived → The template record is never hard-deleted and can always be restored. Use restoreFeatureTemplate to bring an archived template back; its key stays taken.</description></item>
    /// </list>
    /// </remarks>
    public Task ArchiveFeatureTemplate(ArchiveFeatureTemplateRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [new Param("remove_from_catalog", request.RemoveFromCatalog)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            ArchiveFeatureTemplateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Feature Template
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureTemplateResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateFeatureTemplateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Defines a new feature at the site level. Feature templates aren't billable on their own. Attach a template to products or components to grant the feature to subscribers.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A feature template is created with kind usage_limit and no unit → Maxio answers 422. Send unit whenever kind is usage_limit.</description></item>
    ///   <item><description>A feature template is created with kind service_right and no value_type → Maxio answers 422. Send value_type (text, boolean or numeric) whenever kind is service_right.</description></item>
    ///   <item><description>default_periodicity_interval or default_periodicity_unit is sent for a feature template whose kind is not usage_limit → Maxio answers 422. Send default periodicity only for usage_limit templates.</description></item>
    ///   <item><description>Only one of default_periodicity_unit and default_periodicity_interval is sent → Maxio answers 422. Send default_periodicity_unit and default_periodicity_interval together or not at all.</description></item>
    ///   <item><description>A feature template is created with a key that another feature template already uses → Maxio answers 422. Choose a key not already used by any feature template on the site (archived ones included).</description></item>
    ///   <item><description>A feature template's key would be changed after it was set → the outcome is not stated. Choose feature template keys carefully; create a new template instead of changing a key.</description></item>
    ///   <item><description>A Feature has wallet_behavior and a subscription resolves an entitlement to it → A wallet with origin feature is provisioned automatically; Create Wallet is never called for it. Configure wallet_behavior on the Feature template instead of creating these wallets yourself.</description></item>
    ///   <item><description>A feature template of kind access_right or usage_limit is created with a value_type → value_type is ignored and set automatically: boolean for access_right, numeric for usage_limit. Do not send value_type for access_right or usage_limit; it is inferred.</description></item>
    ///   <item><description>A feature template is created or updated with wallet_behavior (wallet_product_id, name, price_point_id, depletion_policy, hard_stop_overflow, optional meter_id and conversion_rate), a field the Feature Template reference does not list → Subscriptions that resolve an entitlement to that feature get a wallet provisioned automatically (origin feature). Configure wallet_behavior on the feature template to auto-provision wallets; do not call createWallet for that case.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureTemplateResponse> CreateFeatureTemplate(CreateFeatureTemplateOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureTemplateResponse>(),
            CreateFeatureTemplateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Feature Templates
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureTemplatesListResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListFeatureTemplatesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the feature templates defined for your site, active (non-archived) ones by default. Pass <c>status=archived</c> or <c>status=all</c> to widen the result set.
    /// <para>
    /// Supply <c>page</c> or <c>per_page</c> to paginate. Without either parameter, the response includes the full result set.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>listFeatureTemplates is called without page and without per_page → The full result set is returned in one response, not a first page of 20. Send page or per_page to paginate feature templates; otherwise expect every match at once.</description></item>
    ///   <item><description>listFeatureTemplates is called with q → Only templates whose name contains q, compared case-insensitively, are returned. Use q for a case-insensitive name substring search.</description></item>
    ///   <item><description>listFeatureTemplates is called with updated_from → Templates updated on or after that date are returned (the date itself included). Treat updated_from as an inclusive lower bound.</description></item>
    ///   <item><description>listFeatureTemplates is called with updated_to → Templates updated on or before that date are returned (the date itself included). Treat updated_to as an inclusive upper bound.</description></item>
    ///   <item><description>listFeatureTemplates returns a non-empty active result set and status=archived was not requested → archived_count is returned as 0, not the real number of archived templates. Do not read archived_count as a total unless status=archived or the active result is empty.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureTemplatesListResponse> ListFeatureTemplates(ListFeatureTemplatesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("status", request.Status),
                new Param("q", request.Q),
                new Param("kind", request.Kind),
                new Param("updated_from", request.UpdatedFrom?.ToDate()),
                new Param("updated_to", request.UpdatedTo?.ToDate()),
                new Param("sort_by", request.SortBy),
                new Param("sort_direction", request.SortDirection),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureTemplatesListResponse>(),
            ListFeatureTemplatesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Feature Template
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureTemplateResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadFeatureTemplateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a single feature template. Archived feature templates are not addressable here and return <c>404</c>. Restore a template first to read or update it.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A read or update is sent for a feature template that is archived → Maxio answers 404. Restore an archived template with restoreFeatureTemplate before reading or updating it.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureTemplateResponse> ReadFeatureTemplate(ReadFeatureTemplateRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureTemplateResponse>(),
            ReadFeatureTemplateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Restore Feature Template
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureTemplateResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RestoreFeatureTemplateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Clears the feature template's archived state. Feature catalog items created from this template are not automatically restored. Restore each one individually.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An archived feature template is restored → Feature catalog items created from the template stay archived. Restore each feature catalog item individually after restoring its template.</description></item>
    ///   <item><description>An archived feature template is restored → Entitlements removed when it was archived are never restored. Re-grant removed entitlements yourself after restoring a feature template.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureTemplateResponse> RestoreFeatureTemplate(RestoreFeatureTemplateRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features/{id}/restore.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<FeatureTemplateResponse>(),
            RestoreFeatureTemplateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Feature Template
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FeatureTemplateResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateFeatureTemplateError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates the name, description, unit, value type, default value, or default periodicity of a feature template. <c>key</c> is rejected on every update. <c>kind</c> is rejected once any feature catalog item has been created from this template.
    /// <para>
    /// Archived feature templates are not addressable here and return <c>404</c>. Restore a template first to update it.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A read or update is sent for a feature template that is archived → Maxio answers 404. Restore an archived template with restoreFeatureTemplate before reading or updating it.</description></item>
    ///   <item><description>An update to a feature template includes key → Maxio answers 422. Never send key on an update; it is fixed at creation.</description></item>
    ///   <item><description>A feature template's key would be changed after it was set → the outcome is not stated. Choose feature template keys carefully; create a new template instead of changing a key.</description></item>
    ///   <item><description>A feature template's kind is changed after the template has been attached to a product or component → the outcome is not stated. Set the right kind before attaching the template anywhere.</description></item>
    ///   <item><description>An update changes kind on a feature template from which a feature catalog item has already been created → Maxio answers 422. Settle kind before attaching the template to any product or component.</description></item>
    ///   <item><description>A Feature has wallet_behavior and a subscription resolves an entitlement to it → A wallet with origin feature is provisioned automatically; Create Wallet is never called for it. Configure wallet_behavior on the Feature template instead of creating these wallets yourself.</description></item>
    ///   <item><description>A feature template is created or updated with wallet_behavior (wallet_product_id, name, price_point_id, depletion_policy, hard_stop_overflow, optional meter_id and conversion_rate), a field the Feature Template reference does not list → Subscriptions that resolve an entitlement to that feature get a wallet provisioned automatically (origin feature). Configure wallet_behavior on the feature template to auto-provision wallets; do not call createWallet for that case.</description></item>
    /// </list>
    /// </remarks>
    public Task<FeatureTemplateResponse> UpdateFeatureTemplate(UpdateFeatureTemplateOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/features/{id}.json"),
            [new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<FeatureTemplateResponse>(),
            UpdateFeatureTemplateError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
