using System.Collections.Generic;
using Uwp.App.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp.App.Controls
{
    public sealed partial class JournalView : UserControl
    {
        public JournalView()
        {
            InitializeComponent();
            TeachingTipTarget = Share;
        }

        internal FrameworkElement TeachingTipTarget { get; private set; }

        public void LoadDataGrid()
        {
            var journalItems = new List<Journal>
            {
                new Journal("First paper about space telescopes published", 1946),
                new Journal("Sputnik launched", 1957),
                new Journal("NASA created", 1958),
                new Journal("Project gained support", 1969),
                new Journal("First working group held", 1974),
                new Journal("Congress approved funding and project began", 1977),
                new Journal("Grinding of primary mirror began", 1978),
                new Journal("Astronauts began training for servicing missions", 1979),
                new Journal("Large space telescope named after Edwin Hubble", 1983),
                new Journal("Space shuttle Challenger lost", 1986),
                new Journal("Hubble launched", 1990),
                new Journal("Hubble deployed", 1990),
                new Journal("First image taken", 1990),
                new Journal("Spherical aberration discovered in mirror", 1990),
            };

            DataGrid.ItemsSource = journalItems;
        }
    }
}
