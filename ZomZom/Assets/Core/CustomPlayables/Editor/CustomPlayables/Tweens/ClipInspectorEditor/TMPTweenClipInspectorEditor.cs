using UnityEditor;

[CustomEditor(typeof(TMPTweenClip))]
public class TMPTweenClipInspectorEditor : DelayedTweenClipInspectorEditor
{
    private SerializedProperty m_PositionParameter;
    private SerializedProperty m_RotationParameter;
    private SerializedProperty m_ScaleParameter;
    private SerializedProperty m_PivotOffsetParameter;
    private SerializedProperty m_GradientParameter;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawDelayedTweenBehaviour();
        DrawTMPTweenBehaviour();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTMPTweenBehaviour()
    {
        PlayableEditorCommons.DrawValueTweenParameter(m_PositionParameter, "Position");
        PlayableEditorCommons.DrawValueTweenParameter(m_RotationParameter, "Rotation");
        PlayableEditorCommons.DrawValueTweenParameter(m_ScaleParameter, "Scale");
        PlayableEditorCommons.DrawValueTweenParameter(m_PivotOffsetParameter, "Char Pivot");
        PlayableEditorCommons.DrawGradientValueTweenParameter(m_GradientParameter, "Char Color");
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref m_ScaleParameter, "m_ScaleParameter");
        GetSerializedReference(ref m_PositionParameter, "m_PositionParameter");
        GetSerializedReference(ref m_RotationParameter, "m_RotationParameter");
        GetSerializedReference(ref m_PivotOffsetParameter, "m_PivotOffsetParameter");
        GetSerializedReference(ref m_GradientParameter, "m_GradientParameter");
    }
}






