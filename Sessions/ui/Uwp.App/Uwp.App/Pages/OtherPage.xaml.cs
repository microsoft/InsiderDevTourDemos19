using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp.App.Pages
{
    public sealed partial class OtherPage : Page
    {
        public OtherPage()
        {
            InitializeComponent();

            Title.Opacity = 0;
            Loaded += (s, e) =>
            {
                Title.Fade(1.0f, duration: 1600, delay: 600).Start();
                ShowTextShimming();
            };
        }

        private void ShowTextShimming()
        {
            var compositor = Window.Current.Compositor;
            var titleVisual = VisualExtensions.GetVisual(Title);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = Colors.White;
            pointLight.CoordinateSpace = titleVisual;
            pointLight.Targets.Add(titleVisual);
            pointLight.Offset = new Vector3(-(float)Title.ActualWidth * 5, (float)Title.ActualHeight / 2, 100.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)Title.ActualWidth * 5, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(5000);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
        }
    }
}
