using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using Uwp.App.Models;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Uwp.App.Controls
{
    public sealed partial class ImageView : UserControl
    {
        private readonly Compositor _compositor;
        private readonly List<Photo> _photos = new List<Photo>
            {
                new Photo("Veil Nebula Supernova Remnant","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hs-2015-29-a-xlarge_web.jpg" ),
                new Photo("Hubble Sets Sights on a Galaxy with a Bright Heart","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1843a.jpg" ),
                new Photo("Hubble Captures Tangled Remnants of a Supernova","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1848a.jpg" ),
                new Photo("Hubble Celebrates 29th Anniversary with a Colorful Look at the Southern Crab Nebula","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-h-p1915a-m-1707x2000.png" ),
                new Photo("Hubble Spots Flock of Cosmic Ducks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1912a.jpg" ),
                new Photo("Hubble Shows Star Cluster’s True Identity","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1910a.jpg" ),
                new Photo("Triangulum Galaxy Shows Stunning Face in Detailed Hubble Portrait","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/m33.png" ),
                new Photo("Hubble Captures Tangled Remnants of a Supernova","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1848a.jpg" ),
                new Photo("Hubble Shears a Woolly Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_09252015.jpg" ),
                new Photo("Hubble Sees a Spiral Galaxy’s Brights and Darks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_10012015.jpg" ),
                new Photo("Hubble Sees Elegant Spiral Hiding a Hungry Monster","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_10162015.jpg" ),
                new Photo("A Hubble View of Starburst Galaxy Messier 94","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_102315.jpg" ),
                new Photo("Hubble Views a Lonely Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_11132015.jpg" ),
                new Photo("Celebrating 28 Years of the Hubble Space Telescope","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-h-p1821a-m-1699x2000.png" ),
                new Photo("STS-109 Returns to Earth After Servicing the Hubble Telescope","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/sts109-730-034.jpg" ),
                new Photo("Burst of Celestial Fireworks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-gallery-1022a-2000x960.jpg" ),
                new Photo("Hubble’s Cartwheel","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1036a.jpg" ),
                new Photo("All the Glittering Stars","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/img1_0.png" ),
                new Photo("Hubble Frames an Explosive Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1749a.jpg" ),
                new Photo("Hubble Sets Sights on a Galaxy with a Bright Heart","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1843a.jpg" ),
                new Photo("Hubble Captures Tangled Remnants of a Supernova","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1848a.jpg" ),
                new Photo("Hubble Celebrates 29th Anniversary with a Colorful Look at the Southern Crab Nebula","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-h-p1915a-m-1707x2000.png" ),
                new Photo("Hubble Spots Flock of Cosmic Ducks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1912a.jpg" ),
                new Photo("Hubble Shows Star Cluster’s True Identity","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1910a.jpg" ),
                new Photo("Triangulum Galaxy Shows Stunning Face in Detailed Hubble Portrait","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/m33.png" ),
                new Photo("Hubble Captures Tangled Remnants of a Supernova","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1848a.jpg" ),
                new Photo("Veil Nebula Supernova Remnant","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hs-2015-29-a-xlarge_web.jpg" ),
                new Photo("Hubble Shears a Woolly Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_09252015.jpg" ),
                new Photo("Hubble Sees a Spiral Galaxy’s Brights and Darks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_10012015.jpg" ),
                new Photo("Hubble Sees Elegant Spiral Hiding a Hungry Monster","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_10162015.jpg" ),
                new Photo("A Hubble View of Starburst Galaxy Messier 94","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_102315.jpg" ),
                new Photo("Hubble Views a Lonely Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/hubble_friday_11132015.jpg" ),
                new Photo("Celebrating 28 Years of the Hubble Space Telescope","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-h-p1821a-m-1699x2000.png" ),
                new Photo("STS-109 Returns to Earth After Servicing the Hubble Telescope","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/sts109-730-034.jpg" ),
                new Photo("Burst of Celestial Fireworks","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/stsci-gallery-1022a-2000x960.jpg" ),
                new Photo("Hubble’s Cartwheel","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1036a.jpg" ),
                new Photo("All the Glittering Stars","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/img1_0.png" ),
                new Photo("Hubble Frames an Explosive Galaxy","https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/potw1749a.jpg" ),
            };

        public ImageView()
        {
            InitializeComponent();

            _compositor = Window.Current.Compositor;
            VisualExtensions.GetVisual(ImageList).Clip = _compositor.CreateInsetClip();
        }

        public event ImageUpdatedEventHandler ImageUpdated;

        public void LoadImages()
        {
            Banner.Source = new BitmapImage(new Uri(_photos[0].Url));
            BannerTitle.Text = _photos[0].Title;
            ImageUpdated.Invoke(this, new ImageUpdatedEventArgs(Banner.Source));

            // TODO 1.2: [ItemsRepeater] - Auto-select first photo when loaded.
            Repeater.Loaded += async (s, e) =>
            {
                // ChangeView can be called only when the layout has completed, hence waiting in the Loaded event.
                ImageList.ChangeView((Layout.ItemWidth + Layout.Spacing) * 500, null, null, true);
                ScrollToCenterOfViewport(s);

                // Auto-select first photo.
                await Task.Delay(1200);
                var first = Repeater.FindDescendants<RadioButton>().Where(r => r.Tag.Equals(_photos[0].Title)).FirstOrDefault();
                if (first != null) first.IsChecked = true;
            };
        }

        private void OnThumbnailClicked(object sender, RoutedEventArgs e)
        {
            var button = (ButtonBase)sender;

            if (button.FindName("Thumbnail") is Image image)
            {
                Banner.Source = image.Source;
                BannerTitle.Text = button.Tag.ToString();
                BannerEnter.Begin();

                ImageUpdated.Invoke(this, new ImageUpdatedEventArgs(Banner.Source));
            }

            ScrollToCenterOfViewport(sender);
        }

        private static void ScrollToCenterOfViewport(object sender)
        {
            var item = (FrameworkElement)sender;
            item.StartBringIntoView(new BringIntoViewOptions()
            {
                HorizontalAlignmentRatio = 0.5,
                VerticalAlignmentRatio = 0.5,
                AnimationDesired = true,
            });
        }
    }

    public delegate void ImageUpdatedEventHandler(object sender, ImageUpdatedEventArgs args);

    public class ImageUpdatedEventArgs : EventArgs
    {
        public ImageUpdatedEventArgs(ImageSource imageSource)
        {
            ImageSource = imageSource;
        }

        public ImageSource ImageSource { get; }
    }
}
