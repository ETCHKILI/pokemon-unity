Shader "Hidden/Custom/DitherPostFX"
{
    HLSLINCLUDE 
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    #pragma shader_feature DITHER2X2 DITHER3X3 DITHER4X4 DITHER8X8

    half _Size;
    int _ColorSteps;
    float _CenterShift;
    float2 _Resolution;
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    float _ResolutionScale;
    
    float Dither(int x, int y, float f){
        float l = 0.0;

        #if DITHER2X2
            int sum = 4;
            int ditherMatrix[2][2] = {
                {0,2},
                {3,1}
            };
        #endif
    
        #if DITHER3X3
            int sum = 9;
            int ditherMatrix[3][3] = {
                {7,2,6},
                {4,0,1},
                {3,8,5}
            };
        #endif

        #if DITHER4X4
            int sum = 16;
            int ditherMatrix[4][4] = {
                { 0, 8, 2,10},
                {12, 4,14, 6},
                { 3,11, 1, 9},
                {15, 7,13, 5}
            };
        #endif

        #if DITHER8X8
            int sum = 64;
            int ditherMatrix[8][8] = {
                { 0, 32,  8, 40,  2, 34, 10, 42},
                {48, 16, 56, 24, 50, 18, 58, 26},
                {12, 44,  4, 36, 14, 46,  6, 38},
                {60, 28, 52, 20, 62, 30, 54, 22},
                { 3, 35, 11, 43,  1, 33,  9, 41},
                {51, 19, 59, 27, 49, 17, 57, 25},
                {15, 47,  7, 39, 13, 45,  5, 37},
                {63, 31, 55, 23, 61, 29, 53, 21} 
            };
        #endif

        float f0 = max( 0.0,floor(f*_ColorSteps)/_ColorSteps );
        float f1 = min( f0 + (float)1.0/_ColorSteps, 1.0 );
        float d = smoothstep( f0,f1, f );

        if(f >= 1.0){
            l = f1;
        }else if( f == 0){
            l = f0;
        }else{
            if( d < (float)ditherMatrix[x][y] / sum ){
                l = f0;
            }else{ 
                l = f1; 
            }
        }
        return l;
    }
    
    float ApplyShift( float f ){
        float r = f;
        float d = 1.0 - abs(0.5 - f)*2.0;
        r += d*_CenterShift;
        return r;
    }
    
    
    
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float2 pixelXY = floor(_Resolution * i.texcoord);
        float2 UV = floor( i.texcoord * _Resolution ) / _Resolution;
        #ifdef DITHER2X2
            int px = int( pixelXY.x %  2);
            int py = int( pixelXY.y %  2);
        #endif

        #ifdef DITHER3X3
            int px = int( pixelXY.x %  3);
            int py = int( pixelXY.y %  3);
        #endif

        #ifdef DITHER4X4
            int px = int( pixelXY.x %  4);
            int py = int( pixelXY.y %  4);
        #endif

        #ifdef DITHER8X8
            int px = int( pixelXY.x %  8);
            int py = int( pixelXY.y %  8);
        #endif

        float3 lum = float3(0.299, 0.587, 0.144);
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UV);
        float grayscale = dot( color.xyz, lum );
        return float4(
            Dither( px, py, ApplyShift( color.x ) ),
            Dither( px, py, ApplyShift( color.y ) ),
            Dither( px, py, ApplyShift( color.z ) ),
            color.a
        );
    }
    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}