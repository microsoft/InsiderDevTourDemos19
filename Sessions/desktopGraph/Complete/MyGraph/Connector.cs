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
            // Create Client Application and Authentication Provider
            _clientApp = InteractiveAuthenticationProvider.CreateClientApplication(
                _clientId,
                FileBasedTokenStorageProvider.Instance);

            _clientApp.RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

            var authProvider = new InteractiveAuthenticationProvider(_clientApp, _scopes);

            // Create GraphServiceClient with middleware pipeline setup
            _graph = new GraphServiceClient(authProvider);
        }

		// TODO: Get User Name
        public async Task<string> GetUserNameAsync()
        {
            // Request using default app permissions
            var user = await _graph.Me.Request().GetAsync();

            return user.DisplayName;
        }

		// TODO: Get Calendar Events
        public async Task<Event[]> GetCalendarEventsAsync()
        {
            // Calendar Data, Today and Next 2 Days (Local)
            // Between Previous Midnight (of today) and Midnight of 2nd day (3 from now)
            var today = DateTimeOffset.Now.Date.ToUniversalTime();
            var events = await _graph.Me.CalendarView.Request(new[] {
                new QueryOption("startDateTime", today.ToString("o", CultureInfo.InvariantCulture)),
                new QueryOption("endDateTime", today.AddDays(3).ToString("o", CultureInfo.InvariantCulture)),
            }).OrderBy("start/dateTime").GetAsync();

            return events.CurrentPage.ToArray();
        }

		// TODO: Add User Activity
        public async Task<UserActivity> AddUserActivityAsync(string appActivityId, string activity, string description)
        {
            var result = await _graph.Me.Activities.Request().AddUserActivityAsync(new UserActivity()
            {
                AppActivityId = appActivityId,
                ActivitySourceHost = "https://graphexplorer.blob.core.windows.net",
                AppDisplayName = "Graph in WPF",
                ActivationUrl = "https://developer.microsoft.com/en-us/graph/graph-explorer",
                FallbackUrl = "https://developer.microsoft.com/en-us/graph/graph-explorer",
                VisualElements = new VisualInfo()
                {
                    Description = description,
                    BackgroundColor = "#008272",
                    DisplayText = activity,
                    Attribution = new ImageInfo()
                    {
                        IconUrl = "https://raw.githubusercontent.com/microsoftgraph/g-raph/master/g-raph.png",
                        AlternateText = "Microsoft Graph",
                        AddImageQuery = false,
                    },
                }
            });

            // Need History Item to appear in Timeline
            await _graph.Me.Activities[result.Id].HistoryItems.Request().AddActivityHistoryAsync(new ActivityHistoryItem()
            {
                StartedDateTime = DateTimeOffset.Now.AddMinutes(-5),
                LastActiveDateTime = DateTimeOffset.Now,
                UserTimezone = TimeZoneInfo.Local.StandardName
            });

            // Return completed result.
            return await _graph.Me.Activities[result.Id].Request().Expand("historyItems").GetAsync();
        }

        public async void LogoutAsync()
        {
            // Note: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/425
            foreach (var account in await _clientApp.GetAccountsAsync())
            {
                await _clientApp.RemoveAsync(account);
            }

            FileBasedTokenStorageProvider.Instance.ClearCache();
        }
    }
}
