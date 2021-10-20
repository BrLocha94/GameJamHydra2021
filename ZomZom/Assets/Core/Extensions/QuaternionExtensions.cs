using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class QuaternionExtensions
{
    public static Quaternion AddQuaternions(Quaternion first, Quaternion second)
    {
        first.w += second.w;
        first.x += second.x;
        first.y += second.y;
        first.z += second.z;
        return first;
    }

    public static Quaternion FromVector4(Vector4 value)
    {
        return new Quaternion(value.x, value.y, value.z, value.w);
    }
    public static void FromVector4(this ref Quaternion rotation,Vector4 value)
    {
        rotation = new Quaternion(value.x, value.y, value.z, value.w);
    }
    public static Vector4 ToVector4(this Quaternion rotation)
    {
        return new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);
    }

    public static Quaternion ScaleQuaternion(Quaternion rotation, float multiplier)
    {
        rotation.w *= multiplier;
        rotation.x *= multiplier;
        rotation.y *= multiplier;
        rotation.z *= multiplier;
        return rotation;
    }
    public static void ScaleQuaternion(this ref Quaternion rotation, float multiplier)
    {
        rotation = ScaleQuaternion(rotation, multiplier);
    }

    public static float QuaternionMagnitude(Quaternion rotation)
    {
        return Mathf.Sqrt((Quaternion.Dot(rotation, rotation)));
    }
    public static float QuaternionMagnitude(this ref Quaternion rotation)
    {
        return QuaternionMagnitude(rotation);
    }

    public static Quaternion NormalizeQuaternion(Quaternion rotation)
    {
        float magnitude = QuaternionMagnitude(rotation);

        if (magnitude > 0f)
            return ScaleQuaternion(rotation, 1f / magnitude);

        return Quaternion.identity;
    }
    public static void NormalizeQuaternion(this ref Quaternion rotation)
    {
        rotation = NormalizeQuaternion(rotation);
    }

    public static bool IsQuaternionInvalid(Quaternion rotation)
    {
        bool check = rotation.x == 0f;
        check &= rotation.y == 0;
        check &= rotation.z == 0;
        check &= rotation.w == 0;

        return check;
    }
    public static void ValidateQuaternion(ref Quaternion rotation)
    {
        if(IsQuaternionInvalid(rotation))rotation = Quaternion.identity;
    }
}

