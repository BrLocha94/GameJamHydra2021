using UnityEditor;
/*[CustomEditor(typeof(LineRendererTweenClip))]
public class LineRendererTweenClipInspectorEditor : DelayedTweenClipInspectorEditor
{
    private SerializedProperty m_BezierTweenParamter;
    private SerializedProperty m_LineLenght;
    private SerializedProperty m_StartColorTweenParamter;
    private SerializedProperty m_EndColorTweenParamter;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase(); 
        DrawCallbackTweenBehavior();
        EditorGUI.BeginChangeCheck();
        DrawDelayedTweenBehaviour();
        DrawLineRendererTweenBehaviour();
        if(EditorGUI.EndChangeCheck())
        {
            (tweenClip.template as LineRendererTweenBehaviour).OnTweenParameterDataChanged();
        }
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawLineRendererTweenBehaviour()
    {
        EditorGUILayout.PropertyField(m_LineLenght);
        
        PlayableEditorCommons.DrawValueTweenParameter(m_StartColorTweenParamter,"Start Color");
        PlayableEditorCommons.DrawValueTweenParameter(m_EndColorTweenParamter,"End Color");
        PlayableEditorCommons.DrawBezierValueTweenParameter(m_BezierTweenParamter, "Bezier");
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref m_BezierTweenParamter, "m_BezierTweenParamter");
        GetSerializedReference(ref m_StartColorTweenParamter, "m_StartColorTweenParamter");
        GetSerializedReference(ref m_EndColorTweenParamter, "m_EndColorTweenParamter");
        GetSerializedReference(ref m_LineLenght, "m_LineLenght");
    }
}*/


[CustomEditor(typeof(LightTweenClip))]
public class LightTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty m_IntensityTweenParameter;
    private SerializedProperty m_ColorTweenParameter;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawLightTweenBehaviour();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawLightTweenBehaviour()
    {
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawValueTweenParameter(m_IntensityTweenParameter, "Intensity");
        PlayableEditorCommons.DrawGradientValueTweenParameter(m_ColorTweenParameter, "Color");
        EditorGUILayout.Space();
    }
    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref m_IntensityTweenParameter, "m_IntensityTweenParameter");
        GetSerializedReference(ref m_ColorTweenParameter, "m_ColorTweenParameter");
    }
}








