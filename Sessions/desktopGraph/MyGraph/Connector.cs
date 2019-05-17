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
    }
}
