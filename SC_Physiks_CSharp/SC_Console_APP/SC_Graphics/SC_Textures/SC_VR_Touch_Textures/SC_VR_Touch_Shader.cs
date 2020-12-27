using SharpDX.Direct3D11;
using SharpDX.WIC;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;

using SharpDX;
using SharpDX.D3DCompiler;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//using SharpHelper;
using SharpDX.DXGI;
using SharpDX.Direct3D;

namespace SC_Console_APP
{
    public class SC_VR_Touch_Shader                   // 199 lines
    {
        public SC_VR_Touch_Shader.DVertex[] Vertices { get; set; }
        public int[] indices;


        public SharpDX.Vector3 Position { get; set; }
        public SharpDX.Quaternion Rotation { get; set; }
        public SharpDX.Vector3 Forward { get; set; }

        private float _sizeX = 0;
        private float _sizeY = 0;
        private float _sizeZ = 0;


        public SharpDX.Matrix _MatrixPos { get; set; }

        public Vector4 _color;

        [StructLayout(LayoutKind.Sequential)]
        public struct DInstanceType
        {
            public Vector3 position;
            //public Matrix worldMatrix;
        };
        float InstanceCount = 0;

        DInstanceType[] instances;








     // Structures.
     [StructLayout(LayoutKind.Sequential)]
        public struct DVertex
        {
            public static int AppendAlignedElement = 12;
            public Vector3 position;
            public Vector4 color;
            public Vector2 texture;
            //public Vector3 normal;
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct DMatrixBuffer
        {
            //public Matrix world;
            //public Matrix view;
            //public Matrix projection;
            public Matrix worldViewProjection;
        }









        // Properties.
        private VertexShader VertexShader  { get; set; }
        private PixelShader PixelShader  { get; set; }
        private GeometryShader GeometryShader  { get; set; }
        private SamplerState SamplerState { get; set; }

        private InputLayout Layout { get; set; }
        private SharpDX.Direct3D11.Buffer ConstantMatrixBuffer { get; set; }

        // Constructor
        public SC_VR_Touch_Shader() { }

        // Methods.
        public bool Initialize(SharpDX.Direct3D11.Device device, IntPtr windowsHandle) //, float x, float y, float z, Vector4 color,Matrix worldMatrix
        {
            //this._color = color;
            //this._sizeX = x;
            //this._sizeY = y;
            //this._sizeZ = z;

            // Initialize the vertex and pixel shaders.
            return InitializeShader(device, windowsHandle); // @"Tut04\Shaders\Color.ps" //, "Color.vs", "Color.ps"
        }
        private bool InitializeShader(SharpDX.Direct3D11.Device device, IntPtr windowsHandle) //, string vsFileName, string psFileName //, Matrix worldMatrix
        {
            try
            {
                var vsFileNameByteArray = SC_Console_APP.Properties.Resources.textureTrig1;
                var psFileNameByteArray = SC_Console_APP.Properties.Resources.textureTrig;
                //var vsFileNameByteArray = SC_WPF_RENDER.Properties.Resources.red1;
                //var psFileNameByteArray = SC_WPF_RENDER.Properties.Resources.red;
                //var gsFileNameByteArray = SC_WPF_RENDER.Properties.Resources.HLSL;


                ShaderBytecode vertexShaderByteCode = ShaderBytecode.Compile(vsFileNameByteArray, "ColorVertexShader", "vs_5_0", ShaderFlags.None, EffectFlags.None);
                ShaderBytecode pixelShaderByteCode = ShaderBytecode.Compile(psFileNameByteArray, "ColorPixelShader", "ps_5_0", ShaderFlags.None, EffectFlags.None);
                //ShaderBytecode geometryShaderByteCode = ShaderBytecode.Compile(gsFileNameByteArray, "GS", "gs_5_0", ShaderFlags.None, EffectFlags.None);

                //ShaderBytecode vertexShaderByteCode = ShaderBytecode.Compile(gsFileNameByteArray, "VS", "vs_5_0", ShaderFlags.None, EffectFlags.None);
                //ShaderBytecode pixelShaderByteCode = ShaderBytecode.Compile(gsFileNameByteArray, "PS", "ps_5_0", ShaderFlags.None, EffectFlags.None);


                // Create the vertex shader from the buffer.
                VertexShader = new VertexShader(device, vertexShaderByteCode);
                // Create the pixel shader from the buffer.
                PixelShader = new PixelShader(device, pixelShaderByteCode);

                //GeometryShader = new GeometryShader(device, geometryShaderByteCode);
                // Now setup the layout of the data that goes into the shader.
                // This setup needs to match the VertexType structure in the Model and in the shader.



                InputElement[] inputElements = new InputElement[]
                {
                    //new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 0)

                    new InputElement()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElement()
                    {
                        SemanticName = "COLOR",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElement.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },

                     new InputElement()
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 1,
                        Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
                        Slot = 1,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerInstanceData,
                        InstanceDataStepRate = 1
                    },

                };

                // Create the vertex input the layout. Kin dof like a Vertex Declaration.
                Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

                // Release the vertex and pixel shader buffers, since they are no longer needed.
                vertexShaderByteCode.Dispose();
                pixelShaderByteCode.Dispose();

                // Setup the description of the dynamic matrix constant Matrix buffer that is in the vertex shader.
                BufferDescription matrixBufferDescription = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<DMatrixBuffer>(),// * Utilities.SizeOf<DInstanceType>() * instances.Length, //Utilities.SizeOf<DMatrixBuffer>() *
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };


                /*BufferDescription matrixBufferDescription = new BufferDescription()
                {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<DMatrixBuffer>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };*/


                // Create the constant buffer pointer so we can access the vertex shader constant buffer from within this class.
                ConstantMatrixBuffer = new SharpDX.Direct3D11.Buffer(device, matrixBufferDescription);


                SamplerStateDescription samplerDesc = new SamplerStateDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),  // Black Border.
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };

                // Create the texture sampler state.
                SamplerState = new SamplerState(device, samplerDesc);

                //ConstantMatrixBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, instances);


                //int bufferSlotNumberer = 0;
                //device.ImmediateContext.VertexShader.SetConstantBuffer(bufferSlotNumberer, ConstantMatrixBuffer);


                //ConstantMatrixBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.ConstantBuffer, instances, Utilities.SizeOf<DMatrixBuffer>()* Utilities.SizeOf<DInstanceType>() * instances.Length, ResourceUsage.Dynamic, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);



                // Create a texture sampler state description.
                /*SamplerStateDescription samplerDesc = new SamplerStateDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),  // Black Border.
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };

                // Create the texture sampler state.
                SamplerState = new SamplerState(device, samplerDesc);
                */

                //int bufferSlotNumber = 0;
                //device.ImmediateContext.VertexShader.SetConstantBuffer(bufferSlotNumber, ConstantMatrixBuffer);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing shader. Error is " + ex.Message);
                return false;
            }
        }
        inData data;
        SharpDX.Direct3D11.Buffer bufferWorld;

        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct inData
        {
            //public SharpDX.Matrix worldMatrix;
            //public SharpDX.Matrix viewMatrix;
            //public SharpDX.Matrix projectionMatrix;
            //public Matrix[] worldViewProjection;
            public Vector3 instancePosition;
        }
        public void ShutDown()
        {
            // Shutdown the vertex and pixel shaders as well as the related objects.
            ShuddownShader();
        }
        private void ShuddownShader()
        {
            SamplerState?.Dispose();
            SamplerState = null;
            // Release the matrix constant buffer.
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            // Release the layout.
            Layout?.Dispose();
            Layout = null;
            // Release the pixel shader.
            PixelShader?.Dispose();
            PixelShader = null;
            // Release the vertex shader.
            VertexShader?.Dispose();
            VertexShader = null;

            //GeometryShader?.Dispose();
            //GeometryShader = null;
            
        }

        public bool Render(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, int vertexCount, int instanceCount, Matrix[] _worldMatrix)
        {
            if (!SetShaderParameters(deviceContext, worldMatrix, viewMatrix, projectionMatrix,  _worldMatrix))
                return false;

            RenderShader(deviceContext, indexCount, vertexCount, instanceCount);

            return true;
        }

        int bufferSlotNumber = 0;
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Matrix[] _worldMatrix)
        {
            try
            {
                Matrix wvp = worldMatrix * viewMatrix * projectionMatrix;
                wvp.Transpose();

                DataStream streamer;
                deviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out streamer);
                //streamer.WriteRange(_worldMatrix);
                DMatrixBuffer matrixBuffer = new DMatrixBuffer()
                {
                    worldViewProjection = wvp
                };
                streamer.Write(matrixBuffer);
                //streamer.Write(_worldMatrix);
                deviceContext.UnmapSubresource(ConstantMatrixBuffer, 0); //to update the data on GPU
                int bufferSlotNumberer = 0;
                deviceContext.VertexShader.SetConstantBuffer(bufferSlotNumberer, ConstantMatrixBuffer);
                streamer.Dispose();
                
                /*Vector3[] total = new Vector3[_worldMatrix.Length];
                for (int i = 0; i < _worldMatrix.Length; i++)
                {
                    total[i].X = _worldMatrix[i].M41;
                    total[i].Y = _worldMatrix[i].M42;
                    total[i].Z = _worldMatrix[i].M43;

                    // _worldMatrix[i].M41 += instances[i].position.X;
                    //_worldMatrix[i].M42 += instances[i].position.Y;
                    //_worldMatrix[i].M43 += instances[i].position.Z;

                    //total[i] = _worldMatrix[i] * viewMatrix * projectionMatrix;
                    //total[i].Transpose();
                }

                /*DataStream streamer;
                deviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out streamer);
                streamer.WriteRange(total);

                deviceContext.UnmapSubresource(ConstantMatrixBuffer, 0); //to update the data on GPU
                int bufferSlotNumber = 0;
                deviceContext.VertexShader.SetConstantBuffer(bufferSlotNumber, ConstantMatrixBuffer);
                streamer.Dispose();
                */





                /*//inData MatrixBuffer = new inData();
                //MatrixBuffer.instancePosition = new Vector3(-5, 5, -5);
                DataStream stream;
                deviceContext.MapSubresource(InstanceBuffer, MapMode.WriteNoOverwrite, SharpDX.Direct3D11.MapFlags.None, out stream);
                stream.WriteRange(total);
                //stream.Write(MatrixBuffer);

                deviceContext.UnmapSubresource(InstanceBuffer, 0); //to update the data on GPU
                bufferSlotNumber = 1;
                deviceContext.VertexShader.SetConstantBuffer(bufferSlotNumber, InstanceBuffer);
                stream.Dispose();*/
                

                //inData MatrixBuffer = new inData();
               // MatrixBuffer.instancePosition = new Vector3(-5, 5, -5);
                //deviceContext.UpdateSubresource(ref MatrixBuffer, InstanceBuffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

    


        private void RenderShader(DeviceContext deviceContext, int indexCount, int vertexCount, int instanceCount)
        {

            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            //deviceContext.PixelShader.SetSampler(0, SamplerState);
            //deviceContext.GeometryShader.Set(GeometryShader);

            deviceContext.PixelShader.SetSampler(0, SamplerState);

            //deviceContext.Draw(36,0);

            //deviceContext.DrawInstanced(36, instanceCount,0,0);
            deviceContext.DrawInstanced(vertexCount, instanceCount, 0, 0);

            //deviceContext.DrawIndexedInstanced(indexCount, instanceCount, 0, 0, 0);
        }

    }
}