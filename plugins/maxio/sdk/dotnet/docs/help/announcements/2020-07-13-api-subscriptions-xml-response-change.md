# 2020-07-13 API Subscriptions XML Response Change

As of July 13th, 2020, the XML response for reading multiple subscriptions has changed.
Before, dates and integers with no value still displayed a type:

```
<trial-started-at type="datetime" nil="true"/>
<next_product_id type="integer" nil="true"/>
```

Now, the type will no longer be returned for a nil field.

```
<trial-started-at nil="true"/>
<next_product_id nil="true"/>
```

Note: This same change was already previously made when reading a single subscription in 2018.
