# PWA Web Push

A Sample Web Push Notification server implemented with node.js and the [web-push](https://www.npmjs.com/package/web-push) npm module.
The web push subscriptions are saved to a [nedb](https://github.com/louischatriot/nedb) database that requires no configuration.
The server serves a simple PWA app with a service worker set up to support web push notifications.

This sample is based on code from the following:

- [Get started with Progressive Web Apps](https://docs.microsoft.com/en-us/microsoft-edge/progressive-web-apps/get-started)

- [web-push-book](https://github.com/gauntface/web-push-book)

- [Web Push Notifications](https://webpushdemo.azurewebsites.net/)

- [Sending Messages with Web Push Libraries](https://developers.google.com/web/fundamentals/push-notifications/sending-messages-with-web-push-libraries)

## Setup

- You will need to create a .env file in the PWA-push folder. It should have the following format:

VAPID_PUBLICKEY=your vapid public key

VAPID_PRIVATEKEY=your vapid private key

VAPID_EMAIL=your vapid contact email address

- You can generate the vapid keys [here](https://tools.reactpwa.com/vapid). Copy the public and private keys into the .env file.
  Note: The .gitignore file is set to ignore the .env file

- You can create a .env file from the command prompt using:

```console
type nul > .env
```

- Install the npm dependencies

```console
npm install
```

- Start the server

```console
npm start
```

- You can also open the PWA-push.sln and start the node server from there.

- With your browser, open http://localhost:1337/

- Click on Send Notification

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Reporting Security Issues

Security issues and bugs should be reported privately, via email, to the Microsoft Security
Response Center (MSRC) at [secure@microsoft.com](mailto:secure@microsoft.com). You should
receive a response within 24 hours. If for some reason you do not, please follow up via
email to ensure we received your original message. Further information, including the
[MSRC PGP](https://technet.microsoft.com/en-us/security/dn606155) key, can be found in
the [Security TechCenter](https://technet.microsoft.com/en-us/security/default).
