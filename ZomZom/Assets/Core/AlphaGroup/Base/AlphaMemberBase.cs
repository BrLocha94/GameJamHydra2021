using UnityEngine;
using System;

[ExecuteInEditMode]
public abstract class AlphaMemberBase<T> : MonoBehaviour
{
    private const int maxUpwardsParentSearchCount = 12;
    private Transform m_Parent;
    protected AlphaMemberBase<T> nearParent;
    protected float m_Alpha = 1;

    public event Action<float> OnAlphaChanged;

    public bool forceUpdateMemberParent = true;

    /// <summary>
    /// Alpha parameter driven by the group alpha
    /// </summary>
    public virtual float memberAlpha
    {
        get => m_Alpha;
        set
        {
            m_Alpha = Mathf.Clamp01(value);
            OnAlphaChange();
        }
    }
    protected virtual void OnAlphaChange()
    {
        OnAlphaChanged?.Invoke(CalculatedAlpha());
    }
    protected void OnParentAlphaChanged(float parentAlpha)
    {
        memberAlpha = parentAlpha;
    }
    protected virtual float CalculatedAlpha()
    {
        return m_Alpha;
    }
    protected virtual void Awake()
    {
        m_Parent = transform.parent;
        if (nearParent==null) UpdateMemberParent();
    }
    protected virtual void OnEnable()
    {
        m_Parent = transform.parent;
        if (forceUpdateMemberParent) UpdateMemberParent();
        else if (nearParent==null) UpdateMemberParent();
    }
    protected virtual void OnDestroy()
    {
        if (nearParent != null)
        {
            nearParent.OnAlphaChanged -= OnParentAlphaChanged;
        }
    }
    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (transform.hasChanged && (forceUpdateMemberParent && Application.isPlaying))
        {
            if (m_Parent != null && m_Parent != transform.parent)
            {
                m_Parent = transform.parent;
                UpdateMemberParent();
            }

            transform.hasChanged = false;
        }
#else

        if (transform.hasChanged && forceUpdateMemberParent)
        {
            if (m_Parent != null && m_Parent != transform.parent)
            {
                m_Parent = transform.parent;
                UpdateMemberParent();
            }

            transform.hasChanged = false;
        }

#endif
    }
    public void UpdateMemberParent()
    {

        if (nearParent != null)
        {
            nearParent.OnAlphaChanged -= OnParentAlphaChanged;
        }

        Transform tParent = transform;


        for (int i = 0; i < maxUpwardsParentSearchCount; i++)
        {
            tParent = tParent.parent;

            if (tParent != null)
            {
                nearParent = tParent.GetComponent<AlphaMemberBase<T>>();
                if (nearParent != null)
                {
                    nearParent.OnAlphaChanged += OnParentAlphaChanged;
                    memberAlpha = nearParent.CalculatedAlpha();
                    break;
                }
            }
            else
            {
                memberAlpha = 1;
                break;
            }
        }
    }
}
