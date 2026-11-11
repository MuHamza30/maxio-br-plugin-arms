# Version History

The latest version of `Embeddable Components` is available at the following URL:
[@maxio-com/self-service](https://www.npmjs.com/package/@maxio-com/self-service)

#### 2026-09-25 (1.18.2 Latest)

Declared Node 20 or later as a requirement in the package's `engines` field.

- [bugfix] Declare Node >=20 engine requirement
- [internal] Drop @semantic-release/git so releases can publish again

#### 2026-09-25 (1.18.1)

Upgraded react-router from 6.x to 7.x to resolve CVE-2026-53669 and CVE-2026-53666.
This version requires Node 20 or later. Installs on Node 18 or earlier can fail when the package manager enforces engine requirements, for example `yarn install --frozen-lockfile` or npm with `engine-strict`.

- [bugfix] Migrate react-router 6.x to 7.x

#### 2026-09-24 (1.18.0)

- [internal] Added Dependabot configuration with grouped updates

#### 2026-09-24 (1.17.3)

- [internal] Updated dependencies

#### 2026-06-23 (1.17.2)

- [internal] Updated dependencies

#### 2026-04-03 (1.16.1)

- [internal] Updated dependencies

#### 2026-02-18 (1.16.0)

- [internal] added repository workflow automation

#### 2026-01-22 (1.15.1)

Update dependencies to latest versions

- [bugfix] Updated package.json and pnpm dependencies

#### 2024-12-02 (1.15.0)

Improved subscription management by ensuring expired credit cards are linked to the active payment method.

- [feature] Exclude active cc from rest
- [feature] Move expired credit card to active payment method

#### 2024-11-20 (1.14.1)

Fixed a typo in messaging on the configure subscription form.

- [bugfix] Correct wording on configure subscription page

#### 2024-11-13 (1.14.0)

Added versioning for translations while maintaining backward compatibility.
Introduced versioned URLs.

- [feature] Translations versioning

#### 2024-11-13 (1.13.3)

Improved credit card validation form to properly highlight invalid fields.

- [bugfix] Add expiration value to CreditCardPCIFields
- [bugfix] Add missing field in createFieldsConfig
- [bugfix] Change use of expiration value
- [bugfix] Fix state select field
- [bugfix] Get rid of selectedCountry state
- [bugfix] Handle editing address
- [bugfix] Make card expiration and card number always required

#### 2024-11-06 (1.13.2)

Improved payment profile functionality by ensuring dropdowns stay open until an option is selected, and allowing forms to close when clicking the close button or outside the window.

- [bugfix] Close select when user clicks outside element
- [bugfix] Make solution global
- [bugfix] Payment profile defects

#### 2024-11-05 (1.13.1)

Fixed an issue where expired credit cards were not correctly marked as "expired."

- [bugfix] Reflect expired flag

#### 2024-10-25 (1.13.0)

Ensured that the subscription is immediately active after reactivation, regardless of whether the payment profile is new, expired, or up-to-date, providing a seamless experience for users.

- [feature] Add card and active right away
- [feature] Add dynamic content of createPaymentProfile
- [feature] Add new reactivate subscription flow
- [feature] Update case with up-to-date payment profile
- [feature] Update subscription when add payment profile

#### 2024-10-16 (1.12.1)

Added missing currency property for Subscription resource.

- [bugfix] Add missing prop
- [bugfix] Currency of subscriptions price

#### 2024-10-11 (1.12.0)

Resolved problems with the form autocomplete feature to ensure smoother user input.

- [bugfix] Add missing cvv
- [feature] Make autocomplete disable

#### 2024-08-14 (1.11.6)

Added Subscription components settings.

- [bugfix] usedInBilling prop is null

#### 2024-07-30 (1.11.5)

Made i18nSettings properties default.

- [bugfix] Add new key for button
- [bugfix] Defaults for i18nSettings
- [bugfix] Make i18nSettings properties default

#### 2024-07-30 (1.11.4)

Added i18nSettings properties to load default translations. Bug fixes and improvements.

- [bugfix] Added DEFAULT_LOAD_PATH
- [bugfix] Broken tests
- [bugfix] Change content of cancel button
- [bugfix] Change incorrect
- [bugfix] Change to close
- [bugfix] Define loadPath for i18n
- [bugfix] Fix nextBillingAmount price after adding a coupon
- [bugfix] Fixed CouponErrorCodes import
- [bugfix] Issue with wrong refreshing next billing amount field
- [bugfix] Issues with displaying subscriptionDetails
- [bugfix] Remove instance of deleted file
- [bugfix] Remove unnecessary useCoupons hook
- [bugfix] Remove unused file
- [bugfix] Translations
- [bugfix] Typo
- [bugfix] Use capital letter
- [bugfix] Use static en in default_load_path

#### 2024-07-12 (1.11.3)

Fixed total price value being displayed for Subscription resource.

- [bugfix] Change total price value

#### 2024-07-02 (1.11.2)

- [bugfix] Include plan_change_scheduled to the nextBillingAmount

#### 2024-06-28 (1.11.1)

- [bugfix] Change reactive subscription error message

#### 2024-06-27 (1.11.0)

Updated the Embeddable Components to respect billing portal settings. The discount section is now hidden when applying coupons is disabled.

- [feature] Hide discount section when coupons setting is false

#### 2024-06-26 (1.10.1)

Added a mechanism for fetching and handling subscription details in the app.
<br>
Introduced a new coupons prop to render coupon data in the Subscription component.
<br>
Implemented a custom hook `useSubscriptionDetailsService` for managing subscription details retrieval.
<br>
Updated test cases with `subscriptionDetails` constant for simulating subscription detail responses.
<br>
Enhanced API-related configurations and mock setups in SubscriptionContainer tests.

- [bugfix] Fixed state result after fetch subscription details
- [bugfix] invalid-billing-amount

#### 2024-06-03 (1.10.0)

Improvement in session manager to refresh auth token if `exp` claim is missing

- [bugfix] Set properly api url and request headers
- [feature] Handle refresh token if exp date is missing

#### 2024-05-22 (1.9.6)

- [bugfix] Update Content-Type header

#### 2024-04-29 (1.9.5)

- [bugfix] Extend SessionManager to include async headers hook

#### 2024-03-26 (1.9.4)

Migrated Storybook to the latest major version to improve compatibility and maintainability.

- [feature] Migration from Storybook 6 to Storybook 8

#### 2024-03-04 (1.9.3)

Updated dependency management and tooling configuration to stabilize builds and test execution.

- [bugfix] Freeze dependencies for minor updates only
- [bugfix] Force Docker Compose compatibility with 4.16.0
- [bugfix] Fixed broken tests after dependency updates
- [feature] Version update from 7 to 8

#### 2023-08-11 (1.9.2)

Improved payment profile serialization reliability and test coverage.

- [bugfix] Fix payment profile property access
- [bugfix] Adjust serializePaymentProfiles test
- [bugfix] Add payment profile serializer test

#### 2023-07-28 (1.9.1)

Improved visual consistency for subscription status tags.

- [bugfix] Align cancelled Tag to have the same color
- [bugfix] Roll back conflicting style change

#### 2023-07-20 (1.9.0)

Enhanced modal display behavior in Storybook and product UI layouts.

- [feature] Improve modal sizing in Storybook
- [bugfix] Update max height handling for modal content

#### 2023-07-18 (1.8.1)

Applied additional refinements to modal layout behavior.

- [bugfix] Modal sizing polish for edge-case layouts

#### 2023-07-18 (1.8.0)

Initial release of the modal sizing improvements for Reusable UI Components.

- [feature] Introduce modal sizing updates for improved usability
