using UnityEditor;

public class TweenClipStartEndInspectorBaseEditor : TweenClipInspectorBaseEditor
{
    protected SerializedProperty startEndValue;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawTweenClipStartEnd();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTweenClipStartEnd()
    {
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawValueTweenParameter(startEndValue, "Value");
        EditorGUILayout.Space();
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref startEndValue, "startEndValueTweenParameter");
    }
}

