using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Exceptions;
using MaxioAdvancedBilling.Core.Extensions;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Core.Request;
using MaxioAdvancedBilling.Core.Response;
using MaxioAdvancedBilling.Errors;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Requests.CustomFields;

namespace MaxioAdvancedBilling.Api;

/// <summary>
/// Custom Fields let you define Metafields for a resource type and manage the Metadata values attached to individual records, so you can capture and store additional data beyond built-in system fields. For more information, see <see href="https://docs.maxio.com/configuration/advanced-billing/custom-fields/understand-custom-fields">Understand Custom Fields</see>.
/// </summary>
public sealed class CustomFields
{
    private readonly RawClient _rawClient;
    private readonly Server _server;
    private readonly AuthSchemes _auth;

    internal CustomFields(RawClient rawClient, Server server, AuthSchemes auth)
    {
        _rawClient = rawClient;
        _server = server;
        _auth = auth;
    }

    /// <summary>
    /// Create Metadata
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Metadata"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateMetadataError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates metadata and metafields for a specific subscription or customer, or updates metadata values of existing metafields for a subscription or customer. Metadata values are limited to 2 KB in size.
    /// <para>
    /// If you create metadata on a subscription or customer with a metafield that does not already exist, the metafield is created with the metadata you specify and it is always added as a text field. You can update the input_type for the metafield with the <see href="$e/Custom%20Fields/updateMetafield">Update Metafield</see> endpoint.
    /// </para>
    /// <para>
    /// &gt;Note: Each site is limited to 100 unique metafields per resource. This means you can have 100 metafields for Subscriptions and another 100 for Customers.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A metadata (custom field) value is larger than 2 KB / 2048 characters → the outcome is not stated. Keep custom field values within 2048 characters (2 KB).</description></item>
    ///   <item><description>Creating a metafield (directly, by Update Metafield with a new current_name, or implicitly through metadata) would exceed 100 unique metafields for that resource type on the site → the outcome is not stated. Keep at most 100 metafields for subscriptions and 100 for customers per site, and reuse existing metafields.</description></item>
    ///   <item><description>A metafield or metadata entry is sent without a name → Maxio answers 422 with "'name' must be present to create or update metafields". Always send name for every metafield and metadata entry.</description></item>
    ///   <item><description>Metadata is created or updated on a subscription or customer with a metafield name that does not exist yet → The metafield is created on the site as a text field with the given value. Create metafields first (or change input_type afterwards with Update Metafield) when they should not be text fields.</description></item>
    ///   <item><description>Create Metadata names a metafield that already has a value on the subscription or customer → The existing value is updated rather than rejected. Treat Create Metadata as an upsert and do not use it to detect existing values.</description></item>
    ///   <item><description>A custom field value is added or changed on a subscription or customer (including at signup) → A custom_field_value_change webhook is fired. Expect custom_field_value_change webhooks for your own metadata writes and avoid loops.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Metadata>> CreateMetadata(CreateMetadataOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/{resource_id}/metadata.json"),
            [
                new TemplateParam("resource_type", request.ResourceType),
                new TemplateParam("resource_id", request.ResourceId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<IReadOnlyList<Metadata>>(),
            CreateMetadataError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Create Metafields
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Metafield"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="CreateMetafieldsError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Creates metafields on a Site for either the Subscriptions or Customers resource.
    /// <para>
    /// Metafields and their metadata are created in the Custom Fields configuration page on your Site. Metafields can be populated with metadata when you create them or later with the <see href="$e/Custom%20Fields/updateMetafield">Update Metafield</see>, <see href="$e/Custom%20Fields/createMetadata">Create Metadata</see>, or <see href="$e/Custom%20Fields/updateMetadata">Update Metadata</see> endpoints. The Create Metadata and Update Metadata endpoints allow you to add metafields and metadata values to a specific subscription or customer.
    /// </para>
    /// <para>
    /// Each site is limited to 100 unique metafields per resource. This means you can have 100 metafields for Subscriptions and another 100 for Customers.
    /// </para>
    /// <para>
    /// &gt; Note: After creating a metafield, the resource type cannot be modified.
    /// </para>
    /// <para>
    /// In the UI and product documentation, metafields and metadata are called Custom Fields.
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Metafield is the custom field</description></item>
    ///   <item><description>Metadata is the data populating the custom field.</description></item>
    /// </list>
    /// <para>
    /// See <see href="https://docs.maxio.com/hc/en-us/articles/24266140850573-Custom-Fields-Reference">Custom Fields Reference</see> and <see href="https://maxio.zendesk.com/hc/en-us/articles/24251701302925-Subscription-Summary-Custom-Fields-Tab">Custom Fields Tab</see> for information on using Custom Fields in the Advanced Billing UI.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Creating a metafield (directly, by Update Metafield with a new current_name, or implicitly through metadata) would exceed 100 unique metafields for that resource type on the site → the outcome is not stated. Keep at most 100 metafields for subscriptions and 100 for customers per site, and reuse existing metafields.</description></item>
    ///   <item><description>A metafield already exists and its resource type (subscriptions or customers) would need to change → the outcome is not stated. Choose the resource type correctly when creating a metafield; create a new metafield on the other resource instead of moving it.</description></item>
    ///   <item><description>A metafield or metadata entry is sent without a name → Maxio answers 422 with "'name' must be present to create or update metafields". Always send name for every metafield and metadata entry.</description></item>
    ///   <item><description>enum values are sent for a metafield whose input_type is not radio or dropdown → enum applies only to radio and dropdown metafields. Send enum only with input_type radio or dropdown.</description></item>
    ///   <item><description>The enum list of a new metafield contains empty strings → Empty strings are not submitted as enum values. Do not rely on empty strings being stored as dropdown or radio options.</description></item>
    ///   <item><description>A metafield is created without input_type → The metafield defaults to a text field. Send input_type explicitly when the metafield should be radio, dropdown or balance_tracker.</description></item>
    ///   <item><description>A metafield's scope flags (csv, invoices, statements, portal, public_show, public_edit) are set to 1 → The metafield and its metadata are shown or exported in those places: CSV export, invoices, statements, the portal/Public Signup Pages, and viewable or editable in Embeddable Components. Set each scope flag deliberately, since it controls where custom field data is shown to customers.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Metafield>> CreateMetafields(CreateMetafieldsOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/metafields.json"),
            [new TemplateParam("resource_type", request.ResourceType)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Post,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<IReadOnlyList<Metafield>>(),
            CreateMetafieldsError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Metadata
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteMetadataError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes one or more metafields (and associated metadata) from the specified subscription or customer.
    /// </remarks>
    public Task DeleteMetadata(DeleteMetadataRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/{resource_id}/metadata.json"),
            [
                new TemplateParam("resource_type", request.ResourceType),
                new TemplateParam("resource_id", request.ResourceId),
            ],
            [new Param("name", request.Name), new Param("names", request.Names)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteMetadataError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Delete Metafield
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="DeleteMetafieldError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Deletes a metafield from your Site. Removes the metafield and associated metadata from all Subscriptions or Customers resources on the Site.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A metafield is deleted → The metafield and its metadata are removed from every subscription or customer on the site. Delete a metafield only when its values are no longer needed on any record.</description></item>
    /// </list>
    /// </remarks>
    public Task DeleteMetafield(DeleteMetafieldRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/metafields.json"),
            [new TemplateParam("resource_type", request.ResourceType)],
            [new Param("name", request.Name)],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Delete,
            EmptyBody.Instance,
            VoidResponse.Instance,
            DeleteMetafieldError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Metadata
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaginatedMetadata"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists metadata and metafields for a specific customer or subscription.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaginatedMetadata> ListMetadata(ListMetadataRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/{resource_id}/metadata.json"),
            [
                new TemplateParam("resource_type", request.ResourceType),
                new TemplateParam("resource_id", request.ResourceId),
            ],
            [new Param("page", request.Page), new Param("per_page", request.PerPage)],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<PaginatedMetadata>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Metadata for Resource Type
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="PaginatedMetadata"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists metadata for a specified array of subscriptions or customers.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>date_field is given a value other than created_at or updated_at → The value is ignored and the date filters are not applied. Send date_field as created_at or updated_at only, and check that date-filtered results really are filtered.</description></item>
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>start_date or end_date is used to filter metadata → start_date matches from midnight and end_date up to and including 11:59:59 PM, both in the site's time zone. Interpret start_date and end_date as whole days in the site's time zone.</description></item>
    ///   <item><description>start_datetime or end_datetime is sent without a time zone → The site's time zone is used. Include a time zone offset in start_datetime and end_datetime when you mean UTC or another zone.</description></item>
    ///   <item><description>Both a date and a datetime bound are sent (start_date with start_datetime, or end_date with end_datetime) → The datetime parameter is used instead of the date parameter. Send either the date or the datetime form of each bound, not both.</description></item>
    ///   <item><description>List Metafields or List Metadata for Resource Type is called without direction → Results are sorted by id ascending; direction=desc sorts descending. Pass direction explicitly when the order matters.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<PaginatedMetadata> ListMetadataForResourceType(ListMetadataForResourceTypeRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/metadata.json"),
            [new TemplateParam("resource_type", request.ResourceType)],
            [
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("date_field", request.DateField),
                new Param("start_date", request.StartDate?.ToDate()),
                new Param("end_date", request.EndDate?.ToDate()),
                new Param("start_datetime", request.StartDatetime?.ToIso8601()),
                new Param("end_datetime", request.EndDatetime?.ToIso8601()),
                new Param("with_deleted", request.WithDeleted),
                new Param("resource_ids", request.ResourceIds),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<PaginatedMetadata>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// List Metafields
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="ListMetafieldsResponse"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="RawError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Lists the metafields and their associated details for a Site and resource type. You can filter the request to a specific metafield.
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A list request asks for a page past the last page of results → An empty result set is returned, not an error. Stop paging when a page comes back empty or shorter than per_page.</description></item>
    ///   <item><description>List Metafields or List Metadata for Resource Type is called without direction → Results are sorted by id ascending; direction=desc sorts descending. Pass direction explicitly when the order matters.</description></item>
    ///   <item><description>A list request sends per_page above 200 → per_page is changed to 200 instead of being rejected. Page with per_page of 200 or less and never expect more than 200 records per page.</description></item>
    /// </list>
    /// </remarks>
    public Task<ListMetafieldsResponse> ListMetafields(ListMetafieldsRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/metafields.json"),
            [new TemplateParam("resource_type", request.ResourceType)],
            [
                new Param("name", request.Name),
                new Param("page", request.Page),
                new Param("per_page", request.PerPage),
                new Param("direction", request.Direction),
            ],
            [],
            HttpMethod.Get,
            EmptyBody.Instance,
            JsonResponse.Create<ListMetafieldsResponse>(),
            RawErrorResponse.Instance,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Metadata
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Metadata"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateMetadataError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates metadata and metafields on the Site and the customer or subscription specified, and updates the metadata value on a subscription or customer.
    /// <para>
    /// If you update metadata on a subscription or customer with a metafield that does not already exist, the metafield is created with the metadata you specify and it is always added as a text field to the Site and to the subscription or customer you specify. You can update the input_type for the metafield with the Update Metafield endpoint.
    /// </para>
    /// <para>
    /// Each site is limited to 100 unique metafields per resource. This means you can have 100 metafields for the Subscription resource and another 100 for the Customer resource.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A metadata (custom field) value is larger than 2 KB / 2048 characters → the outcome is not stated. Keep custom field values within 2048 characters (2 KB).</description></item>
    ///   <item><description>Creating a metafield (directly, by Update Metafield with a new current_name, or implicitly through metadata) would exceed 100 unique metafields for that resource type on the site → the outcome is not stated. Keep at most 100 metafields for subscriptions and 100 for customers per site, and reuse existing metafields.</description></item>
    ///   <item><description>A metafield or metadata entry is sent without a name → Maxio answers 422 with "'name' must be present to create or update metafields". Always send name for every metafield and metadata entry.</description></item>
    ///   <item><description>Metadata is created or updated on a subscription or customer with a metafield name that does not exist yet → The metafield is created on the site as a text field with the given value. Create metafields first (or change input_type afterwards with Update Metafield) when they should not be text fields.</description></item>
    ///   <item><description>A custom field value is added or changed on a subscription or customer (including at signup) → A custom_field_value_change webhook is fired. Expect custom_field_value_change webhooks for your own metadata writes and avoid loops.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Metadata>> UpdateMetadata(UpdateMetadataOperationRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/{resource_id}/metadata.json"),
            [
                new TemplateParam("resource_type", request.ResourceType),
                new TemplateParam("resource_id", request.ResourceId),
            ],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<IReadOnlyList<Metadata>>(),
            UpdateMetadataError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);

    /// <summary>
    /// Update Metafield
    /// </summary>
    /// <param name="request">The operation's inputs</param>
    /// <param name="requestOptions">Per-request options, such as an overriding log level for this call</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="IReadOnlyList{T}"/> of <see cref="Metafield"/> instance.</returns>
    /// <exception cref="ApiException{TError}"> of <see cref="UpdateMetafieldError"/> when the server returns an error response.</exception>
    /// <remarks>
    /// Updates metafields on your Site for a resource type.  Depending on the request structure, you can update or add metafields and metadata to the Subscriptions or Customers resource.
    /// <para>
    /// With this endpoint, you can:
    /// </para>
    /// <para>
    /// - Add metafields. If the metafield specified in current_name does not exist, a new metafield is added.
    ///   &gt;Note: Each site is limited to 100 unique metafields per resource. This means you can have 100 metafields for Subscriptions and another 100 for Customers.
    /// </para>
    /// <para>
    /// - Change the name of a metafield.
    ///   &gt;Note: To keep the metafield name the same and only update the metadata for the metafield, you must use the current metafield name in both the <c>current_name</c> and <c>name</c> parameters.
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Change the input type for the metafield. For example, you can change a metafield input type from text to a dropdown. If you change the input type from text to a dropdown or radio, you must update the specific subscriptions or customers where the metafield was used to reflect the updated metafield and metadata.</description></item>
    /// </list>
    /// <para>
    /// - Add metadata values to the existing metadata for a dropdown or radio metafield.
    ///   &gt;Note: Updates to metadata overwrite. To add one or more values, you must specify all metadata values including the new value you want to add.
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Add new metadata to a dropdown or radio for a metafield that was created without metadata.</description></item>
    /// </list>
    /// <para>
    /// - Remove metadata for a dropdown or radio for a metafield.
    ///   &gt;Note: Updates to metadata overwrite existing values. To remove one or more values, specify all metadata values except those you want to remove.
    /// </para>
    /// <para>
    /// - Add or update scope settings for a metafield.
    ///   &gt;Note: Scope changes overwrite existing settings. You must specify the complete scope, including the changes you want to make.
    /// </para>
    /// <para>
    /// <b>Business rules</b>
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Creating a metafield (directly, by Update Metafield with a new current_name, or implicitly through metadata) would exceed 100 unique metafields for that resource type on the site → the outcome is not stated. Keep at most 100 metafields for subscriptions and 100 for customers per site, and reuse existing metafields.</description></item>
    ///   <item><description>A metafield already exists and its resource type (subscriptions or customers) would need to change → the outcome is not stated. Choose the resource type correctly when creating a metafield; create a new metafield on the other resource instead of moving it.</description></item>
    ///   <item><description>A metafield is given a name already used by another metafield of the same resource type → Maxio answers 422 with "Metafield name must be unique for the Attachment Type". Keep metafield names unique within subscriptions and within customers.</description></item>
    ///   <item><description>enum values are sent for a metafield whose input_type is not radio or dropdown → enum applies only to radio and dropdown metafields. Send enum only with input_type radio or dropdown.</description></item>
    ///   <item><description>A metafield's scope flags (csv, invoices, statements, portal, public_show, public_edit) are set to 1 → The metafield and its metadata are shown or exported in those places: CSV export, invoices, statements, the portal/Public Signup Pages, and viewable or editable in Embeddable Components. Set each scope flag deliberately, since it controls where custom field data is shown to customers.</description></item>
    ///   <item><description>A metafield's scope is updated with only some of the scope attributes → The partial scope overwrites the existing scope settings; attributes left out are not kept. Send the complete scope object, including unchanged attributes, whenever you update scope.</description></item>
    ///   <item><description>Update Metafield names a current_name that does not exist for the resource type → A new metafield is added instead of an error being returned. Check the current_name spelling, since a typo creates a new metafield.</description></item>
    ///   <item><description>Update Metafield is used only to change a metafield's metadata (enum) while keeping its name → the outcome is not stated. Send the current metafield name in both current_name and name.</description></item>
    ///   <item><description>A metafield's input type is changed from text to dropdown or radio → Existing values on subscriptions or customers are not adjusted to the new input type. Update each subscription or customer that used the metafield so its value matches the new metafield and metadata.</description></item>
    ///   <item><description>The metadata values (enum) of a dropdown or radio metafield are updated → The sent list overwrites the existing values; values left out are removed. Send the full list of values: all existing ones plus additions, or all except those to remove.</description></item>
    /// </list>
    /// </remarks>
    public Task<IReadOnlyList<Metafield>> UpdateMetafield(UpdateMetafieldRequest request,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
        _rawClient.Execute(
            _server.Production("/{resource_type}/metafields.json"),
            [new TemplateParam("resource_type", request.ResourceType)],
            [],
            [new HeaderParam("Idempotency-Key", Guid.NewGuid())],
            HttpMethod.Put,
            JsonRequest.Create(request.Body),
            JsonResponse.Create<IReadOnlyList<Metafield>>(),
            UpdateMetafieldError.Response,
            _auth.BasicAuth,
            requestOptions,
            cancellationToken);
}
