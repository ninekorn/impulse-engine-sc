
/////////////
// GLOBALS //
/////////////
cbuffer MatrixBuffer :register(b0)
{
	float4x4 world;
	float4x4 view;
	float4x4 proj;
};

cbuffer LightBuffer :register(b1)
{
	float4 ambientColor;
	float4 diffuseColor;
	float3 lightDirection;
	float padding;
};



//////////////
// TYPEDEFS //
//////////////
struct VertexInputType
{
    float4 position : POSITION;
	float4 color : COLOR;
	float3 normal : NORMAL;
    float2 tex : TEXCOORD;
	float3 instancePosition : POSITION1;
};

struct PixelInputType
{
    float4 position : SV_POSITION;
	float4 color : COLOR;
	float3 normal : NORMAL;
    float2 tex : TEXCOORD;
};


////////////////////////////////////////////////////////////////////////////////
// Vertex Shader
////////////////////////////////////////////////////////////////////////////////
//[maxvertexcount(96)] 
PixelInputType TextureVertexShader(VertexInputType input)
{
    PixelInputType output;
    
	// Change the position vector to be 4 units for proper matrix calculations.
    input.position.w = 1.0f;

	// Update the position of the vertices based on the data for this particular instance.
    input.position.x += input.instancePosition.x;// + input.color1.x;
    input.position.y += input.instancePosition.y;// + input.color1.y;
    input.position.z += input.instancePosition.z;// + input.color1.z;

	// Calculate the position of the vertex against the world, view, and projection matrices.
    output.position = mul(input.position, world);// + float4( input.instancePosition,0);
    output.position = mul(output.position, view);
    output.position = mul(output.position, proj);
    //output.position = input.position;

	//float2 noise = (frac(sin(dot(float2(output.position.x,output.position.z) ,float2(12.9898,78.233)*2.0)) * 43758.5453));
    //float test = abs(noise.x + noise.y) * 0.5;
    //output.color = float4(input.color.x + (input.position.x*0.1),input.color.y+ (input.position.y*0.1),input.color.z+ (input.position.z*0.1),input.color.w);
   

	output.normal = mul(input.normal, (float3x3)world);
	output.normal = normalize(output.normal);

	//output.normal = input.normal;
	output.tex = input.tex;
	output.color = input.color;
 
	return output;
}








/*float rand_1_05(in float2 uv)
{
    float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
    return abs(noise.x + noise.y) * 0.5;
}

float2 rand_2_10(in float2 uv) {
    float noiseX = (frac(sin(dot(uv, float2(12.9898,78.233) * 2.0)) * 43758.5453));
    float noiseY = sqrt(1 - noiseX * noiseX);
    return float2(noiseX, noiseY);
}

float2 rand_2_0004(in float2 uv)
{
    float noiseX = (frac(sin(dot(uv, float2(12.9898,78.233)      )) * 43758.5453));
    float noiseY = (frac(sin(dot(uv, float2(12.9898,78.233) * 2.0)) * 43758.5453));
    return float2(noiseX, noiseY) * 0.004;
}*/




















/*cbuffer data :register(b0)
{
	float4x4 world;
	float4x4 view;
	float4x4 proj;
	float4 lightDirection;
	float4 viewDirection;
	float4 bias;
};

struct VS_IN
{
	float4 position : POSITION;
	float4 color : COLOR;
	float3 normal : NORMAL;
	float4 tangent: TANGENT;
	float3 binormal: BINORMAL;
	float2 texcoord : TEXCOORD;
	float3 instancePosition : POSITION1;
};

struct PS_IN
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD;
	float3 lightDirection:LIGHT;
	float3 viewDirection:VIEW;
};

//texture
//Texture2D textureMap:register(t0);
//Texture2D normalMap:register(t1);
Texture2D normalMap:register(t0);


SamplerState textureSampler;

PS_IN VS( VS_IN input)
{
	PS_IN output = (PS_IN)0;

	float3 binormal = cross(input.normal, input.tangent.xyz) * input.tangent.w;

	input.binormal = binormal;

	float3 B=mul(world,input.binormal);
	float3 T=mul(world,input.tangent);
	float3 N=mul(world,input.normal);

	//output.position = mul(worldViewProj,input.position);

	input.position.x += input.instancePosition.x;// + input.color1.x;
    input.position.y += input.instancePosition.y;// + input.color1.y;
    input.position.z += input.instancePosition.z;// + input.color1.z;

	output.position = mul(input.position, world);
    output.position = mul(output.position, view);
    output.position = mul(output.position, proj);

	output.normal= B;//input.binormal;
	output.texcoord=input.texcoord;
	
	float3x3 Tangent={input.binormal.xyz,input.tangent.xyz,input.normal.xyz};
	output.lightDirection=mul(Tangent,lightDirection.xyz);
	output.viewDirection= mul(Tangent,viewDirection.xyz);

	output.color = input.color;
	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	float2 parallax=input.viewDirection.xy * normalMap.Sample( textureSampler, input.texcoord).a*bias;
	
	//float4 D=textureMap.Sample( textureSampler, input.texcoord +parallax);
	//float4 N=normalMap.Sample( textureSampler, input.texcoord +parallax)*2.0f-1.0f;
	
	//return saturate(dot(N,input.lightDirection))*D+0.2F;
	return normalMap.Sample( textureSampler,input.color * input.texcoord).a*bias;//.a*bias;
}*/