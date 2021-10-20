using UnityEditor;

public abstract class TimelineClipInspectorEditorBase : Editor
{
    protected SerializedProperty template;

    protected virtual void OnEnable()
    {
        GetReferences();
    }
    protected virtual void GetReferences()
    {
        template = serializedObject.FindProperty("template");
    }
    protected void GetSerializedReference(ref SerializedProperty property, string path)
    {
        property = template.FindPropertyRelative(path);
    }
}