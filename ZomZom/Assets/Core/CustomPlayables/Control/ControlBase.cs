using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ControlBase<T> : TweenableBase<T>
{
    [SerializeField] protected T m_Value;
    public UnityEvent<T> OnControlValueChanged;
    public virtual T value
    {
        get
        {
            return m_Value;
        }
        set
        {
            m_Value = value;
            OnControlValueChanged?.Invoke(value);
        }
    }
    public override Dictionary<int, string> TweenableMembers { get; } = new Dictionary<int, string>()
    {
        {0, "Value"}
    };
    public override void SetTweenableValue(int index, T value)
    {
        if (index == 0)
        {
            this.value = value;
        }
    }
    public override T GetTweenableValue(int index)
    {
        if (index == 0)
        {
            return this.value;
        }

        return default(T);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        OnControlValueChanged?.Invoke(value);
    }
#endif
}