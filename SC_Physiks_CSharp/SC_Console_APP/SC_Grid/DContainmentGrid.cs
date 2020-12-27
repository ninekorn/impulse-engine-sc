using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;

namespace SCCoreSystems
{
    public class DContainmentGrid                   // 179 lines
    {
        // Structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct DVertexType
        {
            internal Vector3 position;
            internal Vector4 color;
        }

        // Variables
        private int m_TerrainWidth, m_TerrainHeight, m_TerrainDepth;

        // Properties
        private SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        private SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        private int VertexCount { get; set; }
        public int IndexCount { get; private set; }

        // Constructor
        public DContainmentGrid() { }


        float _tileSize = 0;

        float _offsetX = 0;
        float _offsetY = 0;
        float _offsetZ = 0;

        bool _faceBottom = false;
        bool _faceTop = false;

        bool _faceLeft = false;
        bool _faceRight = false;

        bool _faceFront = false;
        bool _faceBack = false;

        // Methods.
        public bool Initialize(SharpDX.Direct3D11.Device device, int width, int height,int depth, float tileSize, float offsetX, float offsetY, float offsetZ)
        {


            _offsetX = offsetX;
            _offsetY = offsetY;
            _offsetZ = offsetZ;

            _tileSize = tileSize;
            // Manually set the width and height of the terrain.
            m_TerrainWidth = width; 
            m_TerrainHeight = height;
            m_TerrainDepth = depth;
            // Initialize the vertex and index buffer that hold the geometry for the terrain.
            if (!InitializeBuffers(device))
                return false;

            return true;
        }
        private bool InitializeBuffers(SharpDX.Direct3D11.Device device)
        {
            try
            {
                // Calculate the number of vertices in the terrain mesh.
                VertexCount = (m_TerrainWidth) * (m_TerrainHeight)* (m_TerrainDepth) * (m_TerrainWidth) * (m_TerrainHeight) * (m_TerrainDepth ) * 10 + (2* m_TerrainWidth* m_TerrainDepth);
                // Set the index count to the same as the vertex count.
                IndexCount = VertexCount;

                // Create the vertex array.
                DVertexType[] vertices = new DVertexType[VertexCount];         
                // Create the index array.
                int[] indices = new int[IndexCount];

                // Initialize the index to the vertex array.
                int index = 0;

                // Load the vertex and index arrays with the terrain data.
                for (int x = -(m_TerrainWidth); x < (m_TerrainWidth)+1; x++)
                {
                    for (int y = -(m_TerrainHeight); y < (m_TerrainHeight)+1; y++)
                    {
                        for (int z = -(m_TerrainDepth); z < (m_TerrainDepth)+1; z++)
                        {
                            if (x < m_TerrainWidth && z < m_TerrainDepth)
                            {
                                // LINE 1
                                // Upper left.
                                float positionX = (float)z;
                                float positionY = (float)(y);
                                float positionZ = (float)(x + 1);

                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;
                                // Upper right.
                                positionX = (float)(z + 1);
                                positionZ = (float)(x + 1);
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;

                                // LINE 2
                                // Upper right.
                                positionX = (float)(z + 1);
                                positionZ = (float)(x + 1);
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;
                                // Bottom right.
                                positionX = (float)(z + 1);
                                positionZ = (float)x;
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;

                                // LINE 3
                                // Bottom right.
                                positionX = (float)(z + 1);
                                positionZ = (float)x;
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;
                                // Bottom left.
                                positionX = (float)z;
                                positionZ = (float)x;
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;

                                // LINE 4
                                // Bottom left.
                                positionX = (float)z;
                                positionZ = (float)x;
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;
                                // Upper left.
                                positionX = (float)z;
                                positionZ = (float)(x + 1);

                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                                indices[index] = index;
                                index++;

                            }

                            if (y < m_TerrainHeight)
                            {
                                float positionX = (float)(z);
                                float positionZ = (float)(x);
                                float positionY = (float)(y);
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1, 1.0f, 1, 1.0f);
                                indices[index] = index;
                                index++;


                                positionX = (float)(z);
                                positionZ = (float)(x);
                                positionY = (float)(y + 1);
                                vertices[index].position = new Vector3(positionX, positionY, positionZ) * _tileSize;
                                vertices[index].color = new Vector4(1, 1.0f, 1, 1.0f);
                                indices[index] = index;
                                index++;
                            }                          
                        }
                    }
                }

                // Create the vertex buffer.
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);

                // Create the index buffer.
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);

                // Release the arrays now that the buffers have been created and loaded.
                vertices = null;
                indices = null;

                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public void ShutDown()
        {
            // Release the vertex and index buffers.
            ShutdownBuffers();
        }
        private void ShutdownBuffers()
        {
            // Return the index buffer.
            IndexBuffer?.Dispose();
            IndexBuffer = null;
            // Release the vertex buffer.
            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }
        public void Render(DeviceContext deviceContext)
        {
            // Put the vertex and index buffers on the graphics pipeline to prepare them for drawing.
            RenderBuffers(deviceContext);
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            // Set the vertex buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<DVertexType>(), 0));
            // Set the index buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            // Set the type of the primitive that should be rendered from this vertex buffer, in this case triangles.
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
        }
    }
}