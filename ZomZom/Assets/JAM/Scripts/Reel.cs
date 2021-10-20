using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class Reel : MonoBehaviour
{
    [SerializeField] protected List<Slot> m_Slots = new List<Slot>();
    [SerializeField] protected float m_SlotSpacing = 0;
    [SerializeField] protected float m_SlotWidth = 1;
    [SerializeField] protected float m_SlotHeight = 1;
    [SerializeField] protected Bounds m_Bouds = new Bounds(Vector3.zero, new Vector3(1.58f, 5));

    [SerializeField] protected float m_Scroll = 0;
    [SerializeField] protected float m_Offset = 0;
    public float scroll
    {
        get => m_Scroll;
        set
        {
            m_Scroll = value;
            Relayout();
        }
    }
    public float offset
    {
        get => m_Offset;
        set
        {
            m_Offset = value;
            Relayout();
        }
    }

    public List<Slot> slots => m_Slots;
    public Bounds bounds => m_Bouds;

    protected virtual void Awake()
    {
        GetSlotsInHierarch();
        Relayout();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    private void OnTransformChildrenChanged()
    {
        GetSlotsInHierarch();
        Relayout();
    }
    private void OnValidate()
    {
        OnTransformChildrenChanged();
    }

    public Slot GetSlot(int index)
    {
        if (index >= 0 && index < m_Slots.Count) return m_Slots[index];
        return null;
    }

    public virtual void GetSlotsInHierarch()
    {
        var slots = GetComponentsInChildren<Slot>();

        if (slots != null && slots.Length > 0)
        {
            m_Slots = new List<Slot>(slots);

            if (m_Slots.Count < 3) return;

            for (int i = 0; i < m_Slots.Count; i++)
            {
                var slot = m_Slots[i];
                slot.witdh = m_SlotWidth;
                slot.height = m_SlotHeight;
                slot.reel = this;
            }
        }
    }
    public virtual void Relayout()
    {

        float firstSlotExtents = m_Slots[0].bounds.extents.y;
        float firstSlotHeight = m_Slots[0].bounds.size.y;
        float slotsHeight = m_Slots.Count * m_SlotHeight;
        float offset = (m_Offset * m_SlotHeight) * -1;

        for (int i = 0; i < m_Slots.Count; i++)
        {
            Slot slot = m_Slots[i];
            float scrollValue = Mathf.Repeat(m_Scroll, 1) * -1;
            var ypos = (i * (slot.height + m_SlotSpacing)) + (scrollValue * slotsHeight) + firstSlotHeight;
            var yrepeated = scrollValue < 0 ? Mathf.Repeat(ypos, slotsHeight) : ypos;
            slot.transform.localPosition = new Vector3(0, offset + (yrepeated - (bounds.extents.y + firstSlotExtents)), 0);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        ReelEditor.DrawGUI(this, Color.white);
    }
    private void OnDrawGizmosSelected()
    {
        ReelEditor.DrawGUI(this, Color.red);
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Reel))]
public class ReelEditor : Editor
{
    Reel m_Reel;
    private void OnEnable()
    {
        m_Reel = target as Reel;
        m_Reel.GetSlotsInHierarch();
    }

    public static void DrawGUI(Reel reel, Color color)
    {
        Vector3 center = reel.transform.position + reel.bounds.center;
        var lastColor = Handles.color;
        Handles.color = color;
        Handles.DrawWireCube(center, reel.bounds.size);
        Handles.color = lastColor;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Reel.Relayout();
    }
}
#endif
