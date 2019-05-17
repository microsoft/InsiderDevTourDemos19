using SceneLoaderComponent;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Uwp.App.Pages
{
    public sealed partial class HubblePage : Page
    {
        public HubblePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var loader = new SceneLoader();
        }
    }
}
