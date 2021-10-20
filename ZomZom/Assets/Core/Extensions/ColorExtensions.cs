using UnityEngine;

public static class ColorExtensions
{
    public static Color32 ToColor32(this Color color)
    {
        return new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
    }
    public static void AddClamped(this ref Color target, Color color)
    {
        target.r = Mathf.Clamp01(target.r + color.r);
        target.g = Mathf.Clamp01(target.g + color.g);
        target.b = Mathf.Clamp01(target.b + color.b);
        target.a = Mathf.Clamp01(target.a + color.a);
    }
}
