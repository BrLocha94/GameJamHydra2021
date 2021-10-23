#ifndef CRTEFFECT_INCLUDED
#define CRTEFFECT_INCLUDED

void CRTEFFECT_float(float2 UV,float resolution,float RGB_MASK_SIZE, out float4 OUT)
{
    OUT = float4(0, 0, 0, 0);
    float pix = (UV.y) * resolution + UV.x;
    pix = floor(pix);
    OUT = float4(modf(pix, RGB_MASK_SIZE), modf((pix + 1.0), RGB_MASK_SIZE), modf((pix + 2.0), RGB_MASK_SIZE), 1.0);
    OUT = OUT / (RGB_MASK_SIZE - 1.0) + 0.5;
}

   /* float mod(float x, float y)
    {
        return x - y * floor(x / y);
    }*/
#endif