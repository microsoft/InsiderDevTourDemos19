using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Uwp.App.Helpers
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable), Guid("D8FC9236-B2DE-461C-8043-7A1BB3F96819")]
    public interface VisualReference
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable), Guid("53456138-20B5-4F17-8F6C-F379A1AC98D0")]
    public interface IVisualPartner2
    {
        // Properties
        int DepthMode { get; set; }
        bool DisconnectChildrenOnDestroy { get; set; }
        bool IsTransparentForInput { get; set; }
        float RasterizationScaleOverride { get; set; }
        Vector3 UpVectorOverride { get; set; }

        // Parameters to some of these methods are removed because they are complicated to include, and are unused in the tests.
        int AttachMouseDragToHwnd();
        int AttachMouseWheelToHwnd();
        int CaptureAsync();
        int CaptureOverrideSDRBoostAsync();
        int SetInputSinkHandle();
        int SetParentForTransformFromReference(VisualReference value);
        int GetClosedEventHandle();
    }

    public class LabelBuilder
    {
        public LabelBuilder()
        {
            _labelLookupTable = new Dictionary<SceneNode, LabelEntry>();
        }

        private void AddTransformParentComponent(SceneNode node)
        {
            // Empty mesh signifies a transform parent component.
            node.Components.Add(SceneMeshRendererComponent.Create(node.Compositor));
        }

        private bool HasTransformParentComponent(SceneNode node)
        {
            var meshComponent = node.FindFirstComponentOfType(SceneComponentType.MeshRendererComponent) as SceneMeshRendererComponent;
            return (meshComponent != null) && (meshComponent.Mesh == null);
        }

        public void AddPositionedLabelNode(
            UIElement xamlLabelElement,
            Vector3 positionOfDotInNodeSpace,
            SceneNode anchorNode,
            Color labelColor,
            Vector3 labelOffset
            )
        {
            var tempNode = SceneNode.Create(anchorNode.Compositor);

            tempNode.Transform.Translation = positionOfDotInNodeSpace;

            anchorNode.Children.Add(tempNode);

            AddLabelNode(
                xamlLabelElement,
                tempNode,
                labelColor,
                new Vector2(xamlLabelElement.ActualSize.X, xamlLabelElement.ActualSize.Y),
                labelOffset
                );
        }

        public void AddLabelNode(
            UIElement xamlLabelElement,
            SceneNode anchorNode,
            Color labelColor,
            Vector2 labelSize,
            Vector3 labelOffset)
        {
            var compositor = anchorNode.Compositor;

            var interopNode = SceneNode.Create(compositor);
            AddTransformParentComponent(interopNode);
            anchorNode.Children.Add(interopNode);

            // Create visual that corresponds to the interopNode.
            // Interop-node will modify this visual's offset.
            var interopVisual = compositor.CreateContainerVisual();

            // Create label box with a line connecting its horizontal midpoint to the anchor node.
            var labelVisual = compositor.CreateShapeVisual();
            {
                var strokeBrush = compositor.CreateColorBrush(Colors.Black);
                float strokeThickness = 2;

                var labelViewBox = compositor.CreateViewBox();
                labelViewBox.Offset = new Vector2(
                    -(0.5f * labelSize.X + strokeThickness + Math.Abs(labelOffset.X)),
                    -(labelSize.Y + strokeThickness + Math.Abs(labelOffset.Y)));
                labelViewBox.Size = 2 * Vector2.Abs(labelViewBox.Offset);

                labelVisual.Size = labelViewBox.Size;
                labelVisual.ParentForTransform = interopVisual;
                labelVisual.Offset = new Vector3(labelViewBox.Offset, labelOffset.Z);
                labelVisual.Opacity = 1.0f;
                labelVisual.ViewBox = labelViewBox;

                var anchorDot = compositor.CreateEllipseGeometry();
                anchorDot.Center = Vector2.Zero;
                anchorDot.Radius = new Vector2(3 * strokeThickness);

                var anchorDotShape = compositor.CreateSpriteShape(anchorDot);
                anchorDotShape.FillBrush = strokeBrush;
                labelVisual.Shapes.Add(anchorDotShape);

                var labelLine = compositor.CreateLineGeometry();
                labelLine.Start = Vector2.Zero;
                labelLine.End = new Vector2(labelOffset.X, labelOffset.Y);

                var labelLineShape = compositor.CreateSpriteShape(labelLine);
                labelLineShape.StrokeBrush = strokeBrush;
                labelLineShape.StrokeThickness = strokeThickness;
                labelLineShape.StrokeDashArray.Add(2 * strokeThickness);
                labelVisual.Shapes.Add(labelLineShape);

                var xamlVisual = ElementCompositionPreview.GetElementVisual(xamlLabelElement);

                // Need to figure out proper positioning of elements
                if (labelOffset.X != 0.0f)
                {
                    xamlVisual.Offset = new Vector3(labelOffset.X + labelSize.X + labelSize.Y / 2, labelSize.Y / 2.0f, 0.0f);
                }

                labelVisual.Children.InsertAtTop(xamlVisual);
            }

            LabelEntry label;
            label.LabelVisual = labelVisual;
            label.InteropVisual = interopVisual;
            _labelLookupTable.Add(interopNode, label);
        }

        public void BuildLabels(
            ContainerVisual labelRoot,
            SceneVisual sceneVisual)
        {
            BuildLabelsWorker(labelRoot, sceneVisual, sceneVisual.Root);
        }

        private void BuildLabelsWorker(
            ContainerVisual labelRoot,
            SceneVisual sceneVisual,
            SceneNode node)
        {
            if (HasTransformParentComponent(node))
            {
                var label = _labelLookupTable[node];
                labelRoot.Children.InsertAtTop(label.LabelVisual);
                sceneVisual.Children.InsertAtTop(label.InteropVisual);
            }

            foreach (var child in node.Children)
            {
                BuildLabelsWorker(labelRoot, sceneVisual, child);
            }
        }

        struct LabelEntry
        {
            public Visual LabelVisual;
            public Visual InteropVisual;
        }

        private Dictionary<SceneNode, LabelEntry> _labelLookupTable;
    }
}
