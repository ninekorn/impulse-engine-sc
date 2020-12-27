using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using SC_WPF_RENDER.SC_Graphics.SC_Textures.SC_VR_Touch_Textures;

namespace SC_WPF_RENDER.SC_Graphics.SC_Models
{
    public class SC_Terrain
    {
        // Properties
        private SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        private SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        private int VertexCount { get; set; }
        public int IndexCount { get; set; }

        private float _touchSize = 1f;

        public SharpDX.Vector3 Position { get; set; }
        public SharpDX.Quaternion Rotation { get; set; }
        public SharpDX.Vector3 Forward { get; set; }

        public SC_VR_Touch_Shader.DVertex[] Vertices { get; set; }

        public Vector4 _color;
        // Constructor
        public SC_Terrain() { }

        // Methods.
        public bool Initialize(SharpDX.Direct3D11.Device device,float x, float y, float z, Vector4 color)
        {
            this._color = color;
            this._sizeX = x;
            this._sizeY = y;
            this._sizeZ = z;

            // Initialize the vertex and index buffer that hold the geometry for the triangle.
            return InitializeBuffer(device);
        }
        public void ShutDown()
        {
            // Release the vertex and index buffers.
            ShutDownBuffers();
        }
        public void Render(DeviceContext deviceContext)
        {
            // Put the vertex and index buffers on the graphics pipeline to prepare for drawings.
            RenderBuffers(deviceContext);
        }

        private float _sizeX = 0;
        private float _sizeY = 0;
        private float _sizeZ = 0;


        private bool InitializeBuffer(SharpDX.Direct3D11.Device device)
        {
            try
            {

                // Set number of vertices in the vertex array.
                VertexCount = 8;
                // Set number of vertices in the index array.
                IndexCount = 36;

                // Create the vertex array and load it with data.
                Vertices = new[]
                     {
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(-1*_sizeX, -1*_sizeY, 1*_sizeZ),
                         color = _color,

                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(-1*_sizeX, 1*_sizeY, 1*_sizeZ),
                         color = _color,
                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(1*_sizeX, -1*_sizeY, 1*_sizeZ),
                         color = _color,
                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(1*_sizeX, 1*_sizeY, 1*_sizeZ),
                         color = _color,
                     },


                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(-1*_sizeX, -1*_sizeY, -1*_sizeZ),
                         color = _color,
                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(-1*_sizeX, 1*_sizeY, -1*_sizeZ),
                         color = _color,
                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(1*_sizeX, -1*_sizeY, -1*_sizeZ),
                         color = _color,
                     },
                     new SC_VR_Touch_Shader.DVertex()
                     {
                         position = new Vector3(1*_sizeX, 1*_sizeY, -1*_sizeZ),
                         color = _color,
                     },
                 };


                // Create Indicies to load into the IndexBuffer.
                int[] indices = new int[]
                {
                    0, // Bottom left.
					1, // Top middle.
					2,  // Bottom right.
                    3,
                    2,
                    1,

                    1,
                    5,
                    3,
                    7,
                    3,
                    5,

                    2,
                    3,
                    6,
                    7,
                    6,
                    3,

                    6,
                    7,
                    4,
                    5,
                    4,
                    7,

                    4,
                    5,
                    0,
                    1,
                    0,
                    5,

                    4,
                    0,
                    6,
                    2,
                    6,
                    0
                };

                // Create the vertex buffer.
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, Vertices);

                // Create the index buffer.
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);

                // Delete arrays now that they are in their respective vertex and index buffers.
                Vertices = null;
                indices = null;

                // Set the vertex buffer to active in the input assembler so it can be rendered.
                //device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<SC_VR_Touch_Shader.DVertex>(), 0));

                // Set the index buffer to active in the input assembler so it can be rendered.
                //device.ImmediateContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);

                // Set the type of the primitive that should be rendered from this vertex buffer, in this case triangles.
                //device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;




                return true;
            }
            catch
            {
                return false;
            }
        }
        private void ShutDownBuffers()
        {
            // Release the index buffer.
            IndexBuffer?.Dispose();
            IndexBuffer = null;
            // Release the vertex buffer.
            VertexBuffer?.Dispose();
            VertexBuffer = null;



        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            // Set the vertex buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<SC_VR_Terrain_Shader.DVertex>(), 0));

            // Set the index buffer to active in the input assembler so it can be rendered.
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);

            // Set the type of the primitive that should be rendered from this vertex buffer, in this case triangles.
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}