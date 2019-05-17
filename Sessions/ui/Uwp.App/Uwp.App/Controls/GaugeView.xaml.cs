using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp.App.Controls
{
    public sealed partial class GaugeView : UserControl
    {
        public GaugeView()
        {
            InitializeComponent();

            TeachingTipTarget = Gauge1;
        }

        internal FrameworkElement TeachingTipTarget { get; private set; } 
    }
}
