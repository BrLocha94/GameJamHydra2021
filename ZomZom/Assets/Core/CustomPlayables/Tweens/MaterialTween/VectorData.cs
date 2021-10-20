using UnityEngine;

public class VectorData
{
    public enum EVectorType {Vector2, Vector3,Vector4}
    public EVectorType type = EVectorType.Vector4;
    public int propertyID;
    public Vector4 vector;
    public Vector4 defaultVector;
    public void Add(Vector4 v4)
    {
        vector += v4;
    }
    public void Initialize(Vector4 v4)
    {
        defaultVector = v4;
    }
}
