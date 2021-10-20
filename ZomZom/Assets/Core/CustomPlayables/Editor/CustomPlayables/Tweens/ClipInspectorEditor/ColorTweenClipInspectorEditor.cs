using UnityEditor;

[CustomEditor(typeof(ColorTweenClip))]
public class ColorTweenClipInspectorEditor : TweenClipStartEndInspectorBaseEditor
{
    protected override void DrawTweenClipStartEnd()
    {
        EditorGUILayout.Space();
        PlayableEditorCommons.DrawGradientValueTweenParameter(startEndValue, "Value");
        EditorGUILayout.Space();
    }
}
