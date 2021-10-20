using System;
using UnityEditor;
using UnityEngine;
using static TransformTweenBehaviour;

[CustomEditor(typeof(TransformTweenClip))]
public class TransformTweenClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty m_TweenMode;

    private SerializedProperty m_PositionTweenParameter;
    private SerializedProperty m_RotationTweenParameter;
    private SerializedProperty m_ScaleTweenParameter;

    private SerializedProperty m_TransformPositionTweenParameter;
    private SerializedProperty m_TransformRotationTweenParameter;
    private SerializedProperty m_TransformScaleTweenParameter;

    private SerializedProperty m_BezierPositionTweenParameter;
    private SerializedProperty m_BezierRotationTweenParameter;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(m_TweenMode);

        var mode = (ETweenMode)m_TweenMode.enumValueIndex;

        switch (mode)
        {
            case ETweenMode.Transform:
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    PlayableEditorCommons.DrawTransformValueTweenParameter(m_TransformPositionTweenParameter, "Position");
                    PlayableEditorCommons.DrawTransformValueTweenParameter(m_TransformRotationTweenParameter, "Rotation");
                    PlayableEditorCommons.DrawTransformValueTweenParameter(m_TransformScaleTweenParameter, "Scale");
                }
                EditorGUILayout.EndVertical();
                break;
            case ETweenMode.Value:

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    PlayableEditorCommons.DrawValueTweenParameter(m_PositionTweenParameter, "Position");
                    PlayableEditorCommons.DrawValueTweenParameter(m_RotationTweenParameter, "Rotation");
                    PlayableEditorCommons.DrawValueTweenParameter(m_ScaleTweenParameter, "Scale");
                }
                EditorGUILayout.EndVertical();


                break;
            case ETweenMode.Bezier:
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    PlayableEditorCommons.DrawBezierValueTweenParameter(m_BezierPositionTweenParameter, "Position");
                    PlayableEditorCommons.DrawBezierValueTweenParameter(m_BezierRotationTweenParameter, "Rotation");
                }
                EditorGUILayout.EndVertical();
                break;
            default:
                break;
        }


        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected override void GetReferences()
    {
        base.GetReferences();

        GetSerializedReference(ref m_TweenMode, "m_TweenMode");
        GetSerializedReference(ref m_PositionTweenParameter, "m_PositionTweenParameter");
        GetSerializedReference(ref m_RotationTweenParameter, "m_RotationTweenParameter");
        GetSerializedReference(ref m_ScaleTweenParameter, "m_ScaleTweenParameter");

        GetSerializedReference(ref m_TransformPositionTweenParameter, "m_TransformPositionTweenParameter");
        GetSerializedReference(ref m_TransformRotationTweenParameter, "m_TransformRotationTweenParameter");
        GetSerializedReference(ref m_TransformScaleTweenParameter, "m_TransformScaleTweenParameter");

        GetSerializedReference(ref m_BezierPositionTweenParameter, "m_BezierPositionTweenParameter");
        GetSerializedReference(ref m_BezierRotationTweenParameter, "m_BezierRotationTweenParameter");


    }
}

[CustomEditor(typeof(ParticleSystemTweenClip))]
public class ParticleSystemClipInspectorEditor : TweenClipInspectorBaseEditor
{
    private SerializedProperty m_BezierPositionTweenParameter;
    private SerializedProperty m_BezierPosition2TweenParameter;
    private SerializedProperty m_BlendBezierTweenParameter;
    private SerializedProperty m_Delay;
    private SerializedProperty m_ParticlesGroup;
    private SerializedProperty m_RandomOrder;
    private SerializedProperty m_RandomSpacing;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTweenClipBase();
        DrawCallbackTweenBehavior();

        EditorGUILayout.PropertyField(m_Delay);
        EditorGUILayout.PropertyField(m_RandomOrder);
        EditorGUILayout.PropertyField(m_RandomSpacing);
        EditorGUILayout.PropertyField(m_ParticlesGroup);
        PlayableEditorCommons.DrawValueTweenParameter(m_BlendBezierTweenParameter, "Blend Bezier");
        PlayableEditorCommons.DrawBezierValueTweenParameter(m_BezierPositionTweenParameter, "Position");
        PlayableEditorCommons.DrawBezierValueTweenParameter(m_BezierPosition2TweenParameter, "Position2");

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref m_BezierPositionTweenParameter, "m_BezierPositionTweenParameter");
        GetSerializedReference(ref m_BezierPosition2TweenParameter, "m_BezierPosition2TweenParameter");
        GetSerializedReference(ref m_BlendBezierTweenParameter, "m_BlendBezierTweenParameter");
        GetSerializedReference(ref m_Delay, "delay");
        GetSerializedReference(ref m_RandomOrder, "randomOrder");
        GetSerializedReference(ref m_RandomSpacing, "randomSpacing");
        GetSerializedReference(ref m_ParticlesGroup, "particlesGroup");
    }
}



