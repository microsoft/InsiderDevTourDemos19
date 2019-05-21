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

            // TODO 6.2: [AnimatedVisualPlayer] - Show the Lottie animation while the 3D model is loading.
            LottiePlayer.Visibility = Visibility.Visible;
            LottiePlayer.AutoPlay = true;

            HubbleImage.Visibility = Visibility.Collapsed;

            // TODO 5.1: [SceneLoader] - Load the .gltf 3D model into a SceneNode.
            var uri = new Uri("ms-appx:///Assets/Models/Telescope.gltf");
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var buffer = await FileIO.ReadBufferAsync(storageFile);
            var loader = new SceneLoader();
            var sceneNode = loader.Load(buffer, _compositor);

            // TODO 6.3: [AnimatedVisualPlayer] - Hide the Lottie animation after the 3D model finished loading.
            LottiePlayer.Stop();
            await LottiePlayer.Fade(duration: 600).StartAsync();

            // TODO 5.2: [SceneLoader] - Use a UIElement ModelHost to host _hostVisual that hosts SceneVisual that hosts SceneNode.
            _hostVisual.RelativeSizeAdjustment = Vector2.One;
            ElementCompositionPreview.SetElementChildVisual(ModelHost, _hostVisual);
            _sceneVisual.Scale = new Vector3(2.5f, 2.5f, 1.0f);
            _sceneVisual.Root = SceneNode.Create(_compositor);
            _sceneVisual.Root.Children.Clear();
            _sceneVisual.Root.Children.Add(sceneNode);

            // Auto-rotate the SceneVisual.
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
