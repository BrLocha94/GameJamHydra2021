#ifndef CRTEFFECT_INCLUDED
#define CRTEFFECT_INCLUDED

void CRTEFFECT_float(float2 UV, float PIXELSIZE, out float3 PIX)
{
    //float2 cor;
    //PIX
	
   /* cor.x = UV.x / PIXELSIZE;
    cor.y = (UV.y + PIXELSIZE * 1.5 * modf(floor(cor.x), 2.0)) / (PIXELSIZE * 3.0);
	
    float2 ico = floor(cor);
    float2 fco = frac(cor);

    PIX = step(1.5, modf(float3(0.0, 1.0, 2.0) + ico.x, 3.0));*/
    
   // float3 ima = SAMPLE_TEXTURE2D (Tex, SS, PIXELSIZE * ico * float2(1.0, 3.0));
	
    /*float3 col = pix * dot(pix, ima);

    col *= step(abs(fco.x - 0.5), 0.4);
    col *= step(abs(fco.y - 0.5), 0.4);
	
    col *= 1.2;
    OUT = float4(col, 1.0);*/
}

#endif