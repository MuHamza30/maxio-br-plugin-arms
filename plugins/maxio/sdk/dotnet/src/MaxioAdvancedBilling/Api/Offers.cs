using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.Offers;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Offer object bundles a specific Product, Price Point, and any associated discounts into a single pre-configured signup option that customers can subscribe to directly.
/// <para>
/// Offers are commonly used to power self-service signup pages without requiring each pricing combination to be built manually. For more information, see <see href="https://docs.maxio.com/billing/catalog/offers/understand-offers">Understand Offers</see>.
/// </para>
/// </summary>
public sealed class Offers
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal Offers(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Archive Offer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Archives an existing offer. Please provide an <c>offer_id</c> in order to archive the correct item.
    /// </remarks>
    public Task ArchiveOffer(ArchiveOfferRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/offers/{offer_id}/archive.json"),
            [new TemplateParam("offer_id", request.OfferId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Offer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="OfferResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateOfferError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates an offer within your site.
    /// <para>
    /// Offers allow you to package complicated combinations of products, components and coupons into a convenient package which can then be subscribed to just like products.
    /// </para>
    /// <para>
    /// Once an offer is defined it can be used as an alternative to the product when creating subscriptions.
    /// </para>
    /// <para>
    /// For more information, see <see href="https://maxio.zendesk.com/hc/en-us/articles/24261295098637-Offers-Overview">Offers</see> in the product documentation.
    /// </para>
    /// <para>
    /// ## Using a Product Price Point
    /// </para>
    /// <para>
    /// You can optionally pass in a <c>product_price_point_id</c> that corresponds with the <c>product_id</c> and the offer will use that price point. If a <c>product_price_point_id</c> is not passed in, the product's default price point will be used.
    /// </para>
    /// </remarks>
    public Task<OfferResponse> CreateOffer(CreateOfferOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/offers.json"),
            [],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<OfferResponse>(),
            CreateOfferError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Offers
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListOffersResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ListOffersError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists offers for a site.
    /// </remarks>
    public Task<ListOffersResponse> ListOffers(ListOffersRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/offers.json"),
            [],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("include_archived", request.IncludeArchived),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListOffersResponse>(),
            ListOffersError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Offer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="OfferResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Returns a specific offer's attributes. This is different from listing all offers for a site, as it requires an <c>offer_id</c>.
    /// </remarks>
    public Task<OfferResponse> ReadOffer(ReadOfferRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/offers/{offer_id}.json"),
            [new TemplateParam("offer_id", request.OfferId)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<OfferResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unarchive Offer
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Unarchives a previously archived offer. Please provide an <c>offer_id</c> in order to unarchive the correct item.
    /// </remarks>
    public Task UnarchiveOffer(UnarchiveOfferRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/offers/{offer_id}/unarchive.json"),
            [new TemplateParam("offer_id", request.OfferId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            VoidResponse.Instance,
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
