// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



uniform float4 _SceenAndTex;

struct vData 
{
    float4 vertex : POSITION;
    half2 texcoord : TEXCOORD0;
};

struct v2f 
{
	float4 pos : SV_POSITION;
	half2 uv : TEXCOORD0;
	half2 uvd : TEXCOORD1;
};

v2f vert(vData v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.texcoord;
	o.uvd = v.texcoord * _SceenAndTex.xy;
	return o;
}
