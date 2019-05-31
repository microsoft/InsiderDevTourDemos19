# Microsoft Graph session demos

## 1. Microsoft Graph Explorer

Start by going to [graph.microsoft.com](https://graph.microsoft.com) and navigating to the Graph Explorer​ in the top nav bar.

Show getting JSON from the /me endpoint​.

Show the different samples and point out that each sample links to the relevant docs​.

Show additional samples and enable few of your favorites (ex: calendar, one drive, etc).​

Login with the tenant account and retrieve real data​

Have [outlook.com](https://outlook.office365.com) logged in in a separate window with the same account and snap it side by side with the Graph Explorer​

Post a draft email in the Graph Explorer and show the draft showing in the outlook view​

1. Add a draft
    ```
    POST https://graph.microsoft.com/v1.0/me/messages

    {
    "subject": "message subject"
    }
    ```

2. Patch a draft (copy id from previous call)
    ```
    PATCH https://graph.microsoft.com/v1.0/me/messages/{id}

    {
    "subject": "message updated subject
    }
    ```

3. Delete draft
    ```
    DELETE https://graph.microsoft.com/v1.0/me/messages/{id}
    ```

Finally, navigate to the documentation and make sure developers are aware of the different sections​

## 2. Quick starts

Start from [graph.microsoft.com](https://graph.microsoft.com) again and navigate to quick starts (Getting Started dropdown in the top nav bar)

Select Node.js-> Get an app ID and secret (log in with tenant account)

Copy secret to clipboard

Download the zip-> unzip and open in VS Code -> run `npm install`

While `npm install` is going, open `.env` and paste secret

once `npm install` is finished run `npm start`

Open the browser to localhost and sign in with the tenant account

Show the calendar view in the site

Go back to vscode and show `graph.js`

## 3. Microsoft Graph Toolkit

### Prerequisites

1. VS Code
2. [VS Code live server extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer)
    - VS Code live server uses `127.0.0.1` as the default host and `8080` as the default port. You can change both of these in the VS code settings - I recommend `localhost:3000`. The `settings.json` in this folder makes this change for you - open this folder in vscode.
3. [A client id for an AAD app created in the azure portal with the appropriate redirect url](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-spa-app-registration) (the redirect url will depend on your live server settings - for example: `http://localhost:3000` and `http://localhost:3000/index.html`). Make sure both `Access tokens` and `ID tokens` are checked under the `Authentication` tab in the portal.

### Demo

Start with the repo page of the [Microsoft Graph Toolkit](https://aka.ms/mgt) in the browser

In the index.html file, show the reference to the toolkit loader

Add a <mgt-login> component -> Start live server and open in the browser - show the code and the browser side by side

Go to azure portal and copy the client ID for the app you had created previously

Back to the code, add <msal-provider> with the client ID you copied -> show the code now working in the browser

Back to the code, add the <mgt-agenda> component and show it working.

Back to the code, add a quick template to overwrite the event template and show templating
