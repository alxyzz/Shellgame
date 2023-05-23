Shader "Custom/SH_CompactPBR"
{
    Properties
    {
        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MRATex ("MRA Map", 2D) = "gray" {}
        _NormalTex ("Normal Map", 2D) = "bump" {}
        _EmissiveTex ("Emissive Map", 2D) = "black" {}
        _EmissiveStrength ("Emissive Strength", Float) = 1
    }
    
    CustomEditor "CompactPBRGUI"
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MRATex;
        sampler2D _NormalTex;
        sampler2D _EmissiveTex;
        float _EmissiveStrength;

        #include "SHF_Common.cginc"

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float3 emission = tex2D(_EmissiveTex, IN.uv_MainTex);
            PBRData data = unpackPBR(_MainTex, _MRATex, _NormalTex, emission, IN.uv_MainTex);
            o.Albedo = data.albedo;
            // Metallic and smoothness come from slider variables
            o.Metallic = data.metallic;
            o.Smoothness = data.smoothness;
            o.Occlusion = data.ambientOcclusion;
            o.Normal = data.normal;
            o.Emission = data.emission * _EmissiveStrength;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
