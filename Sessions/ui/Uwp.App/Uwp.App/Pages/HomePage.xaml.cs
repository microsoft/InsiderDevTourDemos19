using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Uwp.App.Pages
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ImageView.LoadImages();
            JournalView.LoadDataGrid();
        }

        internal void ShowTeachingTips() =>
            HelperView.ShowTeachingTips(GaugeView.TeachingTipTarget, JournalView.TeachingTipTarget);

        private void ImageView_ImageUpdated(object sender, Controls.ImageUpdatedEventArgs args)
        {
            BlurredBackground.UpdateImage(args.ImageSource);
        }
    }
}
