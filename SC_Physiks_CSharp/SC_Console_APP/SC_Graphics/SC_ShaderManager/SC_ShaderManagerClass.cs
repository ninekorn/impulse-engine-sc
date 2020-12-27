using SharpDX;
using SharpDX.Direct3D11;
using System;

//using SC_SkYaRk_Clean.SC_Graphics.SC_Textures.SC_VR_Desktop_Screen_Textures;

namespace SC_Console_APP
{
    public class DShaderManager                 // 77 lines
    {
        //public SC_VR_Chunk_Shader chunkShader { get; set; }

        /*public DShaderManager(SharpDX.Direct3D11.Device _device, SharpDX.Direct3D11.Buffer _constantBuffer)
        {
            chunkShader = new SC_VR_Chunk_Shader(_device, _constantBuffer);
        }*/


        // Properties
        //public DTextureShader TextureShader { get; set; }

        //public SC_VR_Touch_Shader touchShader { get; set; }
        //public DLightShader LightShader { get; set; }
        //public DBumpMapShader BumpMapShader { get; set; }

        // Methods


        //public SC_VR_Terrain_Shader terrainShader { get; set; }


        //public DTextureShader dtextureShader { get; set; }

        public void Initialize(Device device, IntPtr windowsHandle) //, float x, float y, float z, Vector4 color,Matrix worldMatrix
        {
            TextureShader = new DTextureShader();
            TextureShader.Initialize(device, windowsHandle);




            // Initialize the texture shader object.
            //if (!TextureShader.Initialize(device, windowsHandle))
            //    return false;

            // Create the texture shader object.
            /*TextureShader = new SC_VR_Desktop_Screen_Shader();

            // Initialize the texture shader object.
            if (!TextureShader.Initialize(device, windowsHandle))
                return false;
            */


            //touchShader = new SC_VR_Touch_Shader();

            // Initialize the texture shader object.
            //if (!touchShader.Initialize(device, windowsHandle)) //, x, y, z,  color, worldMatrix
            //    return false;



            //terrainShader = new SC_VR_Terrain_Shader();

            // Initialize the texture shader object.
            //if (!terrainShader.Initialize(device, windowsHandle))
            //    return false;





            // Initialize the texture shader object.
            //if (!chunkShader.Initialize(device, windowsHandle))
            //    return false;


            //dtextureShader = new DTextureShader();

            // Initialize the texture shader object.
            //if (!dtextureShader.Initialize(device, windowsHandle))
            //    return false;



            /*// Create the texture shader object.
            TextureShader = new DTextureShader();

            // Initialize the texture shader object.
            if (!TextureShader.Initialize(device, windowsHandle))
                return false;

            // Create the light shader object.
            LightShader = new DLightShader();

            // Initialize the light shader object.
            if (!LightShader.Initialize(device, windowsHandle))
                return false;

            // Create the bump map shader object.
            BumpMapShader = new DBumpMapShader();

            // Initialize the bump map shader object.
            if (!BumpMapShader.Initialize(device, windowsHandle))
                return false;
                */
            //return true;
        }
        /*public void ShutDown()
        {
            // Release the bump map shader object.
            BumpMapShader?.ShutDown();
            BumpMapShader = null;
            // Release the light shader object.
            LightShader?.ShutDown();
            LightShader = null;
            // Release the texture shader object.
            TextureShader?.ShutDown();
            TextureShader = null;
        }*/

        /*public bool RenderTextureShader(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ShaderResourceView texture)
        {
            // Render the model using the texture shader.
            if (!TextureShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, texture))
                return false;

            return true;
        }*/

        /*public bool RenderTouchTextureShader(DeviceContext deviceContext, int indexCount , Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, int vertexCount, int instanceCount, Matrix[] _worldMatrix)
        {
            // Render the model using the texture shader.

            touchShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, vertexCount, instanceCount,  _worldMatrix);
            /*if (!touchShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, vertexCount, instanceCount)) //, worldMatrix, viewMatrix, projectionMatrix, texture
                return false;
            
            return true;
        }

        public bool RenderTerrain(DeviceContext deviceContext, int indexCount,  Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            // Render the model using the texture shader.

            terrainShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix);

            //if (!terrainShader.Render(deviceContext, indexCount, worldViewProjection)) //, worldMatrix, viewMatrix, projectionMatrix, texture
            //    return false;

            return true;
        }*/

        //ShaderManager.RenderChunkShader(Device, instanceBuffer, colorBuffer, arrayOfInstanceVertex, arrayOfInstancePos, arrayOfInstanceIndices);

        /*public bool RenderChunkShader(SC_DirectX.chunkData chunkDat)
        {
            //SharpDX.Direct3D11.Device device, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, SharpDX.Direct3D11.Buffer instanceBuffer, Vector4[][] arrayOfInstanceVertex,SC_Graphics.SC_Models.SC_VR_Chunk.DInstanceType[] arrayOfInstancePos,int[][] arrayOfInstanceIndices,SC_Models.SC_VR_Chunk.DVertexData[][] dVertexData)

            // Render the model using the texture shader.

            //chunkShader.Render(device,  worldMatrix,  viewMatrix,  projectionMatrix, instanceBuffer, arrayOfInstanceVertex, arrayOfInstancePos,arrayOfInstanceIndices, dVertexData);
            //chunkShader.Render(chunkDat);



            /*if (!touchShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, vertexCount, instanceCount)) //, worldMatrix, viewMatrix, projectionMatrix, texture
                return false;
            
            return true;
        }*/


        /*public bool RenderTexturedTrig(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, int vertexCount, int instanceCount, Matrix[] _worldMatrix)
        {
            // Render the model using the texture shader.

            dtextureShader.Render(deviceContext, vertexCount, instanceCount, worldMatrix, viewMatrix, projectionMatrix);

            return true;
        }*/


        /*public bool RenderLightShader(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ShaderResourceView texture, Vector3 lightDirection, Vector4 ambiant, Vector4 diffuse, Vector3 cameraPosition, Vector4 specular, float specualrPower)
        {
            // Render the model using the light shader.
            if (!LightShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, texture, lightDirection, ambiant, diffuse, cameraPosition, specular, specualrPower))
                return false;

            return true;
        }
        public bool RenderBumpMapShader(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ShaderResourceView colorTexture, ShaderResourceView normalTexture, Vector3 lightDirection, Vector4 diffuse)
        {
            // Render the model using the bump map shader.
            if (!BumpMapShader.Render(deviceContext, indexCount, worldMatrix, viewMatrix, projectionMatrix, colorTexture, normalTexture, lightDirection, diffuse))
                return false;

            return true;
        }*/
    }
}