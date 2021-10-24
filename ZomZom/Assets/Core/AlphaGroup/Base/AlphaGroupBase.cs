using System.Collections.Generic;
using UnityEngine;

public abstract class AlphaGroupBase<T> : AlphaMemberBase<T>
{
    [Range(0f,1f)]
    [SerializeField] protected float m_GroupAlpha = 1;
    public virtual float groupAlpha
    {
        get => m_GroupAlpha;
        set
        {
            m_GroupAlpha = Mathf.Clamp01(value);
            OnAlphaChange();
        }
    }
    protected override float CalculatedAlpha()
    {
        return base.CalculatedAlpha() * m_GroupAlpha;
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (AlphaMemberBase<T> child in GetComponentsInChildren<AlphaMemberBase<T>>())
        {
            if (!child.HasParent) child.UpdateMemberParent();
        }
    }
    public override Dictionary<int, string> TweenableMembers { get; } = new Dictionary<int, string>()
    {
        {0, "Group Alpha"}
    };
    public override void SetTweenableValue(int index, float value)
    {
        if (index == 0)
        {
            groupAlpha = value;
        }
    }
    public override float GetTweenableValue(int index)
    {
        if (index == 0)
        {
            return groupAlpha;
        }
        return 0;
    }
}
