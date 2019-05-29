using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using SceneLoaderComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Uwp.App.Helpers;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Graphics;
using Windows.Graphics.DirectX;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Composition.Interactions;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace Uwp.App.Pages
{
    public sealed partial class HubblePage : Page
    {
        private readonly Compositor _compositor;
        private readonly ContainerVisual _hostVisual;
        private readonly SceneVisual _sceneVisual;

        //        private readonly LabelBuilder _labelBuilder = new LabelBuilder();
        private List<SceneNode> _labelNodes;
        private List<SceneNode> _labelParentNodes;
        private List<SceneNode> _sphereNodes;
        private List<SceneNode> _pipeNodes;
        private List<SceneMetallicRoughnessMaterial> _labelMaterials;
        private static readonly Vector3 _defaultRotationAxis = new Vector3(0.0f, 1.0f, 0.2f);
        private VisualInteractionSource _interactionSource;
        private InteractionTracker _tracker;

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

            // TODO 5.1: [SceneLoader] - Load a .gltf 3D model into a SceneNode (new API in Composition).
            var uri = new Uri("ms-appx:///Assets/Models/Telescope.gltf");
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var buffer = await FileIO.ReadBufferAsync(storageFile);
            var loader = new SceneLoader();
            var sceneNode = loader.Load(buffer, _compositor);

            // TODO 5.2: [SceneLoader] - Host the 3D SceneNode inside a XAML element (ModelHost Rectangle).
            // NOTE: 3D Xaml Control coming too! It is being tracked here:  
            // https://github.com/microsoft/microsoft-ui-xaml/issues/686
            ElementCompositionPreview.SetElementChildVisual(ModelHost, _hostVisual);
            _sceneVisual.Scale = new Vector3(new Vector2(2.5f), 1.0f);
            _sceneVisual.Opacity = 0.0f;
            _sceneVisual.Root = SceneNode.Create(_compositor);
            _sceneVisual.Root.Children.Clear();
            _sceneVisual.Root.Children.Add(sceneNode);
            _hostVisual.Children.InsertAtTop(_sceneVisual);
            // Build the labels that are surrounding the 3D model.
            await BuildLabels();

            // TODO 6.3: [AnimatedVisualPlayer] - Hide the Lottie animation after the 3D model finished loading.
            LottiePlayer.Stop();
            await LottiePlayer.Fade(duration: 600).StartAsync();

            // Auto-rotate the 3D model and its surrounding labels.
            RotateModelAndLabels();

            // Use InteractionTracker to allow zoom in/out the model.
            AttachZoomInteractionToModel();
        }

        private async Task BuildLabels()
        {
            _labelNodes = new List<SceneNode>();
            _labelParentNodes = new List<SceneNode>();
            _sphereNodes = new List<SceneNode>();
            _labelMaterials = new List<SceneMetallicRoughnessMaterial>();
            _pipeNodes = new List<SceneNode>();

            await AddLabel(Label1, new Vector3(160.0f, -200.0f, -4.0f), new Vector2(59.0f, 0.0f));
            await AddLabel(Label2, new Vector3(-200.0f, 60.0f, -8.0f), new Vector2(67.0f, 0.0f));
            await AddLabel(Label3, new Vector3(120.0f, 60.0f, -8.0f), new Vector2(65.0f, 0.0f));

            var color = new Vector4(new Vector3(50.0f, 160.0f, 95.0f) / 255.0f, 1.0f);

            var solarPanelLabelPin = new Vector3(120.0f, 62.0f, -8.0f);
            var solarPanelPin = new Vector3(10.0f, -40.0f, 70.0f);
            AddSphere(2.0f, solarPanelPin, color);
            //AddSphere(2.0f, solarPanelLabelPin, color);
            AddPipe(0.25f, solarPanelPin, solarPanelLabelPin, color);

            var secondaryMirrorLabelPin = new Vector3(-200.0f, 61.0f, -8.0f);
            var secondaryMirrorPin = new Vector3(60.0f, 0.0f, 0.0f);
            AddSphere(2.0f, secondaryMirrorPin, color);
            //AddSphere(2.0f, secondaryMirrorLabelPin, color);
            AddPipe(0.25f, secondaryMirrorPin, secondaryMirrorLabelPin, color);

            var primaryMirrorLabelPin = new Vector3(160.0f, -152.0f, -4.0f);
            var primaryMirrorPin = new Vector3(122.0f, -50.0f, 0.0f);
            AddSphere(2.0f, primaryMirrorPin, color);
            //AddSphere(2.0f, primaryMirrorLabelPin, color);
            AddPipe(0.25f, primaryMirrorPin, primaryMirrorLabelPin, color);
        }

        private void AttachZoomInteractionToModel()
        {
            var rootVisual = VisualExtensions.GetVisual(LayoutRoot);
            rootVisual.Size = _hostVisual.Size = LayoutRoot.RenderSize.ToVector2();
            _hostVisual.CenterPoint = new Vector3(_hostVisual.Size / 2, 0.0f);

            _interactionSource = VisualInteractionSource.Create(rootVisual);
            _interactionSource.PositionXSourceMode = InteractionSourceMode.EnabledWithInertia;
            _interactionSource.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;
            _interactionSource.IsPositionXRailsEnabled = false;
            _interactionSource.IsPositionYRailsEnabled = false;
            _interactionSource.ScaleSourceMode = InteractionSourceMode.EnabledWithInertia;
            _interactionSource.ManipulationRedirectionMode = VisualInteractionSourceRedirectionMode.CapableTouchpadOnly;

            _tracker = InteractionTracker.Create(_compositor);
            _tracker.InteractionSources.Add(_interactionSource);
            _tracker.MaxPosition = new Vector3(rootVisual.Size, 0.0f) * 3.0f;
            _tracker.MinPosition = _tracker.MaxPosition * -1.0f;
            _tracker.MinScale = 0.9f;
            _tracker.MaxScale = 2.0f;
            _tracker.ScaleInertiaDecayRate = 0.96f;

            var scaleAnimation = _compositor.CreateExpressionAnimation("Vector2(t.Scale, t.Scale)");
            scaleAnimation.SetReferenceParameter("t", _tracker);
            _hostVisual.StartAnimation("Scale.XY", scaleAnimation);
        }

        private void RotateModelAndLabels()
        {
            _sceneVisual.Opacity = 1.0f;
            _sceneVisual.Root.Transform.RotationAxis = _defaultRotationAxis;

            var scaleAnimation = _compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(0.0f, Vector3.Zero);
            scaleAnimation.InsertKeyFrame(1.0f, Vector3.One);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(400);

            var rotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
            rotationAnimation.InsertKeyFrame(1.0f, 360.0f, _compositor.CreateLinearEasingFunction());
            rotationAnimation.Duration = TimeSpan.FromSeconds(20);
            rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            var inverseRotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
            inverseRotationAnimation.InsertKeyFrame(0.0f, 360.0f, _compositor.CreateLinearEasingFunction());
            inverseRotationAnimation.InsertKeyFrame(1.0f, 0.0f, _compositor.CreateLinearEasingFunction());
            inverseRotationAnimation.Duration = TimeSpan.FromSeconds(20);
            inverseRotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            foreach (var node in _sphereNodes)
            {
                node.Transform.StartAnimation(nameof(SceneNode.Transform.Scale), scaleAnimation);
            }

            foreach (var node in _pipeNodes)
            {
                node.Transform.StartAnimation(nameof(SceneNode.Transform.Scale), scaleAnimation);
            }

            foreach (var node in _labelNodes)
            {
                node.Transform.RotationAxis = _defaultRotationAxis;
                node.Transform.StartAnimation(nameof(SceneNode.Transform.RotationAngleInDegrees), inverseRotationAnimation);
                node.Transform.StartAnimation(nameof(SceneNode.Transform.Scale), scaleAnimation);
            }

            _sceneVisual.Root.Transform.RotationAxis = _defaultRotationAxis;
            _sceneVisual.Root.Transform.StartAnimation(nameof(SceneNode.Transform.RotationAngleInDegrees), rotationAnimation);
        }

        private async Task AddLabel(UIElement element, Vector3 offset, Vector2 anchorPoint)
        {
            var labelSize = element.RenderSize.ToVector2();

            var newLabelNode = SceneNode.Create(_compositor);
            var newLabelParentNode = SceneNode.Create(_compositor);
            var newMaterial = SceneMetallicRoughnessMaterial.Create(_compositor);

            _labelParentNodes.Add(newLabelParentNode);
            _labelNodes.Add(newLabelNode);
            _labelMaterials.Add(newMaterial);

            var newLabelMesh = SceneMesh.Create(_compositor);
            var newLabelRenderer = SceneMeshRendererComponent.Create(_compositor);

            newLabelParentNode.Children.Add(newLabelNode);

            SceneHelper.FillMeshWithSquare(newLabelMesh, labelSize.X / _sceneVisual.Scale.X, labelSize.Y / _sceneVisual.Scale.X, anchorPoint.X, anchorPoint.Y);

            newLabelRenderer.Mesh = newLabelMesh;
            newLabelRenderer.Material = newMaterial;

            newLabelNode.Components.Add(newLabelRenderer);
            _sceneVisual.Root.Children.Add(newLabelParentNode);

            newLabelNode.Transform.Translation = offset;
            newLabelNode.Transform.Scale = Vector3.Zero;

            var compositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, CanvasDevice.GetSharedDevice());

            SizeInt32 maxSize;
            maxSize.Width = (int)labelSize.X;
            maxSize.Height = (int)labelSize.Y;

            var mipmapSurface = compositionGraphicsDevice.CreateMipmapSurface(
                maxSize,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            int levelWidth = maxSize.Width;
            int levelHeight = maxSize.Height;

            var labelColorInput = SceneSurfaceMaterialInput.Create(_compositor);

            labelColorInput.Surface = mipmapSurface;
            labelColorInput.BitmapInterpolationMode = CompositionBitmapInterpolationMode.MagLinearMinLinearMipLinear;

            newMaterial.BaseColorInput = labelColorInput;

            bool fMoreProcessing = true;
            uint mipLevel = 0;

            while (fMoreProcessing)
            {
                var level = mipmapSurface.GetDrawingSurfaceForLevel(mipLevel);
                {
                    using (var session = CanvasComposition.CreateDrawingSession(level))
                    {
                        var bitmapFromFile = await CanvasBitmap.LoadAsync(session, await element.ToRandomAccessStream());

                        try
                        {
                            session.DrawImage(bitmapFromFile, new Rect(0, 0, levelWidth, levelHeight));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (levelWidth == 1 && levelHeight == 1)
                {
                    fMoreProcessing = false;
                }

                mipLevel++;

                levelWidth /= 2;
                levelHeight /= 2;

                if (levelWidth <= 0)
                {
                    levelWidth = 1;
                }

                if (levelHeight <= 0)
                {
                    levelHeight = 1;
                }

            } // For loop
        }

        private void AddSphere(float radius, Vector3 offset, Vector4 color)
        {
            var newSphereNode = SceneNode.Create(_compositor);
            var newMaterial = SceneMetallicRoughnessMaterial.Create(_compositor);

            _sceneVisual.Root.Children.Add(newSphereNode);

            var mesh = SceneMesh.Create(_compositor);
            var renderer = SceneMeshRendererComponent.Create(_compositor);

            newSphereNode.Components.Add(renderer);

            renderer.Mesh = mesh;
            renderer.Material = newMaterial;

            newMaterial.BaseColorFactor = color;

            newSphereNode.Transform.Translation = offset;
            newSphereNode.Transform.Scale = new Vector3(0.0f, 0.0f, 0.0f);

            SceneHelper.FillMeshWithSphere(mesh, radius, 64);

            _sphereNodes.Add(newSphereNode);
        }

        private void AddPipe(float radius, Vector3 start, Vector3 end, Vector4 color)
        {
            var pipeNode = LabelHelper.AddPipe(_sceneVisual.Root, radius, start, end, color);

            _pipeNodes.Add(pipeNode);

            pipeNode.Transform.Scale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch) return;

            _interactionSource?.TryRedirectForManipulation(e.GetCurrentPoint(LayoutRoot));
        }
    }
}
