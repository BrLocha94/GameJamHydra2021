using UnityEngine;

public class ColorData
{
    public int propertyID;
    public Vector4 color;
    public Vector4 defaultColor;
    public void Add(Vector4 c)
    {
        color += c;
    }
    public void Initialize(Vector4 c)
    {
        defaultColor = c;
    }
}
