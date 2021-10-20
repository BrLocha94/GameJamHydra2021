using UnityEditor;

[CustomEditor(typeof(RenderSettingsClip))]
public class RenderSettingsTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty skyColorGradientTweenParameter;
    private SerializedProperty equatorColorGradientTweenParameter;
    private SerializedProperty groundColorGradientTweenParameter;

    private SerializedProperty ambientColorGradientTweenParameter;
    private SerializedProperty ambientIntensity;
    private SerializedProperty reflectionIntensity;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawRenderSettingsBehaviour();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    public virtual void DrawRenderSettingsBehaviour()
    {
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawGradientValueTweenParameter(skyColorGradientTweenParameter, "Ambient Sky");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawGradientValueTweenParameter(equatorColorGradientTweenParameter, "Ambient Equator");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawGradientValueTweenParameter(groundColorGradientTweenParameter, "Ambient Ground");

        EditorGUILayout.Space();
        PlayableEditorCommons.DrawGradientValueTweenParameter(ambientColorGradientTweenParameter, "Ambient Color");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawValueTweenParameter(ambientIntensity, "Ambient Intensity");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawValueTweenParameter(reflectionIntensity, "Reflection Intensity");
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref skyColorGradientTweenParameter, "skyColorGradientTweenParameter");
        GetSerializedReference(ref equatorColorGradientTweenParameter, "equatorColorGradientTweenParameter");
        GetSerializedReference(ref groundColorGradientTweenParameter, "groundColorGradientTweenParameter");

        GetSerializedReference(ref ambientColorGradientTweenParameter, "ambientColorGradientTweenParameter");
        GetSerializedReference(ref ambientIntensity, "ambientIntensity");
        GetSerializedReference(ref reflectionIntensity, "reflectionIntensity");
    }

}








