using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Uwp.App.Controls
{
    public sealed partial class BlurredBackground : UserControl
    {
        public BlurredBackground()
        {
            InitializeComponent();
        }

        internal void UpdateImage(ImageSource imageSource) => 
            BackgroundImage.Source = imageSource;
    }
}
