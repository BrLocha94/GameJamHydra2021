using UnityEngine;

public abstract class AlphaGroupBase<T> : AlphaMemberBase<T>
{
    protected float m_GroupAlpha = 1;
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
}
