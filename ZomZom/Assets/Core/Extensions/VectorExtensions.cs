using System;
using UnityEngine;
public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 value)
    {
        return new Vector3(value.x, value.y);
    }
    public static Quaternion ToQuaternion(this Vector4 value)
    {
        return new Quaternion(value.x,value.y,value.z, value.w);
    }
    public static Vector4 ClampMagnitude(this Vector4 vector, float maxLength)
    {
        float sqrmag = vector.sqrMagnitude;
        if (sqrmag > maxLength * maxLength)
        {
            float mag = (float)Math.Sqrt(sqrmag);
            //these intermediate variables force the intermediate result to be
            //of float precision. without this, the intermediate result can be of higher
            //precision, which changes behavior.
            float normalized_x = vector.x / mag;
            float normalized_y = vector.y / mag;
            float normalized_z = vector.z / mag;
            float normalized_w = vector.w / mag;
            return new Vector4(normalized_x * maxLength,
                normalized_y * maxLength,
                normalized_z * maxLength,
                normalized_w * maxLength);
        }
        return vector;
    }
}
