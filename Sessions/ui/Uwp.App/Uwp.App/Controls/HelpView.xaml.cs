using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Uwp.App.Controls
{
    public sealed partial class HelpView : UserControl
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private async void TeachingTip1_ActionButtonClick(WinUI.TeachingTip sender, object args)
        {
            TeachingTip1.IsOpen = false;

            await Task.Delay(700);
            TeachingTip2.IsOpen = true;
        }

        internal void ShowTeachingTips(FrameworkElement element1, FrameworkElement element2)
        {
            TeachingTip1.IsOpen = true;
            TeachingTip2.Target = element1;
            TeachingTip3.Target = element2;
        }

        private async void TeachingTip2_ActionButtonClick(WinUI.TeachingTip sender, object args)
        {
            TeachingTip2.IsOpen = false;

            await Task.Delay(700);
            TeachingTip3.IsOpen = true;
        }
    }
}
