using System;
using Uwp.App.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Uwp.App
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += (s, e) => NavigateToHome();
        }

        private void OnMainNavItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args) =>
            NavigateToPage(args.InvokedItemContainer.Tag);

        private void NavigateToPage(object pageTag)
        {
            var pageName = $"Uwp.App.Pages.{pageTag}";
            var pageType = Type.GetType(pageName);

            ContentFrame.Navigate(pageType);
        }

        private void NavigateToHome()
        {
            if (MainNav.MenuItems[0] is WinUI.NavigationViewItemBase item)
            {
                MainNav.SelectedItem = item;
                NavigateToPage(item.Tag);
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            if (MainNav.Content is Frame frame && 
                frame.Content is HomePage homePage)
            {
                homePage.ShowTeachingTips();
            }
        }
    }
}
