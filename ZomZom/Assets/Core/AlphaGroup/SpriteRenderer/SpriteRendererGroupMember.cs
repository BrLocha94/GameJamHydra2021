using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererGroupMember : AlphaMemberBase<SpriteRenderer>
{
    private SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer spriteRenderer => m_SpriteRenderer;

    private Color m_MemberColor = new Color(1, 1, 1, 1);

    protected override void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        if (m_SpriteRenderer == null)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        base.OnEnable();
    }

    /// <summary>
    /// Sprite renderer color
    /// </summary>
    public Color color
    {
        get => m_MemberColor;
        set
        {
            m_MemberColor = value;
            float finalAlpha = m_MemberColor.a * CalculatedAlpha();
            m_SpriteRenderer.forceRenderingOff = Mathf.Approximately(0.0f, finalAlpha);
            m_SpriteRenderer.color = m_SpriteRenderer.color = new Color(m_MemberColor.r, m_MemberColor.g, m_MemberColor.b, finalAlpha);
        }
    }

    /// <summary>
    /// Sprite renderer alpha
    /// </summary>
    public float alpha
    {
        get => m_MemberColor.a;
        set
        {
            m_MemberColor.a = value;
            color = m_MemberColor;
        }
    }

    public override void OnAlphaChange()
    {
        if (m_SpriteRenderer == null) { m_SpriteRenderer = GetComponent<SpriteRenderer>(); }
        float finalAlpha = m_MemberColor.a * CalculatedAlpha();
        m_SpriteRenderer.forceRenderingOff = Mathf.Approximately(0.0f, finalAlpha);
        m_SpriteRenderer.color = m_SpriteRenderer.color = new Color(m_MemberColor.r, m_MemberColor.g, m_MemberColor.b, finalAlpha);
        base.OnAlphaChange();
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (m_SpriteRenderer == null)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
#endif
}
