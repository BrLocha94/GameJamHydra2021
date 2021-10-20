using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteRendererGroupMember), true)]
public class SpriteGroupMemberEditor : Editor
{
    SpriteRendererGroupMember inspectedSpriteMember;

    private void OnEnable()
    {
        inspectedSpriteMember = target as SpriteRendererGroupMember;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        inspectedSpriteMember.color = EditorGUILayout.ColorField(inspectedSpriteMember.color);
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("Force Update Member Parent", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        inspectedSpriteMember.forceUpdateMemberParent = EditorGUILayout.Toggle(inspectedSpriteMember.forceUpdateMemberParent);
        EditorGUI.indentLevel--;
    }
}
