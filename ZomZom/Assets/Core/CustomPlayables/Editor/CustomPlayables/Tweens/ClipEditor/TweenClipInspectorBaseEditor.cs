using UnityEditor;

public class TweenClipInspectorBaseEditor : CallbackTweenClipInspectorBaseEditor
{
    private SerializedProperty easeStyle;
    private SerializedProperty useCurve;
    private SerializedProperty customCurve;
    private SerializedProperty useCurveAsset;
    private SerializedProperty animationCurveAsset;
    private SerializedProperty startTime;
    private SerializedProperty endTime;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTweenClipBase()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.PropertyField(startTime);
            EditorGUILayout.PropertyField(endTime);
        }
        EditorGUILayout.PropertyField(useCurve);

        if (useCurve.boolValue)
        {
            EditorGUILayout.PropertyField(useCurveAsset);
            if (useCurveAsset.boolValue)
                EditorGUILayout.PropertyField(animationCurveAsset);
            else
                EditorGUILayout.PropertyField(customCurve);
        }
        else
        {
            EditorGUILayout.PropertyField(easeStyle);
        }
        EditorGUILayout.EndVertical();
    }
    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref easeStyle, "easeStyle");
        GetSerializedReference(ref useCurve, "useCustomCurve");
        GetSerializedReference(ref customCurve, "customCurve");
        GetSerializedReference(ref useCurveAsset, "useCurveAsset");
        GetSerializedReference(ref animationCurveAsset, "animationCurveAsset");
        GetSerializedReference(ref startTime, "startTime");
        GetSerializedReference(ref endTime, "endTime");
    }
}

