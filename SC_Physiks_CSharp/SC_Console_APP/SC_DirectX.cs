using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

using SharpDX.WIC;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using System.Windows.Media;
using System.Collections;

using System.Threading;
using System.Windows.Media.Imaging;

using Matrix = SharpDX.Matrix;

using SharpDX.DirectInput;

using Segment = SCCoreSystems.SC_Segment.Segment;

namespace SCCoreSystems
{
    public abstract class SC_DirectX
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DMatrixBuffer
        {
            public Matrix world;
            public Matrix view;
            public Matrix proj;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DLightBuffer
        {
            public Vector4 ambientColor;
            public Vector4 diffuseColor;
            public Vector3 lightDirection;
            public float padding; // Added extra padding so structure is a multiple of 16.
        }

        //public SC_VR_Chunk.DInstanceType[] instances { get; set; }
        //[StructLayout(LayoutKind.Sequential)]
        /*public struct DInstanceType
        {
            public Vector3 position;
        };*/

        //public static DShaderManager shaderManager { get; set; }

        //public SC_VR_Chunk[] arrayOfChunks;

        public static Device device;

        public static DeviceContext context;

        public static SharpDX.DirectInput.Keyboard _Keyboard;// { get; set; }

        public VertexShader VertexShader;
        public PixelShader PixelShader;

        public InputLayout Layout;
        public static System.Windows.Forms.Control MainControl;

        public static Matrix view;
        public static Matrix proj;

        public SC_CameraClass Camera;

        //public SC_VR_Chunk_Shader shaderOfChunk;
        /*chunkDat.chunkShader = shaderOfChunk;
        chunkDat.matrixBuffer = arrayOfMatrixBuff;
        chunkDat.vertBuffers = vertBuffers;
        chunkDat.colorBuffers = colorBuffers;
        chunkDat.indexBuffers = indexBuffers;
        chunkDat.instanceBuffers = instanceBuffers;
        chunkDat.dVertBuffers = dVertBuffers;
        chunkDat.texBuffers = texBuffers;
        chunkDat.normalBuffers = normalBuffers;
        chunkDat.lightBuffer = lightBuffer;*/

        public SharpDX.Direct3D11.Buffer[] vertBuffers;
        public SharpDX.Direct3D11.Buffer[] colorBuffers;
        public SharpDX.Direct3D11.Buffer[] indexBuffers;
        public SharpDX.Direct3D11.Buffer[] instanceBuffers;
        public SharpDX.Direct3D11.Buffer[] normalBuffers;
        public SharpDX.Direct3D11.Buffer[] texBuffers;
        public SharpDX.Direct3D11.Buffer[] dVertBuffers;

        public DLightBuffer[] lightBuffer;// = new DLightBuffer[1];
        public DModel Model;
        public DColorShader ColorShader;
        public DModel Model2;
        public DModel Floor;


        public SC_RigidInfo[] arrayOfGameObjects = new SC_RigidInfo[3];


        public DModel corner1Model;
        public DModel corner2Model;
        public DModel corner3Model;
        public DModel corner4Model;

        public DTerrain _grid;

        public DColorShaderer ColorShaderer;

        Texture2D backBuffer = null;
        public RenderTargetView renderView = null;
        Texture2D depthBuffer = null;
        DepthStencilView depthView = null;


        public ImpulseScene impulse;
        public bool playing;
        public float accumulator;
        public Body b = null;
        public Body f = null;

        public static Vector3 viewerPos = new Vector3(0, 0, 0);


        public static float deltaTime =  1.0f / 60f;

        protected SC_DirectX()
        {
            //Update();
            SC_Init_DirectX();
        }

        protected virtual void SC_Init_DirectX()
        {
            var form = new RenderForm("SharpDX - MiniCube Direct3D11 Sample");

            form.CreateControl();
            form.Activate();
            //form.
            //MainControl = form.ActiveControl;

            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Used for debugging dispose object references
            // Configuration.EnableObjectTracking = true;

            // Disable throws on shader compilation errors
            //Configuration.ThrowOnShaderCompileError = false;

            // Create Device and SwapChain
            SwapChain swapChain;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out swapChain);
            context = device.ImmediateContext;

            bool supportConcurentResources;
            bool supportCommandList;
            device.CheckThreadingSupport(out supportConcurentResources, out supportCommandList);

            // Ignore all windows events
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            //shaderManager = new DShaderManager();
            //shaderManager.Initialize(device, form.Handle);



            //view = Matrix.LookAtLH(new Vector3(0, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
            //proj = Matrix.Identity;

            proj = Matrix.PerspectiveFovLH((float)(Math.PI / 4), (form.Width / form.Height), 0.1f, 1000.0f);




            // Use clock
            var clock = new Stopwatch();
            clock.Start();

            // Declare texture for rendering
            bool userResized = true;


            // Setup handler on resize form
            form.UserResized += (sender, args) => userResized = true;

            // Setup full screen mode change F5 (Full) F4 (Window)
            form.KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.F5)
                    swapChain.SetFullscreenState(true, null);
                else if (args.KeyCode == Keys.F4)
                    swapChain.SetFullscreenState(false, null);
                else if (args.KeyCode == Keys.Escape)
                    form.Close();
            };


            DMatrixBuffer[] arrayOfMatrixBuff = new DMatrixBuffer[1];

            //var constantBuffer = new Buffer(device, Utilities.SizeOf<DMatrixBuffer>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
            //var InstanceBuffer = new Buffer(device, Utilities.SizeOf<SC_VR_Chunk.DInstanceType>() * instances.Length, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

            //shaderOfChunk = new SC_VR_Chunk_Shader(device, constantBuffer, Layout, VertexShader, PixelShader, InstanceBuffer, ConstantLightBuffer); //InstanceBuffer
            //Console.WriteLine("start");


            var directInput = new DirectInput();

            _Keyboard = new SharpDX.DirectInput.Keyboard(directInput);
            _Keyboard.Properties.BufferSize = 128;
            _Keyboard.Acquire();


            //int startOnce = 1;
            //threadPool = new SC_ThreadPool();
            //threadPool.startPool(1);

            var rasterDesc = new RasterizerStateDescription()
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back, //CullMode.Back
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = SharpDX.Direct3D11.FillMode.Solid,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            var _rasterState = new RasterizerState(device, rasterDesc);

            device.ImmediateContext.Rasterizer.State = _rasterState;

            BlendStateDescription description = BlendStateDescription.Default();
            var _blendState = new BlendState(device, description);

            DepthStencilStateDescription descriptioner = DepthStencilStateDescription.Default();
            descriptioner.DepthComparison = Comparison.LessEqual;
            descriptioner.IsDepthEnabled = true;

            var _depthState = new DepthStencilState(device, descriptioner);

            SamplerStateDescription descriptionator = SamplerStateDescription.Default();
            descriptionator.Filter = Filter.MinMagMipLinear;
            descriptionator.AddressU = TextureAddressMode.Wrap;
            descriptionator.AddressV = TextureAddressMode.Wrap;
            var _samplerState = new SamplerState(device, descriptionator);






            //CREATE ASSETS
            Camera = new SC_CameraClass();
            Camera.SetPosition(0, 2, -30);

            viewerPos = Camera.GetPosition();
            impulse = new ImpulseScene(deltaTime, 10); //ImpulseMath.DT
            impulse.dt = 1.0f / 60f;
            ImpulseMath ImpulseMath = new ImpulseMath();

            var someAngle = 0;




            
            //THE POLYGONS I AM ADDING.
            /*
            for (int x = -3; x < 3; x++)
            {
                for (int y = -3; y < 3; y++)
                {

                    //CIRCLES
                    //CIRCLES
                    //CIRCLES
                    var circA = impulse.add(new Circle(0.5f), 0, 0);
                    circA.position.set(x * 3, 25 + y * 3);

                    someAngle = 0;
                    circA.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
                    circA.setOriginOrient((float)(Math.PI * 0 / 180.0));
                    circA.setLastOrient((float)(Math.PI * someAngle / 180.0));
                    circA.lastAngle = someAngle;
                    circA.dynamicFriction = 0.5f;
                    circA.staticFriction = 0.5f;
                    circA.restitution = 0.2f;
                    //circA.mass = 1;
                    var ModelCirc = new DModelClassCircle();
                    if (!ModelCirc.Initialize(device, 0.5f, 20, new Vector4(0.65f, 0.25f, 0.25f, 1), 0, 0, someAngle))
                    {
                        return;
                    }
                    ModelCirc.position = new Vector2(circA.position.x, circA.position.y);
                    circA.shape.gameObjectCirc = ModelCirc;
                    //circA.shape.rigidInfo = new SC_RigidInfo();
                    //circA.shape.rigidInfo.Model = ModelCirc;
                    //CIRCLES
                    //CIRCLES
                    //CIRCLES



                    /*Vector2 pos = new Vector2(x, y + offsetStartPosY);

                    float _size = 2.75f;


                    Polygon polyg = new Polygon(0.1f * _size, 0.1f * _size);

                    var vertadder = (int)sc_maths.getSomeRandNumThousandDecimal(0, 10, 1, 0);

                    polyg.vertexCount = 4;

                    f = impulse.add(polyg, 0, 0);

                    f.position.set(pos.X, pos.Y);
                    //f.setStatic();
                    someAngle = 0;
                    f.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
                    f.setOriginOrient((float)(Math.PI * 0 / 180.0));
                    f.setLastOrient((float)(Math.PI * someAngle / 180.0));
                    f.lastAngle = someAngle;

                    var Modeler = new DModel();

                    float r = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);
                    float g = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);
                    float b = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);

                    if (!Modeler.Initialize(device, new Vector3(0.1f * _size, 0.1f * _size, 0), new Vector4(r, g, b, 1), 0, 0, 0))
                    {
                        return;
                    }

                    Modeler.position = new Vector2(f.position.x, f.position.y);
                    f.shape.rigidInfo = new SC_RigidInfo();
                    f.shape.gameObject = Modeler;
                    f.shape.rigidInfo.Model = Modeler;
                    f.justCreated = 1;
                }
            }*/






            /*//CIRCLES
            //CIRCLES
            //CIRCLES
            var circA = impulse.add(new Circle(0.5f), 0, 0);
            circA.position.set(0, 25);
            someAngle = 0;
            circA.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            circA.setOriginOrient((float)(Math.PI * 0 / 180.0));
            circA.setLastOrient((float)(Math.PI * someAngle / 180.0));
            circA.lastAngle = someAngle;
            circA.dynamicFriction = 0.5f;
            circA.staticFriction = 0.5f;
            circA.restitution = 0.2f;
            //circA.mass = 1;
            var ModelCirc = new DModelClassCircle();
            if (!ModelCirc.Initialize(device, 0.5f, 20, new Vector4(0.65f, 0.25f, 0.25f, 1), 0, 0, someAngle))
            {
                return;
            }
            ModelCirc.position = new Vector2(circA.position.x, circA.position.y);
            circA.shape.gameObjectCirc = ModelCirc;
            //circA.shape.rigidInfo = new SC_RigidInfo();
            //circA.shape.rigidInfo.Model = ModelCirc;
            //CIRCLES
            //CIRCLES
            //CIRCLES*/





            /*
            b = impulse.add(new Polygon(0.5f, 0.5f), 0, 0);
            b.position.set(0, 5);

            someAngle = 0;
            b.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            b.setOriginOrient((float)(Math.PI * 0 / 180.0));
            b.setLastOrient((float)(Math.PI * someAngle / 180.0));
            b.lastAngle = someAngle;
            b.dynamicFriction = 0.5f;
            b.staticFriction = 0.5f;
            b.restitution = 0.2f;
            //b.mass = 100;
            Model = new DModel();
            if (!Model.Initialize(device, new Vector3(0.5f, 0.5f, 0), new Vector4(0.75f, 0.75f, 0.75f, 1), 0, 0, someAngle))
            {
                return;
            }

            Model.position = new Vector2(b.position.x, b.position.y);
            b.shape.gameObject = Model;
            b.shape.rigidInfo = new SC_RigidInfo();
            b.shape.rigidInfo.Model = Model;*/





            /*Body ob = impulse.add(new Polygon(0.5f, 0.5f), 0, 0);
            ob.position.set(-0.5f, 3);
            ob.setStatic();
            someAngle = 25;
            ob.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            ob.setOriginOrient((float)(Math.PI * 0 / 180.0));
            ob.setLastOrient((float)(Math.PI * someAngle / 180.0));
            ob.lastAngle = someAngle;
            //ob.dynamicFriction = 0.2f;
            //ob.staticFriction = 0.4f;
            //ob.restitution = 0.2f;
            //ob.mass = 100;

            Model = new DModel();
            if (!Model.Initialize(device, new Vector3(0.5f, 0.5f, 0), new Vector4(0.15f, 0.75f, 0.15f, 1), 0, 0, someAngle))
            {
                return;
            }

            Model.position = new Vector2(ob.position.x, ob.position.y);
            ob.shape.rigidInfo = new SC_RigidInfo();
            ob.shape.gameObject = Model;
            ob.shape.rigidInfo.Model = Model;*/





            //FLOOR
            //FLOOR
            //FLOOR
            f = impulse.add(new Polygon(100, 0.5f), 0, 0);
            f.position.set(0, 0);
            f.setStatic();
            someAngle = 0;
            f.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            f.setOriginOrient((float)(Math.PI * 0 / 180.0));
            f.setLastOrient((float)(Math.PI * someAngle / 180.0));
            f.lastAngle = someAngle;
            Model = new DModel();
            if (!Model.Initialize(device, new Vector3(100, 0.5f, 0), new Vector4(0.15f, 0.75f, 0.15f, 1), 0, 0, 0))
            {
                return;
            }
            Model.position = new Vector2(f.position.x, f.position.y); 
            f.shape.rigidInfo = new SC_RigidInfo();
            f.shape.gameObject = Model;          
            f.shape.rigidInfo.Model = Model;

            /*var circA = impulse.add(new Circle(10), 0, 0);
            circA.position.set(0, 0);
            someAngle = 0;

            circA.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            circA.setOriginOrient((float)(Math.PI * 0 / 180.0));
            circA.setLastOrient((float)(Math.PI * someAngle / 180.0));
            circA.lastAngle = someAngle;
            circA.dynamicFriction = 0.5f;
            circA.staticFriction = 0.5f;
            circA.restitution = 0.2f;
            circA.setStatic();
            //circA.mass = 1;
            var ModelCirc = new DModelClassCircle();
            if (!ModelCirc.Initialize(device, 10, 20, new Vector4(0.65f, 0.25f, 0.25f, 1), 0, 0, someAngle))
            {
                return;
            }
            ModelCirc.position = new Vector2(circA.position.x, circA.position.y);
            circA.shape.gameObjectCirc = ModelCirc;*/
            //circA.shape.rigidInfo = new SC_RigidInfo();
            //circA.shape.rigidInfo.Model = ModelCirc;

            //FLOOR
            //FLOOR
            //FLOOR





            
            //THE POLYGONS I AM ADDING.
            float offsetStartPosY = 10;

            int cWidth = 10;
            int cHeight = 10;


            for (int x = 0; x < cWidth; x ++) //-cWidth
            {
                for (int y = 0; y < cHeight; y++) //-cHeight
                {
                    Vector2 pos = new Vector2(x, y + offsetStartPosY);

                    float _size = 2.75f;


                    Polygon polyg = new Polygon(0.1f * _size, 0.1f * _size);

                    var vertadder = (int)sc_maths.getSomeRandNumThousandDecimal(0, 10, 1, 0);

                    polyg.vertexCount = 4;

                    f = impulse.add(polyg, 0, 0);

                    f.position.set(pos.X, pos.Y);
                    //f.setStatic();
                    someAngle = 0;
                    f.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
                    f.setOriginOrient((float)(Math.PI * 0 / 180.0));
                    f.setLastOrient((float)(Math.PI * someAngle / 180.0));
                    f.lastAngle = someAngle;

                    var Modeler = new DModel();

                    float r = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);
                    float g = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);
                    float b = (float)sc_maths.getSomeRandNumThousandDecimal(0, 99, 0.01f, 0);

                    if (!Modeler.Initialize(device, new Vector3(0.1f * _size, 0.1f * _size, 0), new Vector4(r, g, b, 1), 0, 0, 0))
                    {
                        return;
                    }

                    Modeler.position = new Vector2(f.position.x, f.position.y);
                    f.shape.rigidInfo = new SC_RigidInfo();
                    f.shape.gameObject = Modeler;
                    f.shape.rigidInfo.Model = Modeler;
                    f.justCreated = 1;
                }
            }









            /*f = impulse.add(new Polygon(10, 0.5f), 0, 0);
            f.position.set(-5, 3);
            f.setStatic();
            someAngle = -25;
            f.setOrient((float)(Math.PI * someAngle / 180.0)); //(float)(0 * (180.0 / Math.PI))
            f.setOriginOrient((float)(Math.PI * 0 / 180.0));
            f.setLastOrient((float)(Math.PI * someAngle / 180.0));
            f.lastAngle = someAngle;
            Model = new DModel();
            if (!Model.Initialize(device, new Vector3(10, 0.5f, 0), new Vector4(0.15f, 0.75f, 0.15f, 1), 0, 0, 0))
            {
                return;
            }

            Model.position = new Vector2(f.position.x, f.position.y);
            f.shape.rigidInfo = new SC_RigidInfo();
            f.shape.gameObject = Model;
            f.shape.rigidInfo.Model = Model;*/










            /*arrayOfGameObjects[0] = b.shape.rigidInfo;
            arrayOfGameObjects[1] = f.shape.rigidInfo;
            arrayOfGameObjects[2] = ob.shape.rigidInfo;

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                arrayOfGameObjects[i].vertices = new Vector2[4];
                for (int v = 0; v < arrayOfGameObjects[i].Model.vertices.Length; v++)
                {
                    Vector2 vert = new Vector2(arrayOfGameObjects[i].Model.vertices[v].position.X, arrayOfGameObjects[i].Model.vertices[v].position.Y);
                    arrayOfGameObjects[i].vertices[v] = vert;
                }
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                var lowestX = arrayOfGameObjects[i].vertices.OrderBy(x => x.X).FirstOrDefault();
                var highestX = arrayOfGameObjects[i].vertices.OrderBy(x => x.X).Last();

                var lowestY = arrayOfGameObjects[i].vertices.OrderBy(y => y.Y).FirstOrDefault();
                var highestY = arrayOfGameObjects[i].vertices.OrderBy(y => y.Y).Last();

                arrayOfGameObjects[i].boundsLowX = lowestX;
                arrayOfGameObjects[i].boundsHighX = highestX;
                arrayOfGameObjects[i].boundsLowY = lowestY;
                arrayOfGameObjects[i].boundsHighY = highestY;

                arrayOfGameObjects[i].mainIndex = i;
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                Vector2 cornerBound1 = arrayOfGameObjects[i].boundsLowX;
                if (arrayOfGameObjects[i].boundsLowX.Y > arrayOfGameObjects[i].boundsLowY.Y)
                {
                    cornerBound1 = new Vector2(arrayOfGameObjects[i].boundsLowX.X, arrayOfGameObjects[i].boundsLowY.Y);
                }

                Vector2 cornerBound2 = arrayOfGameObjects[i].boundsLowY;
                if (arrayOfGameObjects[i].boundsLowY.X < arrayOfGameObjects[i].boundsHighX.X)
                {
                    cornerBound2 = new Vector2(arrayOfGameObjects[i].boundsHighX.X, arrayOfGameObjects[i].boundsLowY.Y);
                }

                Vector2 cornerBound3 = arrayOfGameObjects[i].boundsHighX;
                if (arrayOfGameObjects[i].boundsHighX.Y < arrayOfGameObjects[i].boundsHighY.Y)
                {
                    cornerBound3 = new Vector2(arrayOfGameObjects[i].boundsHighX.X, arrayOfGameObjects[i].boundsHighY.Y);
                }

                Vector2 cornerBound4 = arrayOfGameObjects[i].boundsHighY;
                if (arrayOfGameObjects[i].boundsHighY.X > arrayOfGameObjects[i].boundsLowX.X)
                {
                    cornerBound4 = new Vector2(arrayOfGameObjects[i].boundsLowX.X, arrayOfGameObjects[i].boundsHighY.Y);
                }

                arrayOfGameObjects[i].corner1 = cornerBound1;
                arrayOfGameObjects[i].corner2 = cornerBound2;
                arrayOfGameObjects[i].corner3 = cornerBound3;
                arrayOfGameObjects[i].corner4 = cornerBound4;

                //arrayOfGameObjects[i].body.shape.rigidInfo.corner1 = cornerBound1;
                //arrayOfGameObjects[i].body.shape.rigidInfo.corner2 = cornerBound2;
                //arrayOfGameObjects[i].body.shape.rigidInfo.corner3 = cornerBound3;
                //arrayOfGameObjects[i].body.shape.rigidInfo.corner4 = cornerBound4;

                arrayOfGameObjects[i].vertices[0] = arrayOfGameObjects[i].corner1;
                arrayOfGameObjects[i].vertices[1] = arrayOfGameObjects[i].corner2;
                arrayOfGameObjects[i].vertices[2] = arrayOfGameObjects[i].corner3;
                arrayOfGameObjects[i].vertices[3] = arrayOfGameObjects[i].corner4;
            }*/

            accumulator = 0f;
            playing = true;


            /*var lowestX = b.shape.gameObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(x => x.x).FirstOrDefault();
            var highestX = b.shape.gameObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(x => x.x).Last();

            var lowestY = b.shape.gameObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(y => y.y).FirstOrDefault();
            var highestY = b.shape.gameObject.GetComponent<MeshFilter>().mesh.vertices.OrderBy(y => y.y).Last();

            b.shape.rigidInfo.boundsLowX = lowestX;
            b.shape.rigidInfo.boundsHighX = highestX;
            b.shape.rigidInfo.boundsLowY = lowestY;
            b.shape.rigidInfo.boundsHighY = highestY;

            Vector3 cornerBound1 = b.shape.rigidInfo.boundsLowX;
            if (b.shape.rigidInfo.boundsLowX.Y > b.shape.rigidInfo.boundsLowY.Y)
            {
                cornerBound1 = new Vector3(b.shape.rigidInfo.boundsLowX.X, b.shape.rigidInfo.boundsLowY.Y);
            }
            Vector3 cornerBound2 = b.shape.rigidInfo.boundsLowY;
            if (b.shape.rigidInfo.boundsLowY.X < b.shape.rigidInfo.boundsHighX.X)
            {
                cornerBound2 = new Vector3(b.shape.rigidInfo.boundsHighX.X, b.shape.rigidInfo.boundsLowY.Y);
            }
            Vector3 cornerBound3 = b.shape.rigidInfo.boundsHighX;
            if (b.shape.rigidInfo.boundsHighX.Y < b.shape.rigidInfo.boundsHighY.Y)
            {
                cornerBound3 = new Vector3(b.shape.rigidInfo.boundsHighX.X, b.shape.rigidInfo.boundsHighY.Y);
            }

            Vector3 cornerBound4 = b.shape.rigidInfo.boundsHighY;
            if (b.shape.rigidInfo.boundsHighY.X > b.shape.rigidInfo.boundsLowX.X)
            {
                cornerBound4 = new Vector3(b.shape.rigidInfo.boundsLowX.X, b.shape.rigidInfo.boundsHighY.Y);
            }
            b.shape.rigidInfo.corner1 = cornerBound1;
            b.shape.rigidInfo.corner2 = cornerBound2;
            b.shape.rigidInfo.corner3 = cornerBound3;
            b.shape.rigidInfo.corner4 = cornerBound4;*/




































            /*Model = new DModel();
            if (!Model.Initialize(device, new Vector3(1, 1, 0), new Vector4(0, 1, 1, 1), 0, 0, 45))
            {
                return;
            }


            Model = new DModel();
            if (!Model.Initialize(device, new Vector3(1, 1, 0), new Vector4(0, 1, 1, 1), 0, 0, 45))
            {
                return;
            }


            //_grid = new DTerrain();
            //_grid.Initialize(device, 20, 20, 1);

            SC_RigidInfo rect1 = new SC_RigidInfo();
            //rect1.currentObject = one;
            rect1.Model = Model;
            Matrix originRot = Model.originRot;
            //rect1.position = Model.position;
            rect1.position = new Vector2(-2, 5);
            //originRot.M41 = rect1.position.X;
            //originRot.M42 = rect1.position.Y;
            rect1.someWorldMatrix = originRot;
            rect1.supposedPos = rect1.position;
            rect1.Model.position = rect1.position;

            rect1.velocity = new Vector2(0, 0);
            rect1.mass = 10;
            rect1.isStatic = 0;
            rect1.lastFramePos = rect1.position;
            rect1.lerp = 0;
            rect1.lerpPos = null;
            rect1.timeToHit = 0;
            rect1.indexToCollideWith = -1;
            rect1.collideNorm = null;
            rect1.mainIndex = -1;
            rect1.collidePoint = null;
            rect1.lastFrameVelo = new Vector2(0, 0);
            rect1.extendedVelocity = new Vector2(0, 0);
            rect1.restitution = 0.95f;
            rect1.isStaticTemp = 0;
            rect1.angle = 0;
            rect1.angularVelocity = 0;
            rect1.angularAcceleration = 0;
            rect1.torque = 5;
            rect1.inertia = 100;
            rect1.invMass = (rect1.mass != 0.0f) ? 1.0f / rect1.mass : 0.0f;
            rect1.orientation = 0;
            rect1.invInertia = (rect1.inertia != 0.0f) ? 1.0f / rect1.inertia : 0.0f;
            rect1.angularVel = new Vector2(0, 0);
            rect1.airFriction = 0.999f;
            //rect1.rotZ = Model.origin;


            Floor = new DModel();
            if (!Floor.Initialize(device, new Vector3(500, 1, 0), new Vector4(0, 0, 1, 1), 0, 0, 0))
            {
                return;
            }


            SC_RigidInfo rect2 = new SC_RigidInfo();
            //rect2.currentObject = two;
            rect2.Model = Floor;
            //rect2.position = Floor.position;
            rect2.position = new Vector2(0, 0);
            rect2.supposedPos = rect2.position;
            rect2.Model.position = rect2.position;
            Matrix positioner = Matrix.Identity;
            positioner.M41 = rect2.position.X;
            positioner.M42 = rect2.position.Y;
            rect2.someWorldMatrix = positioner;


            rect2.velocity = new Vector2(0, 0);
            rect2.mass = 10;
            rect2.isStatic = 1;
            rect2.lastFramePos = rect2.position;
            rect2.lerp = 1;
            rect2.lerpPos = null;
            rect2.timeToHit = 0;
            rect2.indexToCollideWith = -1;
            rect2.collideNorm = null;
            rect2.mainIndex = -1;
            rect2.collidePoint = null;
            rect2.lastFrameVelo = new Vector2(0, 0);
            rect2.extendedVelocity = new Vector2(0, 0);
            rect2.restitution = 0.25f;
            rect2.isStaticTemp = 0;
            rect2.angle = 0;
            rect2.angularVelocity = 0;
            rect2.angularAcceleration = 0;
            rect2.torque = 0;
            rect2.inertia = 15;
            rect2.invMass = (rect2.mass != 0.0f) ? 1.0f / rect2.mass : 0.0f;
            rect2.orientation = 0;
            rect2.invInertia = (rect2.inertia != 0.0f) ? 1.0f / rect2.inertia : 0.0f;



            Model2 = new DModel();
            if (!Model2.Initialize(device, new Vector3(1, 1, 0), new Vector4(0, 1, 0, 1), 0, 0, 35))
            {
                return;
            }

            SC_RigidInfo rect3 = new SC_RigidInfo();
            rect3.Model = Model2;
            rect3.position = new Vector2(0, 3);
            rect3.someWorldMatrix = Model2.originRot;
            rect3.supposedPos = rect3.position;
            rect3.Model.position = rect3.position;

            rect3.velocity = new Vector2(0, 0);
            rect3.mass = 10;
            rect3.isStatic = 1;
            rect3.lastFramePos = rect3.position;
            rect3.lerp = 0;
            rect3.lerpPos = null;
            rect3.timeToHit = 0;
            rect3.indexToCollideWith = -1;
            rect3.collideNorm = null;
            rect3.mainIndex = -1;
            rect3.collidePoint = null;
            rect3.lastFrameVelo = new Vector2(0, 0);
            rect3.extendedVelocity = new Vector2(0, 0);
            rect3.restitution = 0.75f;
            rect3.isStaticTemp = 0;
            rect3.angle = 0;
            rect3.angularVelocity = 0;
            rect3.angularAcceleration = 0;
            rect3.torque = 0;
            rect3.inertia = 10;
            rect3.invMass = (rect3.mass != 0.0f) ? 1.0f / rect3.mass : 0.0f;
            rect3.orientation = 0;
            rect3.invInertia = (rect3.inertia != 0.0f) ? 1.0f / rect3.inertia : 0.0f;
            rect3.angularVel = new Vector2(0, 0);


















            arrayOfGameObjects[0] = rect1;
            arrayOfGameObjects[1] = rect2;
            arrayOfGameObjects[2] = rect3;


            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                arrayOfGameObjects[i].vertices = new Vector2[4];
                for (int v = 0; v < arrayOfGameObjects[i].Model.vertices.Length; v++)
                {
                    Vector2 vert = new Vector2(arrayOfGameObjects[i].Model.vertices[v].position.X, arrayOfGameObjects[i].Model.vertices[v].position.Y);
                    arrayOfGameObjects[i].vertices[v] = vert;
                }
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                var lowestX = arrayOfGameObjects[i].vertices.OrderBy(x => x.X).FirstOrDefault();
                var highestX = arrayOfGameObjects[i].vertices.OrderBy(x => x.X).Last();

                var lowestY = arrayOfGameObjects[i].vertices.OrderBy(y => y.Y).FirstOrDefault();
                var highestY = arrayOfGameObjects[i].vertices.OrderBy(y => y.Y).Last();

                arrayOfGameObjects[i].boundsLowX = lowestX;
                arrayOfGameObjects[i].boundsHighX = highestX;
                arrayOfGameObjects[i].boundsLowY = lowestY;
                arrayOfGameObjects[i].boundsHighY = highestY;

                arrayOfGameObjects[i].mainIndex = i;
            }

            for (int i = 0; i < arrayOfGameObjects.Length; i++)
            {
                Vector2 cornerBound1 = arrayOfGameObjects[i].boundsLowX;
                if (arrayOfGameObjects[i].boundsLowX.Y > arrayOfGameObjects[i].boundsLowY.Y)
                {
                    cornerBound1 = new Vector2(arrayOfGameObjects[i].boundsLowX.X, arrayOfGameObjects[i].boundsLowY.Y);
                }
                Vector2 cornerBound2 = arrayOfGameObjects[i].boundsLowY;
                if (arrayOfGameObjects[i].boundsLowY.X < arrayOfGameObjects[i].boundsHighX.X)
                {
                    cornerBound2 = new Vector2(arrayOfGameObjects[i].boundsHighX.X, arrayOfGameObjects[i].boundsLowY.Y);
                }
                Vector2 cornerBound3 = arrayOfGameObjects[i].boundsHighX;
                if (arrayOfGameObjects[i].boundsHighX.Y < arrayOfGameObjects[i].boundsHighY.Y)
                {
                    cornerBound3 = new Vector2(arrayOfGameObjects[i].boundsHighX.X, arrayOfGameObjects[i].boundsHighY.Y);
                }

                Vector2 cornerBound4 = arrayOfGameObjects[i].boundsHighY;
                if (arrayOfGameObjects[i].boundsHighY.X > arrayOfGameObjects[i].boundsLowX.X)
                {
                    cornerBound4 = new Vector2(arrayOfGameObjects[i].boundsLowX.X, arrayOfGameObjects[i].boundsHighY.Y);
                }

                arrayOfGameObjects[i].corner1 = cornerBound1;
                arrayOfGameObjects[i].corner2 = cornerBound2;
                arrayOfGameObjects[i].corner3 = cornerBound3;
                arrayOfGameObjects[i].corner4 = cornerBound4;

                arrayOfGameObjects[i].vertices[0] = arrayOfGameObjects[i].corner1;
                arrayOfGameObjects[i].vertices[1] = arrayOfGameObjects[i].corner2;
                arrayOfGameObjects[i].vertices[2] = arrayOfGameObjects[i].corner3;
                arrayOfGameObjects[i].vertices[3] = arrayOfGameObjects[i].corner4;
            }*/




            corner1Model = new DModel();
            if (!corner1Model.Initialize(device, new Vector3(0.1f, 0.1f, 0), new Vector4(1, 0, 0, 1), 0, 0, 0))
            {
                return;
            }
            corner2Model = new DModel();
            if (!corner2Model.Initialize(device, new Vector3(0.1f, 0.1f, 0), new Vector4(1, 0, 0, 1), 0, 0, 0))
            {
                return;
            }
            corner3Model = new DModel();
            if (!corner3Model.Initialize(device, new Vector3(0.1f, 0.1f, 0), new Vector4(1, 0, 0, 1), 0, 0, 0))
            {
                return;
            }
            corner4Model = new DModel();
            if (!corner4Model.Initialize(device, new Vector3(0.1f, 0.1f, 0), new Vector4(1, 0, 0, 1), 0, 0, 0))
            {
                return;
            }

















            ColorShader = new DColorShader();
            ColorShader.Initialize(device, form.Handle);
            //CREATE ASSETS

            ColorShaderer = new DColorShaderer();
            ColorShaderer.Initialize(device, form.Handle);
            //CREATE ASSETS





            RenderLoop.Run(form, () =>
            {
                device.ImmediateContext.Rasterizer.State = _rasterState;
                device.ImmediateContext.OutputMerger.SetBlendState(_blendState);
                device.ImmediateContext.OutputMerger.SetDepthStencilState(_depthState);
                device.ImmediateContext.PixelShader.SetSampler(0, _samplerState);

                context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                context.ClearRenderTargetView(renderView, SharpDX.Color.CornflowerBlue);

                // If Form resized
                if (userResized)
                {
                    // Dispose all previous allocated resources
                    Utilities.Dispose(ref backBuffer);
                    Utilities.Dispose(ref renderView);
                    Utilities.Dispose(ref depthBuffer);
                    Utilities.Dispose(ref depthView);

                    // Resize the backbuffer
                    swapChain.ResizeBuffers(desc.BufferCount, form.ClientSize.Width, form.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

                    // Get the backbuffer from the swapchain
                    backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);

                    // Renderview on the backbuffer
                    renderView = new RenderTargetView(device, backBuffer);

                    // Create the depth buffer
                    depthBuffer = new Texture2D(device, new Texture2DDescription()
                    {
                        Format = Format.D32_Float_S8X24_UInt,
                        ArraySize = 1,
                        MipLevels = 1,
                        Width = form.ClientSize.Width,
                        Height = form.ClientSize.Height,
                        SampleDescription = new SampleDescription(1, 0),
                        Usage = ResourceUsage.Default,
                        BindFlags = BindFlags.DepthStencil,
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None
                    });

                    // Create the depth buffer view
                    depthView = new DepthStencilView(device, depthBuffer);

                    // Setup targets and viewport for rendering
                    context.Rasterizer.SetViewport(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));
                    context.OutputMerger.SetTargets(depthView, renderView);

                    // Setup new projection matrix with correct aspect ratio
                    proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 100.0f);
                    // We are done resizing
                    userResized = false;
                }




                Update();

                /*if (startThread == 0)
                {
                    Thread _mainTasker00 = new Thread((tester0000) =>
                    {


                    _thread_main_loop:

          

                        Thread.Sleep(0);
                        goto _thread_main_loop;

                        //_thread_start:
                    }, 0); //100000 //999999999

                    _mainTasker00.IsBackground = true;
                    _mainTasker00.SetApartmentState(ApartmentState.STA);
                    _mainTasker00.Start();
                    startThread = 1;
                }*/
              



                // Present!
                swapChain.Present(0, PresentFlags.None);

                Thread.Sleep(1);
            });

            // Release all resources
            //signature.Dispose();
            //vertexShaderByteCode.Dispose();
            //VertexShader.Dispose();
            //pixelShaderByteCode.Dispose();
            //PixelShader.Dispose();
            //vertices.Dispose();
            //Layout.Dispose();
            //constantBuffer.Dispose();
            depthBuffer.Dispose();
            depthView.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            context.ClearState();
            context.Flush();
            device.Dispose();
            context.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }

        public int startThread = 0;



        protected abstract void Update();
        /*protected virtual void Update()
        {

        }*/


        /*protected override sealed void Initialize()
        {

        }*/

        private void callFunctionSafe(Func<int> text, RenderForm form)
        {
            var test = new SafeCallDelegate(callFunctionSafe);

            var result = form.BeginInvoke(text);
            form.EndInvoke(result);

        }
        private delegate void SafeCallDelegate(Func<int> someFunction, RenderForm form);



        /*public struct chunkData
        {
            public SharpDX.Direct3D11.Buffer instanceBuffer;
            public Vector4[][] arrayOfInstanceVertex;
            public SC_VR_Chunk.DInstanceType[] arrayOfInstancePos;
            public int[][] arrayOfInstanceIndices;
            public Vector3[][] arrayOfInstanceNormals;
            public Vector2[][] arrayOfInstanceTextureCoordinates;
            public Vector4[][] arrayOfInstanceColors;
            public SC_VR_Chunk.DVertex[][] dVertexData;

            public SharpDX.Direct3D11.Device Device;
            public Matrix worldMatrix;
            public Matrix viewMatrix;
            public Matrix projectionMatrix;
            //public DShaderManager shaderManager;
            public SC_VR_Chunk_Shader chunkShader;
            public DMatrixBuffer[] matrixBuffer;
            public DLightBuffer[] lightBuffer;
            public SharpDX.Direct3D11.Buffer[] vertBuffers;
            public SharpDX.Direct3D11.Buffer[] colorBuffers;
            public SharpDX.Direct3D11.Buffer[] indexBuffers;
            public SharpDX.Direct3D11.Buffer[] normalBuffers;
            public SharpDX.Direct3D11.Buffer[] texBuffers;
            public SharpDX.Direct3D11.Buffer[] dVertBuffers;

            public DeviceContext _renderingContext;
            public SharpDX.Direct3D11.Buffer[] instanceBuffers; 
        }*/
    }
}
