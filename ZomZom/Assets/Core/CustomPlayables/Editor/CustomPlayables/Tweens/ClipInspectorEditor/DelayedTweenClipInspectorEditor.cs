using UnityEditor;

public class DelayedTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty delay;
    private SerializedProperty m_ReverseOrder;
    private SerializedProperty m_Backwards;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawDelayedTweenBehaviour();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawDelayedTweenBehaviour()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.PropertyField(delay);
            EditorGUILayout.PropertyField(m_ReverseOrder);
            EditorGUILayout.PropertyField(m_Backwards);
        }
        EditorGUILayout.EndVertical();
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref delay, "delay");
        GetSerializedReference(ref m_ReverseOrder, "m_ReverseOrder");
        GetSerializedReference(ref m_Backwards, "m_Backwards");
    }
}








