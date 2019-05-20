using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp.App.Controls
{
    public sealed partial class MissionItem : UserControl
    {
        public MissionItem()
        {
            InitializeComponent();
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(MissionItem), new PropertyMetadata(string.Empty));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(string), typeof(MissionItem), new PropertyMetadata(null));
    }
}
