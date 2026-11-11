# 2021-11-18 Renewal Preview Returns Free Non-Zero Components

As of November 18th, 2021, the renewal preview API endpoint will return additional component line items.
Previously, only components with a cost greater than 0 were returned.

With the new behavior, free components will be returned as line items so long as their recorded quantity is greater than 0.
