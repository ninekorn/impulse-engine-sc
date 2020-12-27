using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


namespace SC_Console_APP
{
    public class SC_VR_Chunk
    {

        public const int mapWidth = 1;
        public const int mapHeight = 1;
        public const int mapDepth = 1;


        public const int tinyChunkWidth = 1;
        public const int tinyChunkHeight = 1;
        public const int tinyChunkDepth = 1;

        public const int mapObjectInstanceWidth = 1;
        public const int mapObjectInstanceHeight = 1;
        public const int mapObjectInstanceDepth = 1;




        public SC_VR_Chunk_Shader shaderOfChunk { get; set; }

        public SharpDX.Direct3D11.Buffer InstanceBuffer { get; set; }

        public int VertexCount { get; set; }
        public int IndexCount { get; set; }

        public DVertex[] Vertices { get; set; }
        public int[] indices;

        private float _sizeX = 0;
        private float _sizeY = 0;
        private float _sizeZ = 0;

        public DVertex[][] arrayOfDVertex { get; set; }
        public DInstanceType[] instances { get; set; }
        // Constructor
        public SC_VR_Chunk(float xi, float yi, float zi, Vector4 color, int width, int height, int depth, Vector3 pos) //,DInstanceType[] _instances
        {
            this._color = color;
            this._sizeX = xi;
            this._sizeY = yi;
            this._sizeZ = zi;
            this._chunkPos = pos;

            VertexCount = 1;
            // Set number of vertices in the index array.
            IndexCount = 3;

            byte[] mapperanus;

            arrayOfInstanceVertex = new Vector4[mapWidth * mapHeight * mapDepth][];
            arrayOfInstanceIndices = new int[mapWidth * mapHeight * mapDepth][];
            arrayOfInstanceNormals = new Vector3[mapWidth * mapHeight * mapDepth][];
            arrayOfInstanceTexturesCoordinates = new Vector2[mapWidth * mapHeight * mapDepth][];

            InstanceCount = mapWidth * mapHeight * mapDepth;
            instances = new DInstanceType[InstanceCount];

            Vector3 position;
            //chunk newChunker;
            arrayOfDVertex = new DVertex[InstanceCount][];
            DVertex[] arrayOfD;// = new DVertex[];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int z = 0; z < mapDepth; z++)
                    {
                        var xx = x;
                        var yy = y;// (mapHeight - 1) - y;
                        var zz = z;

                        position = new Vector3(x, y, z);
                        //newChunker = new chunk();
                        //position.X = position.X + (_chunkPos.X ); //*1.05f
                        //position.Y = position.Y + (_chunkPos.Y );
                        //position.Z = position.Z + (_chunkPos.Z );

                        position.X *= ((tinyChunkWidth * planeSize));
                        position.Y *= ((tinyChunkHeight * planeSize));
                        position.Z *= ((tinyChunkDepth * planeSize));

                        //Console.WriteLine(_chunkPos.X);

                        position.X = position.X + (_chunkPos.X); //*1.05f
                        position.Y = position.Y + (_chunkPos.Y);
                        position.Z = position.Z + (_chunkPos.Z);






                        arrayOfInstanceVertex[xx + mapWidth * (yy + mapHeight * zz)] = vertexArray0; //new Vector4(vertexArray0[v].X, vertexArray0[v].Y, vertexArray0[v].Z, 1);
                        arrayOfInstanceIndices[xx + mapWidth * (yy + mapHeight * zz)] = indicesArray0;
                        //arrayOfDVertex[xx + mapWidth * (yy + mapHeight * zz)] = arrayOfD;





                        //byte[] tester = newChunker.startBuildingArray(position, out vertexArray0, out indicesArray0, out mapperanus, out arrayOfD, out normals, out texturesCoordinates);

                        //arrayOfInstanceVertex[xx + mapWidth * (yy + mapHeight * zz)] = vertexArray0; //new Vector4(vertexArray0[v].X, vertexArray0[v].Y, vertexArray0[v].Z, 1);
                        //arrayOfInstanceIndices[xx + mapWidth * (yy + mapHeight * zz)] = indicesArray0;
                        //arrayOfDVertex[xx + mapWidth * (yy + mapHeight * zz)] = arrayOfD;

                        arrayOfInstanceNormals[xx + mapWidth * (yy + mapHeight * zz)] = normals;
                        arrayOfInstanceTexturesCoordinates[xx + mapWidth * (yy + mapHeight * zz)] = texturesCoordinates;

                        //instances[xx + mapWidth * (yy + mapHeight * zz)] = new Vector4[1];
                        //instances[xx + mapWidth * (yy + mapHeight * zz)][0] = new Vector4(position.X, position.Y, position.Z, 1);

                        instances[xx + mapWidth * (yy + mapHeight * zz)] = new DInstanceType()
                        {
                            position = position,
                        };

                        /*= new DInstanceType()
                        {
                            position = position,
                        };*/

                    }
                }
            }























            /*for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int z = 0; z < mapDepth; z++)
                    {
                        var xx = x;
                        var yy = y;//  (mapHeight - 1) - y;
                        var zz = z;


                        Vector3 position = new Vector3(x, y, z);

                        position.X *= ((tinyChunkWidth * planeSize));
                        position.Y *= ((tinyChunkHeight * planeSize));
                        position.Z *= ((tinyChunkDepth * planeSize));

                        position.X = position.X + (_chunkPos.X); //*1.05f
                        position.Y = position.Y + (_chunkPos.Y);
                        position.Z = position.Z + (_chunkPos.Z);

                        instances[xx + mapWidth * (yy + mapHeight * zz)] = new DInstanceType()
                        {
                            position = position,
                        };
                    }
                }
            }*/
        }



        public float planeSize = 0.1f;

        //[StructLayout(LayoutKind.Sequential)]
        public struct DInstanceType
        {
            public Vector3 position;
            //public int[] chunkMap;
        };

        //[StructLayout(LayoutKind.Sequential)]
        public struct DColorType
        {
            public Vector4[] Color;
            //public int[] chunkMap;
        };

        DColorType[] arrayOfColor;

        public int InstanceCount = 0;

        Vector4[] vertexArray0;
        int[] indicesArray0;

        Vector3[] normals;
        Vector2[] texturesCoordinates;


        Vector4[] vertexArray => vertexArray0;
        int[] indicesArray => indicesArray0;

        // Structures.
        [StructLayout(LayoutKind.Sequential)]
        public struct DVertex
        {
            public Vector4 position;
            public Vector4 color;
            public Vector3 normal;
            //public Vector4 tangent;
            //public Vector3 binormal;
            public Vector2 tex;

        }

        public Vector4 _color;


        //public static int instanceCounter = 0;
        public int instanceCounter { get; set; }
        public byte[] map { get; set; }
        // Methods.
        public Vector3 _chunkPos { get; set; }

        private bool InitializeBuffer(SharpDX.Direct3D11.Device device)
        {
            try
            {






                /*VertexCount = vertexArray0.Length;
                IndexCount = indicesArray.Length;

                Vertices = new DVertex[VertexCount];

                for (int v = 0; v < vertexArray0.Length; v++)
                {
                    Vertices[v] = new DVertex()
                    {
                        position = vertexArray0[v],
                        color = _color,
                    };
                }
                indices = indicesArray;*/






                //Set number of vertices in the vertex array.
                /*VertexCount = 4;
                // Set number of vertices in the index array.
                IndexCount = 6;


                // Create the vertex array and load it with data.
                Vertices = new[]
                 {
                     //new DVertex()
                     //{
                     //    position = new Vector3(-1*_sizeX, -1*_sizeY, 1*_sizeZ),
                     //    color = _color,                    
                     //},
                     new DVertex()
                     {
                         position = new Vector4(0*_sizeX, 1*_sizeY, 1*_sizeZ,1),
                         color = _color,
                         //texture = new Vector2(0, 1),
                     },
                     //new DVertex()
                     //{
                     //    position = new Vector3(1*_sizeX, -1*_sizeY, 1*_sizeZ),
                     //    color = _color,
                     //},
                     new DVertex()
                     {
                         position = new Vector4(1*_sizeX, 1*_sizeY, 1*_sizeZ,1),
                         color = _color,
                         //texture = new Vector2(0, 1),
                     },

                     //new DVertex()
                     //{
                     //    position = new Vector3(-1*_sizeX, -1*_sizeY, -1*_sizeZ),
                     //    color = _color,
                     //},
                     new DVertex()
                     {
                         position = new Vector4(0*_sizeX, 1*_sizeY, 0*_sizeZ,1),
                         color = _color,
                         //texture = new Vector2(0, 1),
                     },
                     //new DVertex()
                     //{
                     //    position = new Vector3(1*_sizeX, -1*_sizeY, -1*_sizeZ),
                     //    color = _color,
                     //},
                     new DVertex()
                     {
                         position = new Vector4(1*_sizeX, 1*_sizeY, 0*_sizeZ,1),
                         color = _color,
                         //texture = new Vector2(0, 1),
                     },
                 };

                indices = new int[]
                {
                     2, // Bottom left.
                     1, // Top middle.
                     0,  // Bottom right.
                     1,
                     2,
                     3,
                };*/



                /*// Create Indicies to load into the IndexBuffer.
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
                 };*/


                //VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, Vertices, Utilities.SizeOf<DVertex>() * Vertices.Length, ResourceUsage.Dynamic, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

                //IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);

                //InstanceBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, instances, Utilities.SizeOf<DInstanceType>() * instances.Length, ResourceUsage.Dynamic, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Vector4[][] arrayOfInstanceVertex { get; set; }
        public int[][] arrayOfInstanceIndices { get; set; }


        public Vector3[][] arrayOfInstanceNormals { get; set; }
        public Vector2[][] arrayOfInstanceTexturesCoordinates { get; set; }



        private void ShutDownBuffers()
        {
            InstanceBuffer?.Dispose();
            InstanceBuffer = null;
        }
    }
}