
#ifndef SHF_FILTERS
#define SHF_FILTERS

float3 _sharpenCommon(sampler2D tex, float2 uv, float3 blurred, float sharpness)
{
    const float3 base = tex2D(tex, uv).rgb;
    const float scaler = 10; //Use scalar to put the rang eof the sharpness value mostly between 0-1 for convenience
    return base + (base - blurred) * sharpness * scaler;
}

float3 multiSample(sampler2D tex, float2 uv, int samples)
{
    float3 result = float3(0,0,0);
    for (int i = 0; i < samples; i++)
    {
        result += tex2D(tex, uv + float2(0.0, -.01) * i);
    }
    return result / samples;
}

float4 boxBlur(sampler2D tex, float2 uv, float2 texelSize, float radius, bool circular)
{
    if(radius <= 1.0)
    {
        return tex2D(tex, uv);
    }
    float4 result = float4(0,0,0,0);
    float totalWeight = 0.0;

    for (float x = -radius; x <= radius; x++)
    {
        for (float y = -radius; y <= radius; y++)
        {
            const float2 offset = float2(x, y);
            if(length(offset) <= radius || !circular)
            {
                result += tex2D(tex, uv + (offset * texelSize));
                totalWeight += 1.0;
            }
        }
    }
    return result / totalWeight;
}

float3 sharpenBox(sampler2D tex, float2 uv, float2 texelSize, float sharpness = 1.0, float radius = 2.0, bool circular = true)
{
    float4 blurred = boxBlur(tex, uv, texelSize, radius, circular);
    return _sharpenCommon(tex, uv, blurred.rgb, sharpness);
}

float2 sobelFilter(sampler2D tex, float2 texelSize, float2 uv)
{
    const float3 sobelKernel[] = {
        float3(-1, -1, -1), float3(+1, -1, +1),
        float3(-1, +0, -2), float3(+1, +0, +2),
        float3(-1, +1, -1), float3(+1, -1, +1)
    };

    float2 result = float2(0,0);
    for (int i = 0; i < 6; i++)
    {
        float3 s = sobelKernel[i];
        float sample1 = tex2D(tex, uv + texelSize * s.xy).a * s.z;
        float sample2 = tex2D(tex, uv + texelSize * s.yx).a * s.z;
        result.x += sample1;
        result.y += sample2;
    }
    return result / 4.0;
}

float pixelOutline(sampler2D tex, float2 texelSize, float2 uv)
{
    float result = 0;
    result += tex2D(tex, uv + texelSize * float2(-1, 0)).a;
    result += tex2D(tex, uv + texelSize * float2(+1, 0)).a;
    result += tex2D(tex, uv + texelSize * float2(0, -1)).a;
    result += tex2D(tex, uv + texelSize * float2(0, +1)).a;
    return result;
}

float3 structureTensor(sampler2D tex, float2 texelSize, float2 uv)
{
    const float3 sobelKernel[] = {
        float3(-1, -1, -1), float3(+1, -1, +1),
        float3(-1, +0, -2), float3(+1, +0, +2),
        float3(-1, +1, -1), float3(+1, -1, +1)
    };

    float3 u = float3(0,0,0);
    float3 v = float3(0,0,0);
    for (int i = 0; i < 6; i++)
    {
        float3 k = sobelKernel[i];
        u += tex2D(tex, uv + texelSize * k.xy).xyz * k.z;
        v += tex2D(tex, uv + texelSize * k.yx).xyz * k.z;
    }
    u /= 4;
    v /= 4;

    return float3(
        dot(u, v),
        dot(v, v),
        dot(u, v)
    );
}

float3 flowFromStructure(float3 s)
{
    float l = 0.5 * (s.y + s.x +sqrt(s.y*s.y - 2.0*s.x*s.y + s.x*s.x + 4.0*s.z*s.z));
    float2 d = float2(s.x - l, s.z);
    return (length(d) > 0.0)? float3(normalize(d), sqrt(l)) : float3(0,1,0);
}

#endif