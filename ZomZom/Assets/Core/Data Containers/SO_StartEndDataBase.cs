using UnityEngine;

public abstract class SO_StartEndDataBase<T> : ScriptableObject
{
    [TextArea]
    [SerializeField] private string assetDescription;

    [SerializeField] protected T m_Start;
    [SerializeField] protected T m_End;
    public T start {get=>m_Start;set=>m_Start=value;}
    public T end {get=>m_End;set=>m_End=value;}
}