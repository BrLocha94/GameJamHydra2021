using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(SpriteRendererGroup), true)]
public class SpriteRendererGroupEditor : Editor
{
    SpriteRendererGroup inspectedRendererGroup;
    SerializedProperty m_GroupAlpha;
    private void OnEnable()
    {
        inspectedRendererGroup = target as SpriteRendererGroup;
        m_GroupAlpha = serializedObject.FindProperty("m_GroupAlpha"); 
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Alpha", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_GroupAlpha);
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Force Update Member Parent", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        inspectedRendererGroup.forceUpdateMemberParent = EditorGUILayout.Toggle(inspectedRendererGroup.forceUpdateMemberParent);
        EditorGUI.indentLevel--;
        if(serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
            inspectedRendererGroup.OnAlphaChange();
        }
    }

}
