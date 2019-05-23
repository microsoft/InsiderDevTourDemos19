using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyGraph
{
    public class Connector
    {
        private static string _clientId = "<CLIENT ID>";

        private static string[] _scopes = { "User.Read", "Calendars.Read", "UserActivity.ReadWrite.CreatedByApp" };

        private IPublicClientApplication _clientApp;

        private GraphServiceClient _graph;

        public Connector()
        {
            // TODO: Initialize MSAL.NET & Graph
        }

        // TODO: Get User Name

        // TODO: Get Calendar Events

		// TODO: Add User Activity

        public async void LogoutAsync()
        {
            // TODO: Collapse Me Before Demo
            // Note: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/425
            foreach (var account in await _clientApp.GetAccountsAsync())
            {
                await _clientApp.RemoveAsync(account);
            }

            FileBasedTokenStorageProvider.Instance.ClearCache();
        }
    }
}
