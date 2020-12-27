using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

//using SC_WPF_RENDER.SC_Graphics.SC_Textures.SC_VR_Touch_Textures;

using System.Linq;
using System;


using System.Runtime.InteropServices;

namespace SC_Console_APP
{
    public class SC_VR_Cube
    {
        public SharpDX.Direct3D11.Buffer InstanceBuffer { get; set; }


        private SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        private SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        private int VertexCount { get; set; }
        public int IndexCount { get; set; }

        public SC_VR_Touch_Shader.DVertex[] Vertices { get; set; }
        public int[] indices;


        public SharpDX.Vector3 Position { get; set; }
        public SharpDX.Quaternion Rotation { get; set; }
        public SharpDX.Vector3 Forward { get; set; }

        private float _sizeX = 0;
        private float _sizeY = 0;
        private float _sizeZ = 0;


        public SharpDX.Matrix _MatrixPos { get; set; }

        // Constructor
        public SC_VR_Cube() { }

        public Vector4 _color;

        [StructLayout(LayoutKind.Sequential)]
        public struct DInstanceType
        {
            public Vector3 position;
        };
        float InstanceCount = 0;




        private int _width = 0;
        private int _height = 0;
        private int _depth = 0;



        // Methods.
        public bool Initialize(SharpDX.Direct3D11.Device device,float x, float y, float z, Vector4  color,int width,int height, int depth)
        {
            this._color = color;
            this._sizeX = x;
            this._sizeY = y;
            this._sizeZ = z;

            this._width = width;
            this._height = height;
            this._depth = depth;


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


      
        private bool InitializeBuffer(SharpDX.Direct3D11.Device device)
        {
            try
            {
                 //Set number of vertices in the vertex array.
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
                indices = new int[]
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

        


                int widther = _width;
                int heigther = _height;
                int depther = _depth;

                InstanceCount = widther * heigther * depther;
                DInstanceType[] instances = new DInstanceType[widther * heigther * depther];

                int counter = 0;
                for (int x = 0;x < widther; x++)
                {
                    for (int y = 0; y < heigther; y++)
                    {
                        for (int z = 0; z < depther; z++)
                        {
                            instances[counter] = new DInstanceType()
                            {
                                position = new Vector3(x, y, z),
                            };
                            counter++;
                        }
                    }
                }

                // Create the vertex buffer.
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, Vertices);

                // Create the index buffer.
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);

                InstanceBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, instances);
                //InstanceBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, instances, Utilities.SizeOf<DInstanceType>() * instances.Length, ResourceUsage.Dynamic, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

                // Delete arrays now that they are in their respective vertex and index buffers.
                //Vertices = null;
                //indices = null;

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

            InstanceBuffer?.Dispose();
            InstanceBuffer = null;
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<SC_VR_Touch_Shader.DVertex>(), 0), new VertexBufferBinding(InstanceBuffer, Utilities.SizeOf<DInstanceType>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}








































//VertexCount = 36;
// Set number of vertices in the index array.
//IndexCount = 36;



/*Vertices = new[]
{
     new SC_VR_Touch_Shader.DVertex()
     {
         position = new Vector3( 1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =  new Vector3(1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3(-1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position = new Vector3( 1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =  new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },




     new SC_VR_Touch_Shader.DVertex()
     {
         position =new Vector3(-1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =          new Vector3( 1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =  new Vector3(-1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =  new Vector3(-1, -1,  1)*_sizeX,
         color = _color,
     },


     new SC_VR_Touch_Shader.DVertex()
     {
         position =new Vector3(-1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =         new Vector3(-1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =      new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =    new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =            new Vector3(-1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3(-1,  1,  1)*_sizeX,
         color = _color,
     },


     new SC_VR_Touch_Shader.DVertex()
     {
         position =new Vector3( 1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =    new Vector3( 1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =        new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =      new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3( 1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =  new Vector3( 1, -1, -1)*_sizeX,
         color = _color,
     },




     //BOTTOM
     new SC_VR_Touch_Shader.DVertex()
     {
         position = new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =    new Vector3( 1, -1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =     new Vector3( 1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3( 1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =             new Vector3(-1, -1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =       new Vector3(-1, -1, -1)*_sizeX,
         color = _color,
     },


     //TOP
      new SC_VR_Touch_Shader.DVertex()
     {
         position =     new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =    new Vector3( 1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =       new Vector3(-1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =     new Vector3(-1,  1, -1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =        new Vector3(-1,  1,  1)*_sizeX,
         color = _color,
     },
     new SC_VR_Touch_Shader.DVertex()
     {
         position =   new Vector3( 1,  1,  1)*_sizeX,
         color = _color,
     },


};*/
