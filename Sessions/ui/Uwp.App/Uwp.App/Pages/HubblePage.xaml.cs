using Microsoft.Toolkit.Uwp.UI.Animations;
using SceneLoaderComponent;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;

namespace Uwp.App.Pages
{
    public sealed partial class HubblePage : Page
    {
        private readonly Compositor _compositor;
        private readonly ContainerVisual _hostVisual;
        private readonly SceneVisual _sceneVisual;

        public HubblePage()
        {
            InitializeComponent();

            _compositor = Window.Current.Compositor;
            _hostVisual = _compositor.CreateContainerVisual();
            _sceneVisual = SceneVisual.Create(_compositor);
        }

        private void ModelHost_SizeChanged(object sender, SizeChangedEventArgs e) =>
            UpdateModelPosition();

        private void UpdateModelPosition() =>
            _sceneVisual.Offset = new Vector3((float)ModelHost.ActualWidth / 2, (float)ModelHost.ActualHeight / 2.5f, 0.0f);

        private async void Surprise_Click(object sender, RoutedEventArgs e)
        {
            if (_hostVisual.Children.Any()) return;

            LottiePlayer.Visibility = Visibility.Visible;
            LottiePlayer.AutoPlay = true;

            HubbleImage.Fade(duration: 800).Start();

            var uri = new Uri("ms-appx:///Assets/Models/Telescope.gltf");
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var buffer = await FileIO.ReadBufferAsync(storageFile);

            var loader = new SceneLoader();
            var model = loader.Load(buffer, _compositor);

            LottiePlayer.Stop();
            await LottiePlayer.Fade(duration: 600).StartAsync();

            _hostVisual.RelativeSizeAdjustment = Vector2.One;
            ElementCompositionPreview.SetElementChildVisual(ModelHost, _hostVisual);

            _sceneVisual.Scale = new Vector3(2.5f, 2.5f, 1.0f);
            _sceneVisual.Root = SceneNode.Create(_compositor);
            _sceneVisual.Root.Children.Clear();
            _sceneVisual.Root.Children.Add(model);

            var rotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
            rotationAnimation.InsertKeyFrame(1.0f, 360.0f, _compositor.CreateLinearEasingFunction());
            rotationAnimation.Duration = TimeSpan.FromSeconds(16);
            rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            _sceneVisual.Root.Transform.RotationAxis = new Vector3(0.0f, 1.0f, 0.2f); ;
            _sceneVisual.Root.Transform.StartAnimation(nameof(SceneNode.Transform.RotationAngleInDegrees), rotationAnimation);

            _hostVisual.Children.InsertAtTop(_sceneVisual);
        }
    }
}
