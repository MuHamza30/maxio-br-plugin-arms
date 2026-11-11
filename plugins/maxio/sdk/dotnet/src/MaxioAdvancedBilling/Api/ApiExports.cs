using System;
using System.Collections.Generic;
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
using MaxioAdvancedBilling.Requests.ApiExports;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// API Exports allow you to generate and download bulk exports of subscriptions, invoices, and proforma invoices as asynchronous batch jobs, useful for reporting or migrating data outside the standard paginated list endpoints. For more information, see <see href="https://docs.maxio.com/insights/data-export/field-references/billing-export-fields-reference#invoice-line-items-fields">Billing Export Fields Reference</see>.
/// </summary>
public sealed class ApiExports
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal ApiExports(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Invoices Export
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ExportInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an invoices export and returns a batch job object.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ExportInvoices(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/invoices.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ExportInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Proforma Invoices Export
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ExportProformaInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a proforma invoices export and returns a batch job object. Proforma invoices are only available on Relationship Invoicing sites.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoices export is created or read on a site without Relationship Invoicing → the outcome is not stated. Use proforma invoice exports only on Relationship Invoicing sites.</description></item>
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ExportProformaInvoices(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/proforma_invoices.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ExportProformaInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Subscriptions Export
    /// </summary>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ExportSubscriptionsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a subscriptions export and returns a batch job object.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ExportSubscriptions(RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/subscriptions.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ExportSubscriptionsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Exported Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Invoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListExportedInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists exported invoices for a provided <c>batch_id</c>. Use pagination to control responses returned from the server.
    /// <para>
    /// Example: <c>GET https://{subdomain}.chargify.com/api_exports/invoices/123/rows?per_page=10000&amp;page=1</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    ///   <item><description>Exported rows are listed with per_page above 10000 → per_page is changed to 10000 (the default is 100). Page through exported rows with per_page of at most 10000.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Invoice>> ListExportedInvoices(ListExportedInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/invoices/{batch_id}/rows.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [new Param("per_page", request.PerPage), new Param("page", request.Page)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<Invoice>>(),
            ListExportedInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Exported Proforma Invoices
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="ProformaInvoice"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListExportedProformaInvoicesError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists exported proforma invoices for a provided <c>batch_id</c>. Use pagination to control responses returned from the server.
    /// <para>
    /// Example: <c>GET https://{subdomain}.chargify.com/api_exports/proforma_invoices/123/rows?per_page=10000&amp;page=1</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    ///   <item><description>Exported rows are listed with per_page above 10000 → per_page is changed to 10000 (the default is 100). Page through exported rows with per_page of at most 10000.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<ProformaInvoice>> ListExportedProformaInvoices(ListExportedProformaInvoicesRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/proforma_invoices/{batch_id}/rows.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [new Param("per_page", request.PerPage), new Param("page", request.Page)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<ProformaInvoice>>(),
            ListExportedProformaInvoicesError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Exported Subscriptions
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Subscription"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListExportedSubscriptionsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists exported subscriptions for a provided <c>batch_id</c>. Use pagination to control responses returned from the server.
    /// <para>
    /// Example: <c>GET https://{subdomain}.chargify.com/api_exports/subscriptions/123/rows?per_page=200&amp;page=1</c>.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    ///   <item><description>Exported rows are listed with per_page above 10000 → per_page is changed to 10000 (the default is 100). Page through exported rows with per_page of at most 10000.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Subscription>> ListExportedSubscriptions(ListExportedSubscriptionsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/subscriptions/{batch_id}/rows.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [new Param("per_page", request.PerPage), new Param("page", request.Page)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<IReadOnlyList<Subscription>>(),
            ListExportedSubscriptionsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Invoices Export
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadInvoicesExportError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a batch job object for an invoices export.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ReadInvoicesExport(ReadInvoicesExportRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/invoices/{batch_id}.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ReadInvoicesExportError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Proforma Invoices Export
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadProformaInvoicesExportError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a batch job object for a proforma invoices export. Proforma invoices are only available on Relationship Invoicing sites.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A proforma invoices export is created or read on a site without Relationship Invoicing → the outcome is not stated. Use proforma invoice exports only on Relationship Invoicing sites.</description></item>
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ReadProformaInvoicesExport(ReadProformaInvoicesExportRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/proforma_invoices/{batch_id}.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ReadProformaInvoicesExportError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Subscriptions Export
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="BatchJobResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ReadSubscriptionsExportError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a batch job object for a subscriptions export.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>An export is created → The export runs as an asynchronous batch job: the create call returns a batch job, whose status is read with the Read endpoint before the rows are listed. Poll the export's batch job with the Read endpoint and list rows only after it completes.</description></item>
    /// </list>
    /// </remarks>
    public Task<BatchJobResponse> ReadSubscriptionsExport(ReadSubscriptionsExportRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/api_exports/subscriptions/{batch_id}.json"),
            [new TemplateParam("batch_id", request.BatchId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<BatchJobResponse>(),
            ReadSubscriptionsExportError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
