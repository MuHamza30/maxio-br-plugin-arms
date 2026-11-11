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
