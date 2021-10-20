#ifndef MOTIONBLUR_INCLUDED
#define MOTIONBLUR_INCLUDED

void MotionBlur_float(UnityTexture2D Tex ,UnitySamplerState SS, float Amount,float2 UV, out float4 OUT) 
{
    OUT = float4(0,0,0,0);

    float samples = 10;

    for(float i = 0; i < samples; i++)
    {
        OUT += SAMPLE_TEXTURE2D(Tex, SS, UV+float2(0,i*0.001f*Amount));
    }
    
    OUT/=samples * 2;
}

#endif