using UnityEditor;

[CustomEditor(typeof(CameraMatrixTweenClip))]
public class CameraShakeTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty globalFrequency;
    private SerializedProperty globalAmplitude;

    private SerializedProperty position;
    private SerializedProperty rotation;
    private SerializedProperty scale;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        EditorGUILayout.PropertyField(globalFrequency);
        EditorGUILayout.PropertyField(globalAmplitude);

        EditorGUILayout.Space();
        PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(position, "Position");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(rotation, "Rotation");
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(scale, "Scale");
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }
    protected override void GetReferences()
    {
        base.GetReferences();

        GetSerializedReference(ref globalFrequency, "globalFrequency");
        GetSerializedReference(ref globalAmplitude, "globalAmplitude");

        GetSerializedReference(ref position, "position");
        GetSerializedReference(ref rotation, "rotation");
        GetSerializedReference(ref scale, "scale");
    }
}
[CustomEditor(typeof(CameraMatrix2TweenClip))]
public class CameraMatrix2TweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty position;
    private SerializedProperty rotation;
    private SerializedProperty scale;

    private SerializedProperty projectionPosition;
    private SerializedProperty projectionRotation;
    private SerializedProperty fieldOfView;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Transformation Matrix");
            PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(position, "Position");
            EditorGUILayout.Space();
            PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(rotation, "Rotation");
            EditorGUILayout.Space();
            PlayableEditorCommons.DrawMultipleChannelValueTweenParameter(scale, "Scale");
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Projection Matrix");
            PlayableEditorCommons.DrawValueTweenParameter(projectionPosition, "Obliqueness");
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();

        PlayableEditorCommons.DrawValueTweenParameter(fieldOfView, "Field of View");

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }
    protected override void GetReferences()
    {
        base.GetReferences();

        GetSerializedReference(ref projectionPosition, "projectionPosition");
        GetSerializedReference(ref projectionRotation, "projectionRotation");
        GetSerializedReference(ref fieldOfView, "fieldOfView");

        GetSerializedReference(ref position, "position");
        GetSerializedReference(ref rotation, "rotation");
        GetSerializedReference(ref scale, "scale");
    }
}






