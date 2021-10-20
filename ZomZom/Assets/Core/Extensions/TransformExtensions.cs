using UnityEngine;

public static class TransformExtensions
{
    public static void CopyFrom(this Transform transform, Transform source, bool local = false)
    {
        if(local)
        {
            transform.localPosition = source.localPosition;
            transform.localRotation = source.localRotation;
            transform.localScale = source.localScale;
        }
        else
        {
            transform.position = source.position;
            transform.rotation = source.rotation;
        }
    }
}
