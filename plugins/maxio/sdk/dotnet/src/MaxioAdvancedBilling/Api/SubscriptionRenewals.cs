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
using MaxioAdvancedBilling.Requests.SubscriptionRenewals;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// The Subscription Renewals object lets you schedule, preview, and manage a subscription's upcoming renewal, including locking in specific pricing or canceling a scheduled renewal before it takes effect. For more information, see <see href="https://docs.maxio.com/billing/subscriptions/configuration/term-renewal/scheduled-renewal">Understand Scheduled Renewal</see>.
/// </summary>
public sealed class SubscriptionRenewals
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal SubscriptionRenewals(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Cancel Scheduled Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CancelScheduledRenewalConfigurationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Cancels a scheduled renewal configuration.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> CancelScheduledRenewalConfiguration(CancelScheduledRenewalConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}/cancel.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            CancelScheduledRenewalConfigurationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Scheduled Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateScheduledRenewalConfigurationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates a scheduled renewal configuration for a subscription. The scheduled renewal is based on the subscription’s current product and component setup.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> CreateScheduledRenewalConfiguration(CreateScheduledRenewalConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            CreateScheduledRenewalConfigurationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Scheduled Renewal Configuration Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateScheduledRenewalConfigurationItemError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Adds product and component line items to the scheduled renewal.
    /// <para>
    /// If your site has list vs sales pricing enabled, accepts renewal_configuration_item.custom_price.list_price_point_id, validates and persists it; omitted value follows existing/default behavior; with list vs sales pricing disabled, parameter is ignored (no validation/behavioral impact). This functionality is supported in the API, but is not currently supported in SDKs.
    /// </para>
    /// </remarks>
    public Task<ScheduledRenewalConfigurationItemResponse> CreateScheduledRenewalConfigurationItem(CreateScheduledRenewalConfigurationItemRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/scheduled_renewals/{scheduled_renewals_configuration_id}/configuration_items.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("scheduled_renewals_configuration_id", request.ScheduledRenewalsConfigurationId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ScheduledRenewalConfigurationItemResponse>(),
            CreateScheduledRenewalConfigurationItemError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Scheduled Renewal Configuration Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteScheduledRenewalConfigurationItemError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Removes an item from the pending renewal configuration.
    /// </remarks>
    public Task DeleteScheduledRenewalConfigurationItem(DeleteScheduledRenewalConfigurationItemRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/scheduled_renewals/{scheduled_renewals_configuration_id}/configuration_items/{id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("scheduled_renewals_configuration_id", request.ScheduledRenewalsConfigurationId),
                new TemplateParam("id", request.Id),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteScheduledRenewalConfigurationItemError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Scheduled Renewals
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists scheduled renewal configurations for the subscription and permits an optional status query filter.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationsResponse> ListScheduledRenewalConfigurations(ListScheduledRenewalConfigurationsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId)],
            [new Param("status", request.Status)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ScheduledRenewalConfigurationsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Immediate Renewal Lock-In
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="LockInScheduledRenewalImmediatelyError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Locks in the renewal immediately.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> LockInScheduledRenewalImmediately(LockInScheduledRenewalImmediatelyRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}/immediate_lock_in.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            LockInScheduledRenewalImmediatelyError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Read Scheduled Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Retrieves the configuration settings for the scheduled renewal.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> ReadScheduledRenewalConfiguration(ReadScheduledRenewalConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Scheduled Renewal Lock-In
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="ScheduleScheduledRenewalLockInError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Schedules a future lock-in date for the renewal.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> ScheduleScheduledRenewalLockIn(ScheduleScheduledRenewalLockInRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}/schedule_lock_in.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            ScheduleScheduledRenewalLockInError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Unpublish Scheduled Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UnpublishScheduledRenewalConfigurationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Restores a scheduled renewal configuration to an editable state.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> UnpublishScheduledRenewalConfiguration(UnpublishScheduledRenewalConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}/unpublish.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            EmptyBody.Instance,
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            UnpublishScheduledRenewalConfigurationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Scheduled Renewal
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateScheduledRenewalConfigurationError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates an existing configuration.
    /// </remarks>
    public Task<ScheduledRenewalConfigurationResponse> UpdateScheduledRenewalConfiguration(UpdateScheduledRenewalConfigurationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/subscriptions/{subscription_id}/scheduled_renewals/{id}.json"),
            [new TemplateParam("subscription_id", request.SubscriptionId), new TemplateParam("id", request.Id)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ScheduledRenewalConfigurationResponse>(),
            UpdateScheduledRenewalConfigurationError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Scheduled Renewal Configuration Item
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ScheduledRenewalConfigurationItemResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateScheduledRenewalConfigurationItemError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates an existing configuration item’s pricing and quantity.
    /// <para>
    /// If you site has list vs sales pricing enabled, accepts renewal_configuration_item.custom_price.list_price_point_id, validates and persists it; omitted value follows existing/default behavior; with list vs sales pricing disabled, parameter is ignored (no validation/behavioral impact). This functionality is supported in the API, but is not currently supported in SDKs.
    /// </para>
    /// </remarks>
    public Task<ScheduledRenewalConfigurationItemResponse> UpdateScheduledRenewalConfigurationItem(UpdateScheduledRenewalConfigurationItemRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production(
                "/subscriptions/{subscription_id}/scheduled_renewals/{scheduled_renewals_configuration_id}/configuration_items/{id}.json"),
            [
                new TemplateParam("subscription_id", request.SubscriptionId),
                new TemplateParam("scheduled_renewals_configuration_id", request.ScheduledRenewalsConfigurationId),
                new TemplateParam("id", request.Id),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<ScheduledRenewalConfigurationItemResponse>(),
            UpdateScheduledRenewalConfigurationItemError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
