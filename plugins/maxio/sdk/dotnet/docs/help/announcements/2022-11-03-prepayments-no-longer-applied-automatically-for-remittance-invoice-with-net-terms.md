# 2022-11-03 Prepayments No Longer Applied Automatically For Remittance Invoice with Net Terms

As of November 3rd, 2022, the logic of applying prepayments automatically is being changed for Remittance Invoices.

Previously, we applied prepayments automatically when issuing an invoice only in the cases of the collection method set to "remittance", or when the invoice didn't have net terms set.

Now, we apply prepayments automatically only in the case when net terms are not set, regardless of the collection method.

This change was made to ensure the prepayment behavior upon issuance of invoices with net terms is consistent for invoices of both "automatic" and "remittance" collection methods.
