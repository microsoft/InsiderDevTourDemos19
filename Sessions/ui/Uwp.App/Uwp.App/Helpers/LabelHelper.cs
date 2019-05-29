using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics;
using Windows.Graphics.DirectX;
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

    public class LabelHelper
    {
        // This will add a pipe of designated radius of the base circle and the
        // specified number of vertices around it's circumference,
        static public SceneNode AddPipe(
            SceneNode rootNode,
            float radius,
            Vector3 startPoint,
            Vector3 endPoint,
            Vector4 color,
            int numVerticesAroundCircumference = 16
            )
        {
            var newPipeNode = SceneNode.Create(rootNode.Compositor);
            var newMaterial = SceneMetallicRoughnessMaterial.Create(rootNode.Compositor);

            rootNode.Children.Add(newPipeNode);

            var mesh = SceneMesh.Create(rootNode.Compositor);
            var renderer = SceneMeshRendererComponent.Create(rootNode.Compositor);

            newPipeNode.Components.Add(renderer);

            renderer.Mesh = mesh;
            renderer.Material = newMaterial;

            newMaterial.BaseColorFactor = color;

            Vector3 forwardVector = endPoint - startPoint;

            // We need to pick vectors to represent forward, right, and up to orient the pipe
            // Forward is already supplied to us by the start -> end point.
            // Up and Right don't really matter to us as long as they form a proper coordinate space
            // and they are all in directions perpindicular to each other.
            //
            // We'll first try using Positive Z as a reference vector to a perpindicular "Right", but
            // if the vector is already going along the z-axis we need to use a different reference
            // axis.

            Vector3 referenceVectorToAcquireRightVector;

            if (endPoint.X - startPoint.X != 0.0f || endPoint.Y - startPoint.Y != 0.0f)
            {
                // Use Positive Z axis to acquire a "right vector" using a cross product
                referenceVectorToAcquireRightVector = new Vector3(0.0f, 0.0f, 1.0f);
            }
            else
            {
                // Positive Z axis is co-linear, let's use the negative y axis
                referenceVectorToAcquireRightVector = new Vector3(0.0f, -1.0f, 0.0f);
            }

            Vector3 forwardAxis = Vector3.Normalize(forwardVector);

            // We use the forwardAxis, the referenceVector, and their cross product to get us a rightAxis.
            // The referenceVector doesn't reallly matter what it is very much, what's important
            // is the rightAxis from the CrossProduct will be perpendicular to our forward vector.
            Vector3 rightAxis = Vector3.Cross(forwardAxis, referenceVectorToAcquireRightVector);

            // Now that we have a forward and a right, we can cross again to get an up.
            Vector3 upAxis = Vector3.Cross(rightAxis, forwardAxis);

            SceneHelper.FillMeshWithPipe(
                mesh,
                radius,
                startPoint,
                endPoint,
                forwardAxis,
                upAxis,
                rightAxis,
                numVerticesAroundCircumference
                );

            return newPipeNode;
        }

        static public SceneNode AddSphere(
            SceneNode rootNode,
            float radius,
            Vector3 offset,
            Vector4 color
            )
        {
            var newSphereNode = SceneNode.Create(rootNode.Compositor);
            var newMaterial = SceneMetallicRoughnessMaterial.Create(rootNode.Compositor);

            rootNode.Children.Add(newSphereNode);

            var mesh = SceneMesh.Create(rootNode.Compositor);
            var renderer = SceneMeshRendererComponent.Create(rootNode.Compositor);

            newSphereNode.Components.Add(renderer);

            renderer.Mesh = mesh;
            renderer.Material = newMaterial;

            newMaterial.BaseColorFactor = color;

            newSphereNode.Transform.Translation = offset;

            SceneHelper.FillMeshWithSphere(mesh, radius, 64);

            return newSphereNode;
        }

        static public async void AddLabel(
            SceneNode rootNode,
            List<SceneNode> labelNodes,
            Vector2 labelSize,
            Vector3 offset,
            Vector2 anchorPoint,
            string labelFileName)
        {
            var newLabelNode = SceneNode.Create(rootNode.Compositor);
            var newLabelParentNode = SceneNode.Create(rootNode.Compositor);
            var newMaterial = SceneMetallicRoughnessMaterial.Create(rootNode.Compositor);

            labelNodes.Add(newLabelNode);

            var newLabelMesh = SceneMesh.Create(rootNode.Compositor);
            var newLabelRenderer = SceneMeshRendererComponent.Create(rootNode.Compositor);

            newLabelParentNode.Children.Add(newLabelNode);

            SceneHelper.FillMeshWithSquare(newLabelMesh, labelSize.X, labelSize.Y, anchorPoint.X, anchorPoint.Y);

            newLabelRenderer.Mesh = newLabelMesh;
            newLabelRenderer.Material = newMaterial;

            newLabelNode.Components.Add(newLabelRenderer);

            rootNode.Children.Add(newLabelParentNode);

            newLabelNode.Transform.Translation = offset;

            var compositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(rootNode.Compositor, CanvasDevice.GetSharedDevice());

            SizeInt32 maxSize;

            maxSize.Width = (int)labelSize.X;
            maxSize.Height = (int)labelSize.Y;

            var mipmapSurface = compositionGraphicsDevice.CreateMipmapSurface(
                maxSize,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            int levelWidth = maxSize.Width;
            int levelHeight = maxSize.Height;

            var labelColorInput = SceneSurfaceMaterialInput.Create(rootNode.Compositor);

            labelColorInput.Surface = mipmapSurface;
            labelColorInput.BitmapInterpolationMode = CompositionBitmapInterpolationMode.MagLinearMinLinearMipLinear;

            newMaterial.BaseColorInput = labelColorInput;

            bool fMoreProcessing = true;
            uint mipLevel = 0;

            while (fMoreProcessing)
            {
                CompositionDrawingSurface level = mipmapSurface.GetDrawingSurfaceForLevel(mipLevel);

                {
                    using (var session = CanvasComposition.CreateDrawingSession(level))
                    {
                        CanvasBitmap bitmapFromFile = await CanvasBitmap.LoadAsync(session, labelFileName);

                        try
                        {
                            {
                                session.DrawImage(
                                    bitmapFromFile,
                                    new Rect(
                                        0,
                                        0,
                                        levelWidth,
                                        levelHeight));

                            }
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

            }// For loop
        }

    }
}
