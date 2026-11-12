using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Models.OneOf;

[JsonConverter(typeof(InvoiceEventConverter))]
public record InvoiceEvent
{
    private readonly Optional<ApplyCreditNoteEvent> _applyCreditNoteEventValue;

    private readonly Optional<ApplyDebitNoteEvent> _applyDebitNoteEventValue;

    private readonly Optional<ApplyPaymentEvent> _applyPaymentEventValue;

    private readonly Optional<BackportInvoiceEvent> _backportInvoiceEventValue;

    private readonly Optional<ChangeChargebackStatusEvent> _changeChargebackStatusEventValue;

    private readonly Optional<ChangeInvoiceCollectionMethodEvent> _changeInvoiceCollectionMethodEventValue;

    private readonly Optional<ChangeInvoiceStatusEvent> _changeInvoiceStatusEventValue;

    private readonly Optional<CreateCreditNoteEvent> _createCreditNoteEventValue;

    private readonly Optional<CreateDebitNoteEvent> _createDebitNoteEventValue;

    private readonly Optional<FailedPaymentEvent> _failedPaymentEventValue;

    private readonly Optional<IssueInvoiceEvent> _issueInvoiceEventValue;

    private readonly Optional<RefundInvoiceEvent> _refundInvoiceEventValue;

    private readonly Optional<RemovePaymentEvent> _removePaymentEventValue;

    private readonly Optional<VoidInvoiceEvent> _voidInvoiceEventValue;

    private readonly Optional<VoidRemainderEvent> _voidRemainderEventValue;

    private readonly Optional<UnknownArm> _unknownValue;

    private InvoiceEvent(Optional<ApplyCreditNoteEvent> applyCreditNoteEventValue,
        Optional<ApplyDebitNoteEvent> applyDebitNoteEventValue,
        Optional<ApplyPaymentEvent> applyPaymentEventValue,
        Optional<BackportInvoiceEvent> backportInvoiceEventValue,
        Optional<ChangeChargebackStatusEvent> changeChargebackStatusEventValue,
        Optional<ChangeInvoiceCollectionMethodEvent> changeInvoiceCollectionMethodEventValue,
        Optional<ChangeInvoiceStatusEvent> changeInvoiceStatusEventValue,
        Optional<CreateCreditNoteEvent> createCreditNoteEventValue,
        Optional<CreateDebitNoteEvent> createDebitNoteEventValue,
        Optional<FailedPaymentEvent> failedPaymentEventValue,
        Optional<IssueInvoiceEvent> issueInvoiceEventValue,
        Optional<RefundInvoiceEvent> refundInvoiceEventValue,
        Optional<RemovePaymentEvent> removePaymentEventValue,
        Optional<VoidInvoiceEvent> voidInvoiceEventValue,
        Optional<VoidRemainderEvent> voidRemainderEventValue,
        Optional<UnknownArm> unknownValue)
    {
        _applyCreditNoteEventValue = applyCreditNoteEventValue;
        _applyDebitNoteEventValue = applyDebitNoteEventValue;
        _applyPaymentEventValue = applyPaymentEventValue;
        _backportInvoiceEventValue = backportInvoiceEventValue;
        _changeChargebackStatusEventValue = changeChargebackStatusEventValue;
        _changeInvoiceCollectionMethodEventValue = changeInvoiceCollectionMethodEventValue;
        _changeInvoiceStatusEventValue = changeInvoiceStatusEventValue;
        _createCreditNoteEventValue = createCreditNoteEventValue;
        _createDebitNoteEventValue = createDebitNoteEventValue;
        _failedPaymentEventValue = failedPaymentEventValue;
        _issueInvoiceEventValue = issueInvoiceEventValue;
        _refundInvoiceEventValue = refundInvoiceEventValue;
        _removePaymentEventValue = removePaymentEventValue;
        _voidInvoiceEventValue = voidInvoiceEventValue;
        _voidRemainderEventValue = voidRemainderEventValue;
        _unknownValue = unknownValue;
    }

    public static InvoiceEvent ApplyCreditNoteEvent(ApplyCreditNoteEvent value) =>
        new(
            Optional<ApplyCreditNoteEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent ApplyDebitNoteEvent(ApplyDebitNoteEvent value) =>
        new(
            default,
            Optional<ApplyDebitNoteEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent ApplyPaymentEvent(ApplyPaymentEvent value) =>
        new(
            default,
            default,
            Optional<ApplyPaymentEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent BackportInvoiceEvent(BackportInvoiceEvent value) =>
        new(
            default,
            default,
            default,
            Optional<BackportInvoiceEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent ChangeChargebackStatusEvent(ChangeChargebackStatusEvent value) =>
        new(
            default,
            default,
            default,
            default,
            Optional<ChangeChargebackStatusEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent ChangeInvoiceCollectionMethodEvent(ChangeInvoiceCollectionMethodEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            Optional<ChangeInvoiceCollectionMethodEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent ChangeInvoiceStatusEvent(ChangeInvoiceStatusEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<ChangeInvoiceStatusEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent CreateCreditNoteEvent(CreateCreditNoteEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<CreateCreditNoteEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent CreateDebitNoteEvent(CreateDebitNoteEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<CreateDebitNoteEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent FailedPaymentEvent(FailedPaymentEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<FailedPaymentEvent>.Some(value),
            default,
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent IssueInvoiceEvent(IssueInvoiceEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<IssueInvoiceEvent>.Some(value),
            default,
            default,
            default,
            default,
            default);

    public static InvoiceEvent RefundInvoiceEvent(RefundInvoiceEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<RefundInvoiceEvent>.Some(value),
            default,
            default,
            default,
            default);

    public static InvoiceEvent RemovePaymentEvent(RemovePaymentEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<RemovePaymentEvent>.Some(value),
            default,
            default,
            default);

    public static InvoiceEvent VoidInvoiceEvent(VoidInvoiceEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<VoidInvoiceEvent>.Some(value),
            default,
            default);

    public static InvoiceEvent VoidRemainderEvent(VoidRemainderEvent value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<VoidRemainderEvent>.Some(value),
            default);

    internal static InvoiceEvent Unknown(JsonElement value) =>
        new(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            Optional<UnknownArm>.Some(new UnknownArm(value)));

    public bool TryGetApplyCreditNoteEvent([NotNullWhen(true)] out ApplyCreditNoteEvent? value) =>
        _applyCreditNoteEventValue.TryGetValue(out value);

    public bool TryGetApplyDebitNoteEvent([NotNullWhen(true)] out ApplyDebitNoteEvent? value) =>
        _applyDebitNoteEventValue.TryGetValue(out value);

    public bool TryGetApplyPaymentEvent([NotNullWhen(true)] out ApplyPaymentEvent? value) =>
        _applyPaymentEventValue.TryGetValue(out value);

    public bool TryGetBackportInvoiceEvent([NotNullWhen(true)] out BackportInvoiceEvent? value) =>
        _backportInvoiceEventValue.TryGetValue(out value);

    public bool TryGetChangeChargebackStatusEvent([NotNullWhen(true)] out ChangeChargebackStatusEvent? value) =>
        _changeChargebackStatusEventValue.TryGetValue(out value);

    public bool TryGetChangeInvoiceCollectionMethodEvent([NotNullWhen(true)] out ChangeInvoiceCollectionMethodEvent? value) =>
        _changeInvoiceCollectionMethodEventValue.TryGetValue(out value);

    public bool TryGetChangeInvoiceStatusEvent([NotNullWhen(true)] out ChangeInvoiceStatusEvent? value) =>
        _changeInvoiceStatusEventValue.TryGetValue(out value);

    public bool TryGetCreateCreditNoteEvent([NotNullWhen(true)] out CreateCreditNoteEvent? value) =>
        _createCreditNoteEventValue.TryGetValue(out value);

    public bool TryGetCreateDebitNoteEvent([NotNullWhen(true)] out CreateDebitNoteEvent? value) =>
        _createDebitNoteEventValue.TryGetValue(out value);

    public bool TryGetFailedPaymentEvent([NotNullWhen(true)] out FailedPaymentEvent? value) =>
        _failedPaymentEventValue.TryGetValue(out value);

    public bool TryGetIssueInvoiceEvent([NotNullWhen(true)] out IssueInvoiceEvent? value) =>
        _issueInvoiceEventValue.TryGetValue(out value);

    public bool TryGetRefundInvoiceEvent([NotNullWhen(true)] out RefundInvoiceEvent? value) =>
        _refundInvoiceEventValue.TryGetValue(out value);

    public bool TryGetRemovePaymentEvent([NotNullWhen(true)] out RemovePaymentEvent? value) =>
        _removePaymentEventValue.TryGetValue(out value);

    public bool TryGetVoidInvoiceEvent([NotNullWhen(true)] out VoidInvoiceEvent? value) =>
        _voidInvoiceEventValue.TryGetValue(out value);

    public bool TryGetVoidRemainderEvent([NotNullWhen(true)] out VoidRemainderEvent? value) =>
        _voidRemainderEventValue.TryGetValue(out value);

    public bool TryGetUnknown(out JsonElement value) => _unknownValue.TryGetValue(out value);

    public static implicit operator InvoiceEvent(ApplyCreditNoteEvent value) => ApplyCreditNoteEvent(value);

    public static implicit operator InvoiceEvent(ApplyDebitNoteEvent value) => ApplyDebitNoteEvent(value);

    public static implicit operator InvoiceEvent(ApplyPaymentEvent value) => ApplyPaymentEvent(value);

    public static implicit operator InvoiceEvent(BackportInvoiceEvent value) => BackportInvoiceEvent(value);

    public static implicit operator InvoiceEvent(ChangeChargebackStatusEvent value) =>
        ChangeChargebackStatusEvent(value);

    public static implicit operator InvoiceEvent(ChangeInvoiceCollectionMethodEvent value) =>
        ChangeInvoiceCollectionMethodEvent(value);

    public static implicit operator InvoiceEvent(ChangeInvoiceStatusEvent value) => ChangeInvoiceStatusEvent(value);

    public static implicit operator InvoiceEvent(CreateCreditNoteEvent value) => CreateCreditNoteEvent(value);

    public static implicit operator InvoiceEvent(CreateDebitNoteEvent value) => CreateDebitNoteEvent(value);

    public static implicit operator InvoiceEvent(FailedPaymentEvent value) => FailedPaymentEvent(value);

    public static implicit operator InvoiceEvent(IssueInvoiceEvent value) => IssueInvoiceEvent(value);

    public static implicit operator InvoiceEvent(RefundInvoiceEvent value) => RefundInvoiceEvent(value);

    public static implicit operator InvoiceEvent(RemovePaymentEvent value) => RemovePaymentEvent(value);

    public static implicit operator InvoiceEvent(VoidInvoiceEvent value) => VoidInvoiceEvent(value);

    public static implicit operator InvoiceEvent(VoidRemainderEvent value) => VoidRemainderEvent(value);

    public TResult Match<TResult>(Func<ApplyCreditNoteEvent, TResult> onApplyCreditNoteEvent,
        Func<ApplyDebitNoteEvent, TResult> onApplyDebitNoteEvent,
        Func<ApplyPaymentEvent, TResult> onApplyPaymentEvent,
        Func<BackportInvoiceEvent, TResult> onBackportInvoiceEvent,
        Func<ChangeChargebackStatusEvent, TResult> onChangeChargebackStatusEvent,
        Func<ChangeInvoiceCollectionMethodEvent, TResult> onChangeInvoiceCollectionMethodEvent,
        Func<ChangeInvoiceStatusEvent, TResult> onChangeInvoiceStatusEvent,
        Func<CreateCreditNoteEvent, TResult> onCreateCreditNoteEvent,
        Func<CreateDebitNoteEvent, TResult> onCreateDebitNoteEvent,
        Func<FailedPaymentEvent, TResult> onFailedPaymentEvent,
        Func<IssueInvoiceEvent, TResult> onIssueInvoiceEvent,
        Func<RefundInvoiceEvent, TResult> onRefundInvoiceEvent,
        Func<RemovePaymentEvent, TResult> onRemovePaymentEvent,
        Func<VoidInvoiceEvent, TResult> onVoidInvoiceEvent,
        Func<VoidRemainderEvent, TResult> onVoidRemainderEvent,
        Func<JsonElement, TResult> otherwise) =>
        this switch
        {
            _ when TryGetApplyCreditNoteEvent(out var applyCreditNoteEventValue) => onApplyCreditNoteEvent(
                applyCreditNoteEventValue),
            _ when TryGetApplyDebitNoteEvent(out var applyDebitNoteEventValue) => onApplyDebitNoteEvent(
                applyDebitNoteEventValue),
            _ when TryGetApplyPaymentEvent(out var applyPaymentEventValue) => onApplyPaymentEvent(
                applyPaymentEventValue),
            _ when TryGetBackportInvoiceEvent(out var backportInvoiceEventValue) => onBackportInvoiceEvent(
                backportInvoiceEventValue),
            _ when TryGetChangeChargebackStatusEvent(
                out var changeChargebackStatusEventValue) => onChangeChargebackStatusEvent(
                changeChargebackStatusEventValue),
            _ when TryGetChangeInvoiceCollectionMethodEvent(
                out var changeInvoiceCollectionMethodEventValue) => onChangeInvoiceCollectionMethodEvent(
                changeInvoiceCollectionMethodEventValue),
            _ when TryGetChangeInvoiceStatusEvent(out var changeInvoiceStatusEventValue) => onChangeInvoiceStatusEvent(
                changeInvoiceStatusEventValue),
            _ when TryGetCreateCreditNoteEvent(out var createCreditNoteEventValue) => onCreateCreditNoteEvent(
                createCreditNoteEventValue),
            _ when TryGetCreateDebitNoteEvent(out var createDebitNoteEventValue) => onCreateDebitNoteEvent(
                createDebitNoteEventValue),
            _ when TryGetFailedPaymentEvent(out var failedPaymentEventValue) => onFailedPaymentEvent(
                failedPaymentEventValue),
            _ when TryGetIssueInvoiceEvent(out var issueInvoiceEventValue) => onIssueInvoiceEvent(
                issueInvoiceEventValue),
            _ when TryGetRefundInvoiceEvent(out var refundInvoiceEventValue) => onRefundInvoiceEvent(
                refundInvoiceEventValue),
            _ when TryGetRemovePaymentEvent(out var removePaymentEventValue) => onRemovePaymentEvent(
                removePaymentEventValue),
            _ when TryGetVoidInvoiceEvent(out var voidInvoiceEventValue) => onVoidInvoiceEvent(voidInvoiceEventValue),
            _ when TryGetVoidRemainderEvent(out var voidRemainderEventValue) => onVoidRemainderEvent(
                voidRemainderEventValue),
            _ when TryGetUnknown(out var unknownValue) => otherwise(unknownValue),
            _ => throw new InvalidOperationException($"{nameof(InvoiceEvent)} holds no value.")
        };

    public void Match(Action<ApplyCreditNoteEvent> onApplyCreditNoteEvent,
        Action<ApplyDebitNoteEvent> onApplyDebitNoteEvent,
        Action<ApplyPaymentEvent> onApplyPaymentEvent,
        Action<BackportInvoiceEvent> onBackportInvoiceEvent,
        Action<ChangeChargebackStatusEvent> onChangeChargebackStatusEvent,
        Action<ChangeInvoiceCollectionMethodEvent> onChangeInvoiceCollectionMethodEvent,
        Action<ChangeInvoiceStatusEvent> onChangeInvoiceStatusEvent,
        Action<CreateCreditNoteEvent> onCreateCreditNoteEvent,
        Action<CreateDebitNoteEvent> onCreateDebitNoteEvent,
        Action<FailedPaymentEvent> onFailedPaymentEvent,
        Action<IssueInvoiceEvent> onIssueInvoiceEvent,
        Action<RefundInvoiceEvent> onRefundInvoiceEvent,
        Action<RemovePaymentEvent> onRemovePaymentEvent,
        Action<VoidInvoiceEvent> onVoidInvoiceEvent,
        Action<VoidRemainderEvent> onVoidRemainderEvent,
        Action<JsonElement> otherwise)
    {
        if (TryGetApplyCreditNoteEvent(out var applyCreditNoteEventValue)) onApplyCreditNoteEvent(
            applyCreditNoteEventValue);
        else if (TryGetApplyDebitNoteEvent(out var applyDebitNoteEventValue)) onApplyDebitNoteEvent(
            applyDebitNoteEventValue);
        else if (TryGetApplyPaymentEvent(out var applyPaymentEventValue)) onApplyPaymentEvent(applyPaymentEventValue);
        else if (TryGetBackportInvoiceEvent(out var backportInvoiceEventValue)) onBackportInvoiceEvent(
            backportInvoiceEventValue);
        else if (TryGetChangeChargebackStatusEvent(
            out var changeChargebackStatusEventValue)) onChangeChargebackStatusEvent(changeChargebackStatusEventValue);
        else if (TryGetChangeInvoiceCollectionMethodEvent(
            out var changeInvoiceCollectionMethodEventValue)) onChangeInvoiceCollectionMethodEvent(
            changeInvoiceCollectionMethodEventValue);
        else if (TryGetChangeInvoiceStatusEvent(out var changeInvoiceStatusEventValue)) onChangeInvoiceStatusEvent(
            changeInvoiceStatusEventValue);
        else if (TryGetCreateCreditNoteEvent(out var createCreditNoteEventValue)) onCreateCreditNoteEvent(
            createCreditNoteEventValue);
        else if (TryGetCreateDebitNoteEvent(out var createDebitNoteEventValue)) onCreateDebitNoteEvent(
            createDebitNoteEventValue);
        else if (TryGetFailedPaymentEvent(out var failedPaymentEventValue)) onFailedPaymentEvent(
            failedPaymentEventValue);
        else if (TryGetIssueInvoiceEvent(out var issueInvoiceEventValue)) onIssueInvoiceEvent(issueInvoiceEventValue);
        else if (TryGetRefundInvoiceEvent(out var refundInvoiceEventValue)) onRefundInvoiceEvent(
            refundInvoiceEventValue);
        else if (TryGetRemovePaymentEvent(out var removePaymentEventValue)) onRemovePaymentEvent(
            removePaymentEventValue);
        else if (TryGetVoidInvoiceEvent(out var voidInvoiceEventValue)) onVoidInvoiceEvent(voidInvoiceEventValue);
        else if (TryGetVoidRemainderEvent(out var voidRemainderEventValue)) onVoidRemainderEvent(
            voidRemainderEventValue);
        else if (TryGetUnknown(out var unknownValue)) otherwise(unknownValue);
        else throw new InvalidOperationException($"{nameof(InvoiceEvent)} holds no value.");
    }
}

file sealed class InvoiceEventConverter : JsonConverter<InvoiceEvent>
{
    public override InvoiceEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (!root.TryGetProperty("event_type", out var typeProperty) || typeProperty.ValueKind != JsonValueKind.String)
        {
            throw new JsonException("Missing or non-string 'event_type' discriminator field");
        }
        return typeProperty.GetString() switch
        {
            "apply_credit_note" => InvoiceEvent.ApplyCreditNoteEvent(root.Deserialize<ApplyCreditNoteEvent>(options)!),
            "apply_debit_note" => InvoiceEvent.ApplyDebitNoteEvent(root.Deserialize<ApplyDebitNoteEvent>(options)!),
            "apply_payment" => InvoiceEvent.ApplyPaymentEvent(root.Deserialize<ApplyPaymentEvent>(options)!),
            "backport_invoice" => InvoiceEvent.BackportInvoiceEvent(root.Deserialize<BackportInvoiceEvent>(options)!),
            "change_chargeback_status" => InvoiceEvent.ChangeChargebackStatusEvent(
                root.Deserialize<ChangeChargebackStatusEvent>(options)!),
            "change_invoice_collection_method" => InvoiceEvent.ChangeInvoiceCollectionMethodEvent(
                root.Deserialize<ChangeInvoiceCollectionMethodEvent>(options)!),
            "change_invoice_status" => InvoiceEvent.ChangeInvoiceStatusEvent(root.Deserialize<ChangeInvoiceStatusEvent>(
                options)!),
            "create_credit_note" => InvoiceEvent.CreateCreditNoteEvent(root.Deserialize<CreateCreditNoteEvent>(
                options)!),
            "create_debit_note" => InvoiceEvent.CreateDebitNoteEvent(root.Deserialize<CreateDebitNoteEvent>(options)!),
            "failed_payment" => InvoiceEvent.FailedPaymentEvent(root.Deserialize<FailedPaymentEvent>(options)!),
            "issue_invoice" => InvoiceEvent.IssueInvoiceEvent(root.Deserialize<IssueInvoiceEvent>(options)!),
            "refund_invoice" => InvoiceEvent.RefundInvoiceEvent(root.Deserialize<RefundInvoiceEvent>(options)!),
            "remove_payment" => InvoiceEvent.RemovePaymentEvent(root.Deserialize<RemovePaymentEvent>(options)!),
            "void_invoice" => InvoiceEvent.VoidInvoiceEvent(root.Deserialize<VoidInvoiceEvent>(options)!),
            "void_remainder" => InvoiceEvent.VoidRemainderEvent(root.Deserialize<VoidRemainderEvent>(options)!),
            _ => InvoiceEvent.Unknown(root)
        };
    }

    public override void Write(Utf8JsonWriter writer, InvoiceEvent value, JsonSerializerOptions options)
    {
        if (value.TryGetApplyCreditNoteEvent(out var applyCreditNoteEventValue))
        {
            JsonSerializer.Serialize(writer, applyCreditNoteEventValue, options);
        }
        else if (value.TryGetApplyDebitNoteEvent(out var applyDebitNoteEventValue))
        {
            JsonSerializer.Serialize(writer, applyDebitNoteEventValue, options);
        }
        else if (value.TryGetApplyPaymentEvent(out var applyPaymentEventValue))
        {
            JsonSerializer.Serialize(writer, applyPaymentEventValue, options);
        }
        else if (value.TryGetBackportInvoiceEvent(out var backportInvoiceEventValue))
        {
            JsonSerializer.Serialize(writer, backportInvoiceEventValue, options);
        }
        else if (value.TryGetChangeChargebackStatusEvent(out var changeChargebackStatusEventValue))
        {
            JsonSerializer.Serialize(writer, changeChargebackStatusEventValue, options);
        }
        else if (value.TryGetChangeInvoiceCollectionMethodEvent(out var changeInvoiceCollectionMethodEventValue))
        {
            JsonSerializer.Serialize(writer, changeInvoiceCollectionMethodEventValue, options);
        }
        else if (value.TryGetChangeInvoiceStatusEvent(out var changeInvoiceStatusEventValue))
        {
            JsonSerializer.Serialize(writer, changeInvoiceStatusEventValue, options);
        }
        else if (value.TryGetCreateCreditNoteEvent(out var createCreditNoteEventValue))
        {
            JsonSerializer.Serialize(writer, createCreditNoteEventValue, options);
        }
        else if (value.TryGetCreateDebitNoteEvent(out var createDebitNoteEventValue))
        {
            JsonSerializer.Serialize(writer, createDebitNoteEventValue, options);
        }
        else if (value.TryGetFailedPaymentEvent(out var failedPaymentEventValue))
        {
            JsonSerializer.Serialize(writer, failedPaymentEventValue, options);
        }
        else if (value.TryGetIssueInvoiceEvent(out var issueInvoiceEventValue))
        {
            JsonSerializer.Serialize(writer, issueInvoiceEventValue, options);
        }
        else if (value.TryGetRefundInvoiceEvent(out var refundInvoiceEventValue))
        {
            JsonSerializer.Serialize(writer, refundInvoiceEventValue, options);
        }
        else if (value.TryGetRemovePaymentEvent(out var removePaymentEventValue))
        {
            JsonSerializer.Serialize(writer, removePaymentEventValue, options);
        }
        else if (value.TryGetVoidInvoiceEvent(out var voidInvoiceEventValue))
        {
            JsonSerializer.Serialize(writer, voidInvoiceEventValue, options);
        }
        else if (value.TryGetVoidRemainderEvent(out var voidRemainderEventValue))
        {
            JsonSerializer.Serialize(writer, voidRemainderEventValue, options);
        }
        else if (value.TryGetUnknown(out var unknownValue))
        {
            unknownValue.WriteTo(writer);
        }
        else
        {
            throw new JsonException($"{nameof(InvoiceEvent)} contains no valid value to serialize.");
        }
    }
}
