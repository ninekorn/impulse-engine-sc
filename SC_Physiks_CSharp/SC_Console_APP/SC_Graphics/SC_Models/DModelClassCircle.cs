using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SCCoreSystems
{
    public class DModelClassCircle
    {
        private SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        private SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        private int VertexCount { get; set; }
        public int IndexCount { get; set; }

        public DColorShader.DVertex[] vertices { get; set; }

        public Vector2 position { get; set; }

        public DModelClassCircle() { }

        public float OriginRotationY { get; set; }
        public float OriginRotationX { get; set; }
        public float OriginRotationZ { get; set; }


        public float CurrentRotationY { get; set; }
        public float CurrentRotationX { get; set; }
        public float CurrentRotationZ { get; set; }


        public SharpDX.Matrix originRot { get; set; }

        public bool Initialize(Device device, float radius, int vertNumber, Vector4 color, float rotX, float rotY, float rotZ)
        {
            return InitializeBuffer(device, radius, vertNumber, color, rotX, rotY, rotZ);
        }

        public void ShutDown()
        {
            ShutDownBuffers();
        }

        public void Render(DeviceContext deviceContext)
        {
            RenderBuffers(deviceContext);
        }

        private bool InitializeBuffer(Device device, float radius, int vertNumber,Vector4 color, float rotX, float rotY, float rotZ)
        {
            try
            {

                OriginRotationX = rotX;
                OriginRotationY = rotY;
                OriginRotationZ = rotZ;

                CurrentRotationX = 0;// rotX;
                CurrentRotationY = 0;// rotY;
                CurrentRotationZ = 0;// rotZ;

                float pitch = OriginRotationX * 0.0174532925f;
                float yaw = OriginRotationY * 0.0174532925f;
                float roll = OriginRotationZ * 0.0174532925f;
                originRot = SharpDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);


                /*Vector3 rotOnX = new Vector3(1,0,0);
                Quaternion rotterOnX = SharpDX.Quaternion.RotationAxis(rotOnX, OriginRotationX);

                Vector3 rotOnY = new Vector3(0, 1, 0);
                Quaternion rotterOnY= SharpDX.Quaternion.RotationAxis(rotOnY, OriginRotationX);

                Vector3 rotOnZ = new Vector3(0, 0, 1);
                Quaternion rotterOnZ = SharpDX.Quaternion.RotationAxis(rotOnZ, OriginRotationX);

                var quatterFinal = rotterOnX + rotterOnY + rotterOnZ;

                originRot = Matrix.RotationQuaternion(quatterFinal);*/



                /*VertexCount = 4;
                IndexCount = 6;

                vertices = new[]
                {
                    new DColorShader.DVertex()
                    {
                        position = new Vector3(-dims.X, -dims.Y, 0),
                        color = color//new Vector4(0, 1, 0, 1)
                    },

                    new DColorShader.DVertex()
                    {
                        position = new Vector3(dims.X, -dims.Y, 0),
                        color = color// new Vector4(0, 1, 0, 1)
                    },

					new DColorShader.DVertex()
                    {
                        position = new Vector3(dims.X, dims.Y, 0),
                        color = color//new Vector4(0, 1, 0, 1)
                    },

                    new DColorShader.DVertex()
                    {
                        position = new Vector3(-dims.X, dims.Y, 0),
                        color = color//new Vector4(0, 1, 0, 1)
                    }

                };

                int[] indices = new int[]
                {
                    0,
					3,
					1,
                    3,
                    2,
                    1
                };*/

                List<Vector3> vertexList = new List<Vector3> { };
                float x;
                float y;

                vertexList.Add(new Vector3(0, 0, 0f));
                for (int i = 0; i < vertNumber; i++)
                {
                    x = (float)(radius * Math.Sin((2 * Math.PI * i) / vertNumber));
                    y = (float)(radius * Math.Cos((2 * Math.PI * i) / vertNumber));
                    vertexList.Add(new Vector3(x, y, 0f));
                }

                vertices = new DColorShader.DVertex[vertexList.Count];

                for (int i = 0; i < vertexList.Count; i++)
                {
                    vertices[i] = new DColorShader.DVertex()
                    {
                        position = new Vector3(vertexList[i].X, vertexList[i].Y, 0),
                        color = color
                    };
                }

                List<int> trianglesList = new List<int> { };
                for (int i = 0; i < (vertNumber - 1); i ++)
                {
                    trianglesList.Add(0);
                    trianglesList.Add((i) + 1);
                    trianglesList.Add((i) + 2);
                }

                trianglesList.Add(0);
                trianglesList.Add(vertices.Length - 1);
                trianglesList.Add(1);


                int[] indices = trianglesList.ToArray();




                VertexCount = vertices.Length;
                IndexCount = trianglesList.Count;



                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);

                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);

                //vertices = null;
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
            IndexBuffer?.Dispose();
            IndexBuffer = null;

            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }

        private void RenderBuffers(DeviceContext deviceContext)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<DColorShader.DVertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}