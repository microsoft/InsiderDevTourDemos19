# Identity + Graph Demos

## Demo Setup (same sets up both)

Be logged into windows with an MSA that you'll use for the identity part of the demo.

Setup [todo](https://todo.microsoft.com/en-us) items and [calendar](https://www.outlook.com/) events for the MSA.

Open up `identity-graph` folder in VS Code.

Open up terminal in VS Code with ``Ctrl+` ``

```console
npm install
npm run serve
```

Snap VS Code to the left of the screen `Win+Left`

Open up an in-private browser window.

Setup any `app registration` in the [Azure Portal](https://portal.azure.com/) with a tenant account.

Leave this page open in the window, but go back to the azure portal home page.

Open up two more tabs:

- Open up [Graph Explorer](https://developer.microsoft.com/en-us/graph/graph-explorer) and sign-in to the tenant account, make sure only the Getting Started sample query is shown.
- http://localhost:8080/ signed in to the tenant account.

## Identity Demo

### App Demo

1) Open up a new regular browser window to http://localhost:8080/
2) Login to app, should see account tied to windows at bottom
3) Click on it, should automatically login to app, voila!
4) Close browser
5) Re-open, auto logs in to app

Call out: Same code for this integrated auth, passwords, or multi-factor auth.

### Portal

To get started, we need a client id from Azure.

1) Open up the in-private browser window with the [Azure Portal](https://portal.azure.com/).
2) Search for `app registrations`, click on the one created before.
3) Point out the client-id and the copy icon on the overview page.

### MSAL.js

Mention the Identity library is open sourced!

1) Go to GitHub: https://github.com/AzureAD/microsoft-authentication-library-for-js
2) Scroll to the top of the readme, click on `Samples`
3) Click on the first link to `MSAL JS quickstart sample (calling MS Graph API)`
4) Click on the `JavaScriptSPA` directory and `index.html`
5) Scroll down to line 22 and show where they can insert their client id to get started.

## Graph Demo

Graph can get us a lot of different data.  We can use Graph Explorer to see it easily.

1) Go to the Graph Explorer tab in the in-private browser.
2) `Add more samples` add the Outlook Calendar and People categories.
3) Show the `my events for the next week` and `people I work with` responses.

The Microsoft Graph Toolkit makes it simple to use and display this data we've just seen.

1) Go to the localhost tab.
2) Snap app to the right with `Win+Right` (VS Code should be there on the left now.)
3) Type `mgtpeople` to get snippet added after comment.
4) Save, show recent people, hover over contact to get name.
5) Type `mgttemplate` to get snippet within people tag.
6) Save, see templated control!
