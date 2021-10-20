using UnityEngine;

public abstract class SO_DataBase<T> : ScriptableObject
{
    [TextArea]
    [SerializeField] private string assetDescription;

    [SerializeField] protected T data;
    public T Data => data;
}
