using System;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.Foundation;
using Windows.UI.Composition.Scenes;
using System.Numerics;

namespace Uwp.App.Helpers
{
    [ComImport, Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMemoryBufferByteAccess
    {
        unsafe void GetBuffer(byte** bytes, uint* capacity);
    }

    public class SceneHelper
    {
        // Copied from WrtCompositionMipmapSurface.cpp
        public static uint CalcMipLevel(SizeInt32 size)
        {
            int length = Math.Max(size.Height, size.Width);
            uint mipLevel = 0;
            while (length > 1)
            {
                length /= 2;
                mipLevel++;
            }

            uint maxMipLevel = 15; // Base3D has a limit of max mip level = 15
            return Math.Min(maxMipLevel, mipLevel + 1);
        }

        public static MemoryBuffer CreateMemoryBufferWithArray(float[] srcArray)
        {
            Func<float, byte[]> serializer = value => BitConverter.GetBytes(value);
            return CreateMemoryBufferWithArrayWorker(srcArray, serializer, (uint)(srcArray.Length * sizeof(float)));
        }

        public static MemoryBuffer CreateMemoryBufferWithArray(UInt16[] srcArray)
        {
            Func<UInt16, byte[]> serializer = value => BitConverter.GetBytes(value);
            return CreateMemoryBufferWithArrayWorker(srcArray, serializer, (uint)(srcArray.Length * sizeof(UInt16)));
        }

        public static MemoryBuffer CreateMemoryBufferWithArray(UInt32[] srcArray)
        {
            Func<UInt32, byte[]> serializer = value => BitConverter.GetBytes(value);
            return CreateMemoryBufferWithArrayWorker(srcArray, serializer, (uint)(srcArray.Length * sizeof(UInt32)));
        }

        private static MemoryBuffer CreateMemoryBufferWithArrayWorker<T>(
           T[] srcArray,
           Func<T, byte[]> serializer,
           uint bufferSize)
        {
            MemoryBuffer mb = new MemoryBuffer(bufferSize);
            IMemoryBufferReference mbr = mb.CreateReference();
            IMemoryBufferByteAccess mba = (IMemoryBufferByteAccess)mbr;

            unsafe
            {
                byte* destBytes = null;
                uint destCapacity;
                mba.GetBuffer(&destBytes, &destCapacity);

                int iWrite = 0;
                foreach (T srcValue in srcArray)
                {
                    foreach (byte srcByte in serializer(srcValue))
                    {
                        destBytes[iWrite++] = srcByte;
                    }
                }
            }

            return mb;
        }

        // Will Create a cylinder with a circle centered at <0,0,0> expanding along x/y axis with supplied radius, 
        // and then elongated along z-axis with length.
        //
        // This can also be known as (x*x + y*y) = radius*radius with a clip restricting the z between 0 and length

        public static void FillMeshWithPipe(
            SceneMesh mesh,
            float radius,
            Vector3 startPoint,
            Vector3 endPoint,
            Vector3 forwardAxis,
            Vector3 upAxis,
            Vector3 rightAxis,
            int numRequestedVertices
            )
        {
            int numVerticesAroundCircumference = 16;

            // Need to have vertices going around the circle, that has to happen twice
            // once for each end of the pipe.  Then we need a 2nd set of all of them
            // because the end caps will have different normals.
            int numVerticesTotal = numVerticesAroundCircumference * 2 * 2;

            Vector3[] verticesModelSpace = new Vector3[numVerticesTotal];
            Vector3[] normalsModelSpace = new Vector3[numVerticesTotal];

            Vector2[] ringVerticesCircleSpace = new Vector2[numVerticesAroundCircumference];

            for (int i = 0; i < numVerticesAroundCircumference; i++)
            {
                float angle = (float)i * (2.0f / (float)numVerticesAroundCircumference) * (float)Math.PI;

                ringVerticesCircleSpace[i].X = (float)Math.Cos(angle);
                ringVerticesCircleSpace[i].Y = (float)Math.Sin(angle);
            }

            int iCurrentVertex = 0;

            for (int i = 0; i < numVerticesAroundCircumference; i++)
            {
                verticesModelSpace[iCurrentVertex] = startPoint +
                    ringVerticesCircleSpace[i].X * rightAxis * radius +
                    ringVerticesCircleSpace[i].Y * upAxis * radius;

                normalsModelSpace[iCurrentVertex] =
                    ringVerticesCircleSpace[i].X * rightAxis +
                    ringVerticesCircleSpace[i].Y * upAxis;

                normalsModelSpace[iCurrentVertex] = Vector3.Normalize(normalsModelSpace[iCurrentVertex]);

                iCurrentVertex++;
            }

            for (int i = 0; i < numVerticesAroundCircumference; i++)
            {
                verticesModelSpace[iCurrentVertex] = endPoint +
                    ringVerticesCircleSpace[i].X * rightAxis * radius +
                    ringVerticesCircleSpace[i].Y * upAxis * radius;

                normalsModelSpace[iCurrentVertex] =
                    ringVerticesCircleSpace[i].X * rightAxis +
                    ringVerticesCircleSpace[i].Y * upAxis;

                normalsModelSpace[iCurrentVertex] = Vector3.Normalize(normalsModelSpace[iCurrentVertex]);

                iCurrentVertex++;
            }

            for (int i = 0; i < numVerticesAroundCircumference; i++)
            {
                verticesModelSpace[iCurrentVertex] = startPoint +
                    ringVerticesCircleSpace[i].X * rightAxis * radius +
                    ringVerticesCircleSpace[i].Y * upAxis * radius;

                normalsModelSpace[iCurrentVertex] = Vector3.Negate(forwardAxis);

                iCurrentVertex++;
            }

            for (int i = 0; i < numVerticesAroundCircumference; i++)
            {
                verticesModelSpace[iCurrentVertex] = endPoint +
                    ringVerticesCircleSpace[i].X * rightAxis * radius +
                    ringVerticesCircleSpace[i].Y * upAxis * radius;

                normalsModelSpace[iCurrentVertex] = forwardAxis;

                iCurrentVertex++;
            }

            float[] tempFloatArray = new float[numVerticesTotal * 3];

            int iCurrentFloat = 0;

            for (int i = 0; i < numVerticesTotal; i++)
            {
                tempFloatArray[iCurrentFloat++] = verticesModelSpace[i].X;
                tempFloatArray[iCurrentFloat++] = verticesModelSpace[i].Y;
                tempFloatArray[iCurrentFloat++] = verticesModelSpace[i].Z;
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Vertex,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(tempFloatArray)
               );

            iCurrentFloat = 0;

            for (int i = 0; i < numVerticesTotal; i++)
            {
                tempFloatArray[iCurrentFloat++] = normalsModelSpace[i].X;
                tempFloatArray[iCurrentFloat++] = normalsModelSpace[i].Y;
                tempFloatArray[iCurrentFloat++] = normalsModelSpace[i].Z;
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Normal,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(tempFloatArray)
               );

            UInt32[] colors = new UInt32[numVerticesTotal];

            for (int i = 0; i < numVerticesTotal; i++)
            {
                colors[i] = 0xffffffff;
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Color,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32UInt,
               SceneHelper.CreateMemoryBufferWithArray(colors));

            int numIndices = numVerticesAroundCircumference * 3 * 2;

            UInt16[] indices = new UInt16[numIndices + 3 * 2 * (numVerticesAroundCircumference - 1)];

            int curIndex = 0;

            for (UInt16 i = 0; i < numVerticesAroundCircumference; i++)
            {
                indices[curIndex++] = (UInt16)i;
                indices[curIndex++] = (UInt16)(i + numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)((i + 1) % numVerticesAroundCircumference);

                indices[curIndex++] = (UInt16)((i + 1) % numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)(i + numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)((i + 1) % numVerticesAroundCircumference + numVerticesAroundCircumference);
            }

            for (UInt16 i = 1; i < numVerticesAroundCircumference; i++)
            {
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference + i);
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference + (i + 1) % numVerticesAroundCircumference);
            }

            for (UInt16 i = 1; i < numVerticesAroundCircumference; i++)
            {
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference + numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference + (i + 1) % numVerticesAroundCircumference + numVerticesAroundCircumference);
                indices[curIndex++] = (UInt16)(2 * numVerticesAroundCircumference + i + numVerticesAroundCircumference);
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Index,
               Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt,
               SceneHelper.CreateMemoryBufferWithArray(indices));
        }

        public static void FillMeshWithSphere(SceneMesh mesh, float radius = 200, int numApproxVertices = 9)
        {
            //
            // Use Longitude and Latitude to create a mesh
            //
            // Latitude of 0 is top of sphere
            // Latitude of PI is bottom of sphere
            //
            // Longitude of 0 is x = 1, y = 0
            //

            int numLevels = (int)Math.Ceiling(Math.Sqrt(numApproxVertices));

            if (numLevels < 3)
            {
                numLevels = 3;
            }

            float[] sphereHeights = new float[numLevels];
            float[] sphereMult = new float[numLevels];
            float[] sphereLongX = new float[numLevels];
            float[] sphereLongY = new float[numLevels];

            float flStart = (float)Math.PI / 2;
            float flStop = (float)-Math.PI / 2;

            for (int latLevel = 0; latLevel < numLevels; latLevel++)
            {
                float interp = ((float)latLevel) / (float)(numLevels - 1);

                sphereHeights[latLevel] = (float)Math.Sin((1.0f - interp) * flStart + interp * flStop);
                sphereMult[latLevel] = (float)Math.Cos((1.0f - interp) * flStart + interp * flStop);
            }

            flStart = 0.0f;
            flStop = 2.0f * (float)Math.PI;

            for (int longLevel = 0; longLevel < numLevels; longLevel++)
            {
                float interp = ((float)longLevel) / (float)(numLevels);

                sphereLongX[longLevel] = (float)Math.Cos((1.0f - interp) * flStart + interp * flStop);
                sphereLongY[longLevel] = (float)Math.Sin((1.0f - interp) * flStart + interp * flStop);
            }

            // Start with the triangles coming from the north/south pole of the sphere
            int numVertices = 2;
            int numTriangles = 2 * numLevels;
            int numInteriorRows = 0;

            numVertices += numLevels * (numLevels - 2);

            numInteriorRows = numLevels - 2;

            if (numLevels > 3)
            {
                numTriangles += numInteriorRows * numLevels * 2;
            }

            var spherePositions = new float[numVertices * 3];

            int currentVertex = 0;

            // North Pole
            spherePositions[currentVertex * 3] = 0.0f;
            spherePositions[currentVertex * 3 + 1] = 0.0f;
            spherePositions[currentVertex * 3 + 2] = sphereHeights[0];

            currentVertex++;

            for (int intRow = 0; intRow < numInteriorRows; intRow++)
            {
                for (int iCurLong = 0; iCurLong < numLevels; iCurLong++)
                {
                    spherePositions[currentVertex * 3] = sphereLongX[iCurLong] * sphereMult[intRow + 1];
                    spherePositions[currentVertex * 3 + 1] = sphereLongY[iCurLong] * sphereMult[intRow + 1];
                    spherePositions[currentVertex * 3 + 2] = sphereHeights[intRow + 1];


                    currentVertex++;
                }
            }

            spherePositions[currentVertex * 3] = 0.0f;
            spherePositions[currentVertex * 3 + 1] = 0.0f;
            spherePositions[currentVertex * 3 + 2] = sphereHeights[numLevels - 1];

            currentVertex++;

            for (int i = 0; i < numVertices * 3; i++)
            {
                spherePositions[i] *= radius;
            }

            mesh.PrimitiveTopology = Windows.Graphics.DirectX.DirectXPrimitiveTopology.TriangleList;

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Vertex,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(spherePositions));

            float[] normals = new float[currentVertex * 3];

            for (int iNormal = 0; iNormal < currentVertex; iNormal++)
            {
                float length = (float)Math.Sqrt(spherePositions[iNormal * 3] * spherePositions[iNormal * 3] + spherePositions[iNormal * 3 + 1] * spherePositions[iNormal * 3 + 1] + spherePositions[iNormal * 3 + 2] * spherePositions[iNormal * 3 + 2]);

                normals[iNormal * 3] = spherePositions[iNormal * 3] / length;
                normals[iNormal * 3 + 1] = spherePositions[iNormal * 3 + 1] / length;
                normals[iNormal * 3 + 2] = spherePositions[iNormal * 3 + 2] / length;
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Normal,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(normals));

            mesh.PrimitiveTopology = Windows.Graphics.DirectX.DirectXPrimitiveTopology.TriangleList;

            UInt32[] colors = new UInt32[currentVertex];

            for (int i = 0; i < currentVertex; i++)
            {
                colors[i] = 0xffffffff;
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Color,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32UInt,
               SceneHelper.CreateMemoryBufferWithArray(colors));

            UInt16[] indices = new UInt16[numTriangles * 3];

            int offsetToRowVertex = 1;

            int currentIndex = 0;

            // Add Triangles connecting to the north pole
            for (int i = 0; i < numLevels; i++)
            {
                indices[currentIndex++] = (UInt16)(offsetToRowVertex + i);
                indices[currentIndex++] = (UInt16)(((i + 1) % numLevels) + offsetToRowVertex);
                indices[currentIndex++] = (UInt16)(0);
            }

            for (int iInterior = 0; iInterior < numInteriorRows - 1; iInterior++)
            {
                offsetToRowVertex = 1 + (iInterior * numLevels);
                int offsetToNextRowVertex = 1 + (iInterior + 1) * numLevels;

                for (int i = 0; i < numLevels; i++)
                {
                    indices[currentIndex++] = (UInt16)(offsetToNextRowVertex + i);
                    indices[currentIndex++] = (UInt16)(((i + 1) % numLevels) + offsetToNextRowVertex);
                    indices[currentIndex++] = (UInt16)(offsetToRowVertex + i);

                    indices[currentIndex++] = (UInt16)(offsetToRowVertex + i);
                    indices[currentIndex++] = (UInt16)(((i + 1) % numLevels) + offsetToNextRowVertex);
                    indices[currentIndex++] = (UInt16)(((i + 1) % numLevels) + offsetToRowVertex);
                }
            }

            offsetToRowVertex = 1 + numLevels * (numInteriorRows - 1);

            // Add Triangles connecting to the south pole
            for (int i = 0; i < numLevels; i++)
            {
                indices[currentIndex++] = (UInt16)(numVertices - 1);
                indices[currentIndex++] = (UInt16)(((i + 1) % numLevels) + offsetToRowVertex);
                indices[currentIndex++] = (UInt16)(offsetToRowVertex + i);
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Index,
               Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt,
               SceneHelper.CreateMemoryBufferWithArray(indices));
        }

        public static void FillMeshWithCube(SceneMesh mesh, float size = 200)
        {
            float[] positionsCube = new float[k_rgPositionsCube.Length];
            for (int i = 0; i < k_rgPositionsCube.Length; i++)
            {
                positionsCube[i] = k_rgPositionsCube[i] * (size / 2.0f);
            }

            mesh.PrimitiveTopology = Windows.Graphics.DirectX.DirectXPrimitiveTopology.TriangleList;

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Vertex,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(positionsCube));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Index,
               Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt,
               SceneHelper.CreateMemoryBufferWithArray(k_rgIndicesCube));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Normal,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgNormalsCube));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.TexCoord0,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgUV0sCube));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.TexCoord1,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgUV1sCube));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Color,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32UInt,
               SceneHelper.CreateMemoryBufferWithArray(k_rgColorsCube));
        }

        private static float[] k_rgPositionsCube =
       {
            /* front  */ -1.0f, 1.0f, 1.0f,  -1.0f, -1.0f, 1.0f,  1.0f, -1.0f, 1.0f,   1.0f, 1.0f, 1.0f,
            /* back   */ 1.0f, 1.0f, -1.0f,  1.0f, -1.0f, -1.0f,  -1.0f, -1.0f, -1.0f, -1.0f, 1.0f, -1.0f,
            /* left   */ 1.0f, 1.0f, 1.0f,   1.0f, -1.0f, 1.0f,   1.0f, -1.0f, -1.0f,  1.0f, 1.0f, -1.0f,
            /* right  */ -1.0f, 1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, 1.0f,  -1.0f, 1.0f, 1.0f,
            /* top    */ -1.0f, 1.0f, -1.0f, -1.0f, 1.0f, 1.0f,   1.0f, 1.0f, 1.0f,    1.0f, 1.0f, -1.0f,
            /* bottom */ -1.0f, -1.0f, 1.0f, -1.0f, -1.0f, -1.0f, 1.0f, -1.0f, -1.0f,  1.0f, -1.0f, 1.0f,
        };

        private static UInt16[] k_rgIndicesCube =
       {
            /* front  */ 0, 1, 2,       2, 3, 0,
            /* back   */ 4, 5, 6,       6, 7, 4,
            /* left   */ 8, 9, 10,      10, 11, 8,
            /* right  */ 12, 13, 14,    14, 15, 12,
            /* top    */ 16, 17, 18,    18, 19, 16,
            /* bottom */ 20, 21, 22,    22, 23, 20,
        };

        private static float[] k_rgNormalsCube =
       {
            /* front  */ 0.0f, 0.0f, 1.0f,    0.0f, 0.0f, 1.0f,     0.0f, 0.0f, 1.0f,     0.0f, 0.0f, 1.0f,
            /* back   */ 0.0f, 0.0f, -1.0f,   0.0f, 0.0f, -1.0f,    0.0f, 0.0f, -1.0f,    0.0f, 0.0f, -1.0f,
            /* left   */ 1.0f, 0.0f, 0.0f,    1.0f, 0.0f, 0.0f,     1.0f, 0.0f, 0.0f,     1.0f, 0.0f, 0.0f,
            /* right  */ -1.0f, 0.0f, 0.0f,   -1.0f, 0.0f, 0.0f,    -1.0f, 0.0f, 0.0f,    -1.0f, 0.0f, 0.0f,
            /* top    */ 0.0f, 1.0f, 0.0f,    0.0f, 1.0f, 0.0f,     0.0f, 1.0f, 0.0f,     0.0f, 1.0f, 0.0f,
            /* bottom */ 0.0f, -1.0f, 0.0f,   0.0f, -1.0f, 0.0f,    0.0f, -1.0f, 0.0f,    0.0f, -1.0f, 0.0f,
        };

        private static float[] k_rgUV0sCube =
       {
            /* front  */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            /* back   */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            /* left   */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            /* right  */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            /* top    */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            /* bottom */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
        };

        private static float[] k_rgUV1sCube =
       {
            /* front  */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f,
            /* back   */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f,
            /* left   */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f,
            /* right  */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f,
            /* top    */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f,
            /* bottom */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, 2.0f, 2.0f, -1.0f
        };

        private static UInt32[] k_rgColorsCube =
       {
            /* front  */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
            /* back   */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
            /* left   */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
            /* right  */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
            /* top    */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
            /* bottom */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF,
        };

        public static void FillMeshWithSquare(SceneMesh mesh, float width, float height, float anchorX, float anchorY)
        {
            float[] positionsSquare = new float[k_rgPositionsSquare.Length];

            /*
           for (int i = 0; i < k_rgPositionsSquare.Length; i++)
           {
               positionsSquare[i + 0] = k_rgPositionsSquare[i + 0];
           }*/

            for (int i = 0; i < k_rgPositionsSquare.Length; i += 3)
            {
                positionsSquare[i + 0] = k_rgPositionsSquare[i + 0] * width - anchorX;
                positionsSquare[i + 1] = k_rgPositionsSquare[i + 1] * height - anchorY;
                positionsSquare[i + 2] = k_rgPositionsSquare[i + 2];
            }

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Vertex,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(positionsSquare));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Index,
               Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt,
               SceneHelper.CreateMemoryBufferWithArray(k_rgIndicesSquare));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Normal,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32B32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgNormalsSquare));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.TexCoord0,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgUV0sSquare));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.TexCoord1,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32G32Float,
               SceneHelper.CreateMemoryBufferWithArray(k_rgUV1sSquare));

            mesh.FillMeshAttribute(
               SceneAttributeSemantic.Color,
               Windows.Graphics.DirectX.DirectXPixelFormat.R32UInt,
               SceneHelper.CreateMemoryBufferWithArray(k_rgColorsSquare));
        }

        private static float[] k_rgPositionsSquare =
       {
            0.0f, 1.0f, 1.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f, 1.0f,   1.0f, 1.0f, 1.0f
        };

        private static UInt16[] k_rgIndicesSquare =
       {
            /* front  */ 0, 1, 2,       2, 3, 0
        };

        private static float[] k_rgNormalsSquare =
       {
            /* front  */ 0.0f, 0.0f, 1.0f,    0.0f, 0.0f, 1.0f,     0.0f, 0.0f, 1.0f,     0.0f, 0.0f, 1.0f
        };

        private static float[] k_rgUV0sSquare =
       {
            /* front  */ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f
        };

        private static float[] k_rgUV1sSquare =
       {
            /* front  */ -1.0f, -1.0f, -1.0f, 2.0f, 2.0f, -1.0f, 2.0f, 2.0f
        };

        private static UInt32[] k_rgColorsSquare =
       {
            /* front  */ 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF
        };
    }
}
