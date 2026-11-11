# Configurations

## Step 1: Enable and Set Up Embeddable Components in Advanced Billing

> _For **Early Access**, contact the Maxio Support Team for assistance._

First, identify the domain where you will host the website and generate a sign-in key for authenticated access to the host server.

> _The localhost domain will need to be added to develop the integration on a local machine. Once development is done,
> remove the localhost domain._

1. From Advanced Billing, go to **Config > Integrations > Embeddable Components**.
   <br>
   ![Enable Components Integration](static/images/embeddable-components/configurations/enable-components-integration.png)

2. Enter a **Domain** and click **Enable Maxio Components**.
   <br>
   This automatically generates a Base64 encoded Token Sign in Key.
   <br>
   ![Token Sign in Key](static/images/embeddable-components/configurations/token-sign-in-key.png)

3. Copy and store this token for later access.
   <br>
   Should you lose this key, you will need to generate a new one.

4. Go to **Config** > **Settings** > **Billing Portal** and click **Enabled for this site**.
   <br>
   ![Enable Billing Portal Setting](static/images/embeddable-components/configurations/enable-billing-portal-setting.png)

5. Select which **Features** will be available to your Customers.

   > Some unchecked Advanced Billing configurations are not needed for Embeddable Components. For example, **Remove
   > Maxio Advanced Billing Logo** does not have influence on your self-hosted application. Others, like **Allow
   > Plan Changes** will have an impact on your self-hosted application. Ensure the necessary
   > features
   > are enabled to meet the needs of your implementation.

6. Click **Save**.

## Step 2: Install the Components from Node Packaged Modules (NPM) Package Manager

> _Installing node.js also installs npm. Node.js is also required to run the vue.js or react.js examples._

1. [Access the self-service package](https://www.npmjs.com/package/@maxio-com/self-service).
   <br>
   ![self-service npm package](static/images/embeddable-components/configurations/self-service-npm-package-1.png)
   <br>
   The Readme provides installation instructions, points to vue.js and react.js
   examples, and goes through repository setup and use.
   <br>
   ![self-service npm package](static/images/embeddable-components/configurations/self-service-npm-package-2.png)

2. Create a project directory locally on your machine.

3. Open a command window/terminal and navigate to the local project directory you just created.

4. To download and install the dependencies, run the given npm CLI command:
   <br>
   `npm install @maxio-com/self-service`
   <br>
   ![npm install terminal](static/images/embeddable-components/configurations/npm-install-terminal.png)
   <br>
   A successful installation creates the **node_modules** directory in your local project folder. In this directory, you will find the `@maxio-com\self-service` directory.
   <br>
   ![npm modules directory](static/images/embeddable-components/configurations/node-modules-directory.png)

## Step 3: Use the Examples

To start using these components right away, use the out-of-the-box Vue or React examples provided.

> _The following set-up example uses vue.js on localhost._

### Setting up the Vue.js Self-Service Example

1. From the **npmjs.com** > **Headless Billing Portal** Readme page > **Installation and usage**, go to #4 Examples and click the
   **vue-self-service-example** link.
   <br>
   ![Vue.js Self-Service Example](static/images/embeddable-components/configurations/vue-js-self-service-example.png)
   <br>
   This takes you to the maxio-com vue.js example GitHub repository.

2. Clone the repository to your local environment:
   <br>
   `git clone https://github.com/maxio-com/vue-self-service-example`

3. Open the cloned repository and install the dependencies:
   <br>
   `pnpm install`
   <br>
   or
   <br>
   `npm install`

4. Run the development server (respectively):
   <br>
   `pnpm run dev`
   <br>
   or
   <br>
   `npm run dev`

### Viewing the Example Application

To view the Embeddable Components from an example application, open http://localhost:3000/.

![Embeddable Components localhost](static/images/embeddable-components/configurations/embeddable-components-localhost.png)

### Configuring the Example

1. From your editor, go to **src** > **components**, and open **BillingHistory.vue**.

2. Edit the following options:

   | **Option**        | **Description**                                                                        | **Example**                                                                                                                 |
   | :---------------- | :------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------- |
   | itemsPerPage      | Sets the number of items to list per page.                                             | itemsPerPage: 10,                                                                                                           |
   | paginationVariant | Sets the pagination type:<br/>- simple (infinite scroll)<br/>- pages (page pagination) | paginationVariant: 'simple',                                                                                                |
   | enableFiltering   | Enable (true)/disable (false) billing history filtering.                               | enableFiltering: true,                                                                                                      |
   | visibleColumns    | Set the array for the billing history fields (columns) to show on the table.           | visibleColumns: [<br>"invoice_number",<br>"issue_date",<br>"due_date",<br>"total_amount",<br>"amount_due",<br>"status"<br>] |

   _Example of Billing History Component options:_
   <br>
   ![Example of Billing History Component options](static/images/embeddable-components/configurations/example-of-billing-history-component-options.png)

3. Go back to the example application in your browser, open the **Billing History**
   page, http://localhost:3000/billing-history, and note the changes you made.

4. Next, open **CustomerDetails.vue** and edit the following options:

   | **Option**                | **Description**                                                           | **Example**                                                                                                                                                                                                                              |
   | :------------------------ | :------------------------------------------------------------------------ | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
   | customerDataVisibleFields | Set the array that contains the customer data fields visible to the user. | customerDataVisibleFields: [<br> "address",<br> "address2",<br> "city",<br> "state",<br> "zip",<br> "firstName",<br> "lastName",<br> "email",<br> "ccEmails",<br> "organization",<br> "vatNumber",<br> "phoneNumber",<br> "country"<br>] |

   _Example of Customer Details Component options:_
   <br>
   ![Example of Customer Details Component options](static/images/embeddable-components/configurations/example-of-customer-details-component-options.png)

5. Finally, open **SubscriptionManager.vue** and edit the following options:

   | **Option**                   | **Description**                                                                            | **Example**                                                                                            |
   | ---------------------------- | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ |
   | allocationFlow               | Set how component allocation flow is presented:<br>- modal<br>- page                       |                                                                                                        |
   | subscription                 | Subscription settings object:<br>- details<br> Set how subscription details are presented. |                                                                                                        |
   | paymentProfileEditFlow       | Set which form payment profile edit flow is presented.<br>- modal<br>- page                |                                                                                                        |
   | paymentProfileRequiredFields | Set the array that contains the _required_ payment profile address fields.                 |                                                                                                        |
   | addressFields                | Set the array that contains the payment profile address fields visible to the user.        | addressFields: [<br> "address",<br> "address2",<br> "city",<br> "state",<br> "zip",<br> "country"<br>] |

   _Example of Subscription Manager Component options:_
   <br>
   ![Example of Customer Details Component options](static/images/embeddable-components/configurations/example-of-subscription-manager-component-options.png)

## Step 4: Optional Configurations

### Use as a Standalone Library from CDN

To use Embeddable Components as a standalone library from a Content Delivery Network (CDN) do the following.

1. Add this script that points to the library in CDN to your page:
   <br>
   `<script src="https://cdn.jsdelivr.net/npm/@maxio-com/self-service@1.9.0/dist/maxio-components.umd.min.js"></script>`

2. Initialize Components.

   ```javascript
   const factory = new window.Maxio.Components(options);
   // Render Billing History to the document.body element
   let element = factory.create("billing-history");

   element.render(document.body);
   ```

3. Your local environment should match the domain you configured (add localhost if you want to run it locally).
   <br>
   Example:

   ```html
   <html>
     <head>
       <meta charset="utf-8" />
       <script src="https://cdn.jsdelivr.net/npm/@maxio-com/self-service@1.9.0/dist/maxio-components.umd.min.js"></script>
     </head>
     <body>
       <script>
         const factory = new window.Maxio.Components({
           i18nSettings: {
             loadPath: "<path to localization file>",
             language: "en",
           },
           accessTokenUrl: "<path to authorization service>",
         });

         let element = factory.create("billing-history");

         element.render(document.body);
       </script>
     </body>
   </html>
   ```

### Additional Advanced Billing Configurations

#### Custom Fields (Metafields)

You can set the level of access for each custom field (or metafield) to be viewable and editable for customers
using Embeddable Components. These settings apply to both subscription- and customer-level custom fields. The scopes
are named **public_show** and **public_edit**, respectively. Setting 'public_show' to '1' makes the custom field visible
in the Embeddable Components application, while setting 'public_edit' to '1' makes it editable.

> _Editing and viewing these metafields settings are currently only supported using the API._

#### Individual Component Configuration

Apart from the global components update setting, each individual component has an additional property controlling
whether it can be updated using a billing portal. This property option is also respected by Embeddable Components.

Do the following to enable the Billing Portal for a component.

1. From Advanced Billing, go to **Catalog** > **Products**.

2. Select the **Products family** to which the component belongs.

3. Click the **Components** tab.
   <br>
   ![Components tab](static/images/embeddable-components/configurations/components-tab.png)

4. Choose a component.

5. Scroll to the bottom of the component page, and for **Advanced Options** click **Edit**.

6. Enable the **Billing Portal** option.
   <br>
   ![Enable Billing Portal option](static/images/embeddable-components/configurations/enable-billing-portal-option.png)
