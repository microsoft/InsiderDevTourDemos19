using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp.App.Controls
{
    public sealed partial class SpeedGauge : UserControl
    {
        public SpeedGauge()
        {
            InitializeComponent();
        }

        public double PercentValue
        {
            get => (double)GetValue(PercentValueProperty);
            set => SetValue(PercentValueProperty, value);
        }
        public static readonly DependencyProperty PercentValueProperty =
            DependencyProperty.Register("PercentValue", typeof(double), typeof(SpeedGauge), new PropertyMetadata(0.0d));

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(SpeedGauge), new PropertyMetadata(0.0d));

        public double AverageValue
        {
            get => (double)GetValue(AverageValueProperty);
            set => SetValue(AverageValueProperty, value);
        }
        public static readonly DependencyProperty AverageValueProperty =
            DependencyProperty.Register("AverageValue", typeof(double), typeof(SpeedGauge), new PropertyMetadata(0.0d));

    }
}
