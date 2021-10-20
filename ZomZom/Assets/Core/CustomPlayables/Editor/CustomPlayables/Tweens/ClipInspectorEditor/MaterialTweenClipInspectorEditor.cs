using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialTweenClip))]
public class MaterialTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty m_FloatParameters;
    private SerializedProperty m_ColorParameters;
    private SerializedProperty m_VectorParameters;
    MaterialTweenClip targetClip;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetClip = target as MaterialTweenClip;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();
        DrawMaterialTweenBehaviour();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawMaterialTweenBehaviour()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Float");

            if (GUILayout.Button("Add"))
            {
                targetClip.template.AddFloatParameter();
            }

            for (int i = 0; i < m_FloatParameters.arraySize; i++)
            {
                PlayableEditorCommons.DrawMaterialTweenParameter(m_FloatParameters.GetArrayElementAtIndex(i));

                if (GUILayout.Button("Remove"))
                {
                    targetClip.template.RemoveFloatParameter(i);
                }
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Color");

            if (GUILayout.Button("Add"))
            {
                targetClip.template.AddColorParameter();
            }

            for (int i = 0; i < m_ColorParameters.arraySize; i++)
            {
                PlayableEditorCommons.DrawMaterialTweenParameter(m_ColorParameters.GetArrayElementAtIndex(i));

                if (GUILayout.Button("Remove"))
                {
                    targetClip.template.RemoveColorParameter(i);
                }
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Vector");

            if (GUILayout.Button("Add"))
            {
                targetClip.template.AddVectorParameter();
            }

            for (int i = 0; i < m_VectorParameters.arraySize; i++)
            {
                PlayableEditorCommons.DrawMaterialTweenParameter(m_VectorParameters.GetArrayElementAtIndex(i));

                if (GUILayout.Button("Remove"))
                {
                    targetClip.template.RemoveVectorParameter(i);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref m_FloatParameters, "m_FloatParameters");
        GetSerializedReference(ref m_ColorParameters, "m_ColorParameters");
        GetSerializedReference(ref m_VectorParameters, "m_VectorParameters");
    }
}








