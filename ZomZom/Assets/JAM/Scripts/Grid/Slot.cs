using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private Bounds m_bounds = new Bounds(Vector3.zero, new Vector3(1f, 1f));
    [field:SerializeField] public SlotSymbol SymbolSlot{get; private set;} 

    public float witdh { get => m_bounds.size.x; set => m_bounds.size = new Vector3(value, m_bounds.size.y);}
    public float height { get => m_bounds.size.y; set => m_bounds.size = new Vector3(m_bounds.size.x,value);}

    public Bounds bounds => m_bounds;

    public int index {get;set;} = -1;
    public Reel reel {get;set;}


    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        SlotEditor.DrawGUI(this, Color.grey);
    }
    private void OnDrawGizmosSelected()
    {
        SlotEditor.DrawGUI(this, Color.green);
    }
    #endif


}
#if UNITY_EDITOR
[CustomEditor(typeof(Slot))]
public class SlotEditor : Editor
{
    Slot m_Slot;
    private void OnEnable()
    {
       m_Slot = target as Slot;
    }
    public static void DrawGUI(Slot slot, Color color)
    {
        Vector3 center = slot.transform.position + slot.bounds.center;
        var lastColor = Handles.color;
        Handles.color = color;
        Handles.DrawWireCube(center, slot.bounds.size);
        Handles.color = lastColor;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Slot.reel?.Relayout();
    }
}

#endif
