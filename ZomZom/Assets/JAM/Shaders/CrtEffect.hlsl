#ifndef CRTEFFECT_INCLUDED
#define CRTEFFECT_INCLUDED

#define CRT_STRENGTH 0.8
#define CA_MAX_PIXEL_DIST 5.0
#define BORDER_SIZE 0.2
#define BORDER_STRENGTH 0.8
#define SATURATION 1.4

void CRTEFFECT_float(float2 fragCoord, float2 PIXELSIZE ,out float3 PIX)
{
    // Default lcd colour (affects brightness)
    float pb = 0.0;
    float3 lcdColor = float3(pb, pb, pb);
    float tres = 3.0f;
    // Change every 1st, 2nd, and 3rd vertical strip to RGB respectively
    
    int px = round(fmod(fragCoord.x * PIXELSIZE.x, tres));
    if (px == 1)
        lcdColor.x = 1.0f;
    else if (px == 2)
        lcdColor.y = 1.0f;
    else
        lcdColor.z = 1.0f;
    
    // Darken every 3rd horizontal strip for scanline
    float sclV = 0.0f;
    if (round(fmod(fragCoord.y * PIXELSIZE.y, tres)) == 0)
        lcdColor = float3(sclV, sclV, sclV);
    
   // PIX = px;
    PIX =  lcdColor;

}

float3 saturation_float(float3 rgb, float amount)
{
    const float3 W = float3(0.2125f, 0.7154f, 0.0721f);
    float3 intensity = float3(dot(rgb, W));
    return lerp(intensity, rgb, amount);
}


#endif