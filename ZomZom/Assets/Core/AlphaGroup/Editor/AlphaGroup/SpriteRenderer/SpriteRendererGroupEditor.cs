using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(SpriteRendererGroup), true)]
public class SpriteRendererGroupEditor : Editor
{
    SpriteRendererGroup inspectedRendererGroup;
    private void OnEnable()
    {
        inspectedRendererGroup = target as SpriteRendererGroup;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Alpha", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        inspectedRendererGroup.groupAlpha = EditorGUILayout.Slider(inspectedRendererGroup.groupAlpha, 0f, 1f);
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Force Update Member Parent", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        inspectedRendererGroup.forceUpdateMemberParent = EditorGUILayout.Toggle(inspectedRendererGroup.forceUpdateMemberParent);
        EditorGUI.indentLevel--;
    }

}
