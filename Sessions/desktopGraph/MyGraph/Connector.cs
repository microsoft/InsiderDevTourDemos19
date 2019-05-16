using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace MyGraph
{
    public class Connector
    {
        private static string _clientId = "<client id>";

        private static string[] _scopes = { "User.Read", "Calendars.Read" };

        private IPublicClientApplication _clientApp;

        private GraphServiceClient _graph;

        public Connector()
        {
            // Create Client Application and Authentication Provider
            _clientApp = InteractiveAuthenticationProvider.CreateClientApplication(
                _clientId,
                new FileBasedTokenStorageProvider());

            _clientApp.RedirectUri = "urn:ietf:wg:oauth:2.0:oob:myappname";

            var authProvider = new InteractiveAuthenticationProvider(_clientApp, _scopes);

            // Create GraphServiceClient with middleware pipeline setup
            _graph = new GraphServiceClient(authProvider);
        }

        public async Task<string> GetUserNameAsync()
        {
            // Request using default app permissions
            var user = await _graph.Me.Request().GetAsync();

            return user.DisplayName;
        }
    }
}
