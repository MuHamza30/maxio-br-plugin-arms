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
using MaxioAdvancedBilling.Requests.EventsBasedBillingSegments;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Segment object defines a distinct pricing variation within an event-based billing metric, based on one or more property values, such as a specific country or usage category.
/// <para>
/// Each segment carries its own set of prices, letting a single metric charge different rates depending on which segment an event falls into.
/// </para>
/// </summary>
public sealed class EventsBasedBillingSegments
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal EventsBasedBillingSegments(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Bulk Create Segments
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSegmentsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkCreateSegmentsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates multiple segments in one request. The array of segments can contain up to <c>2000</c> records.
    /// <para>
    /// If any of the records contain an error the whole request would fail and none of the requested segments get created. The error response contains a message for only the one segment that failed validation, with the corresponding index in the array.
    /// </para>
    /// <para>
    /// You may specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task<ListSegmentsResponse> BulkCreateSegments(BulkCreateSegmentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments/bulk.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ListSegmentsResponse>(),
            BulkCreateSegmentsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Bulk Update Segments
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSegmentsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="BulkUpdateSegmentsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates multiple segments in one request. The array of segments can contain up to <c>1000</c> records.
    /// <para>
    /// If any of the records contain an error the whole request would fail and none of the requested segments get updated. The error response contains a message for only the one segment that failed validation, with the corresponding index in the array.
    /// </para>
    /// <para>
    /// You may specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task<ListSegmentsResponse> BulkUpdateSegments(BulkUpdateSegmentsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments/bulk.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ListSegmentsResponse>(),
            BulkUpdateSegmentsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Single Segment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SegmentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateSegmentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a new segment for a component with a segmented metric. It allows you to specify properties to bill upon and prices for each Segment. You can only pass as many "property_values" as the related Metric has segmenting properties defined.
    /// <para>
    /// You may specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task<SegmentResponse> CreateSegment(CreateSegmentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SegmentResponse>(),
            CreateSegmentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Single Segment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteSegmentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a segment with the specified ID.
    /// <para>
    /// You may specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task DeleteSegment(DeleteSegmentRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments/{id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
                new TemplateParam("id", request.Id),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteSegmentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Segments for a Price Point
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListSegmentsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListSegmentsForPricePointError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists segments created for a given price point, in order of creation.
    /// <para>
    /// You can pass <c>page</c> and <c>per_page</c> parameters in order to access all of the segments. By default it will return <c>30</c> records. You can set <c>per_page</c> to <c>200</c> at most.
    /// </para>
    /// <para>
    /// You may specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task<ListSegmentsResponse> ListSegmentsForPricePoint(ListSegmentsForPricePointRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
            ],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("filter", request.Filter),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListSegmentsResponse>(),
            ListSegmentsForPricePointError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Single Segment
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="SegmentResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateSegmentError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates a single segment for a component with a segmented metric. You can also update the pricing for the segment.
    /// <para>
    /// You can specify component and/or price point by using either the numeric ID or the <c>handle:gold</c> syntax.
    /// </para>
    /// </remarks>
    public Task<SegmentResponse> UpdateSegment(UpdateSegmentOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/components/{component_id}/price_points/{price_point_id}/segments/{id}.json"),
            [
                new TemplateParam("component_id", request.ComponentId),
                new TemplateParam("price_point_id", request.PricePointId),
                new TemplateParam("id", request.Id),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<SegmentResponse>(),
            UpdateSegmentError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
