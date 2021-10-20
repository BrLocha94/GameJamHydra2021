using UnityEditor;
using UnityEngine;

public class PlayableEditorCommons
{
    private static string[] m_ChannelsName = new string[] { "X", "Y", "Z", "W" };

    public static void DrawTweenParameter(SerializedProperty property, string name = null)
    {
        var type = property.FindPropertyRelative("m_Type");
        var value = property.FindPropertyRelative("m_Value");
        var ease = property.FindPropertyRelative("m_Ease");
        var curve = property.FindPropertyRelative("m_Curve");
        var multiplier = property.FindPropertyRelative("m_Multiplier");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name != null ? name : property.displayName);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(type, GUIContent.none);

                if (type.enumValueIndex == 0)
                {
                    EditorGUILayout.PropertyField(value, GUIContent.none);
                }
                else if (type.enumValueIndex == 1)
                {
                    EditorGUILayout.PropertyField(ease, GUIContent.none);
                }
                else if (type.enumValueIndex == 2)
                {
                    EditorGUILayout.PropertyField(curve, GUIContent.none);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(multiplier);
        }
        EditorGUILayout.EndVertical();
    }

    public static void DrawNoiseChannelConfig(params SerializedProperty[] properties)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                var noiseChannel = properties[i];
                var frequency = noiseChannel.FindPropertyRelative("frequency");
                var amplitude = noiseChannel.FindPropertyRelative("amplitude");
                var valueCenter = noiseChannel.FindPropertyRelative("valueCenter");
                var multiplier = noiseChannel.FindPropertyRelative("multiplier");
                var offset = noiseChannel.FindPropertyRelative("offset");
                var enable = noiseChannel.FindPropertyRelative("enable");

                EditorGUILayout.LabelField(noiseChannel.displayName, EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(enable);

                if (enable.boolValue)
                {
                    EditorGUILayout.PropertyField(multiplier);
                    EditorGUILayout.PropertyField(offset);
                    DrawTweenParameter(frequency);
                    DrawTweenParameter(amplitude);
                    DrawTweenParameter(valueCenter);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
    public static void DrawValueTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var start = property.FindPropertyRelative("m_Start");
        var end = property.FindPropertyRelative("m_End");
        var tween = property.FindPropertyRelative("m_Tween");
        var dataAsset = property.FindPropertyRelative("m_StarEndData");
        var fromAsset = property.FindPropertyRelative("m_FromAsset");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                EditorGUILayout.PropertyField(fromAsset);

                if (fromAsset.boolValue)
                {
                    EditorGUILayout.PropertyField(dataAsset);
                }
                else
                {
                    EditorGUILayout.PropertyField(start);
                    EditorGUILayout.PropertyField(end);
                }

                DrawTweenParameter(tween);
            }
        }
        EditorGUILayout.EndVertical();
    }
    public static void DrawMultipleChannelValueTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var start = property.FindPropertyRelative("m_Start");
        var end = property.FindPropertyRelative("m_End");
        var individualChannels = property.FindPropertyRelative("m_IndividualChannels");
        var tween = property.FindPropertyRelative("m_Tween");
        var tweenIndividualChannels = property.FindPropertyRelative("m_TweenIndividualChannels");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                EditorGUILayout.PropertyField(start);
                EditorGUILayout.PropertyField(end);
                EditorGUILayout.PropertyField(individualChannels);
                if (individualChannels.boolValue)
                {
                    int count = tweenIndividualChannels.arraySize;

                    for (int i = 0; i < count; i++)
                    {
                        var channel = tweenIndividualChannels.GetArrayElementAtIndex(i);
                        DrawNoisedTweenParameter(channel, m_ChannelsName[i]);
                    }
                }
                else
                {
                    DrawTweenParameter(tween);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
    public static void DrawRotationTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var start = property.FindPropertyRelative("m_Start");
        var end = property.FindPropertyRelative("m_End");
        var tween = property.FindPropertyRelative("m_Tween");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                DrawQuaternionParameter(start, "Start");
                DrawQuaternionParameter(end, "End");
                DrawTweenParameter(tween);
            }
        }
        EditorGUILayout.EndVertical();
    }
    private static void DrawQuaternionParameter(SerializedProperty property, string name)
    {
        var sRot = EditorGUILayout.Vector4Field(name, property.quaternionValue.ToVector4());
        GUI.enabled = false;
        var sRotAngle = sRot * Mathf.Rad2Deg;
        EditorGUILayout.Vector4Field("Degrees", sRotAngle);
        GUI.enabled = true;
        property.quaternionValue = sRot.ToQuaternion();
    }
    public static void DrawGradientValueTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var gradient = property.FindPropertyRelative("m_Gradient");
        var tween = property.FindPropertyRelative("m_Tween");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                EditorGUILayout.PropertyField(gradient);
                DrawTweenParameter(tween);
            }
        }
        EditorGUILayout.EndVertical();
    }
    public static void DrawTransformValueTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var endByOffset = property.FindPropertyRelative("m_EndByOffset");
        var endOffset = property.FindPropertyRelative("m_EndOffset");
        var start = property.FindPropertyRelative("m_Start");
        var end = property.FindPropertyRelative("m_End");
        var tween = property.FindPropertyRelative("m_Tween");
        var fromLocal = property.FindPropertyRelative("m_FromLocal");
        var individualChannels = property.FindPropertyRelative("m_IndividualChannels");
        var tweenIndividualChannels = property.FindPropertyRelative("m_TweenIndividualChannels");


        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                EditorGUILayout.PropertyField(endByOffset);
                EditorGUILayout.PropertyField(fromLocal);
                EditorGUILayout.PropertyField(individualChannels);
                EditorGUILayout.PropertyField(start);
                EditorGUILayout.PropertyField(endByOffset.boolValue ? endOffset : end);
                EditorGUILayout.Space();
                if (individualChannels.boolValue)
                {
                    int channelsCount = tweenIndividualChannels.arraySize;

                    for (int i = 0; i < channelsCount; i++)
                    {
                        var channel = tweenIndividualChannels.GetArrayElementAtIndex(i);
                        DrawNoisedTweenParameter(channel, m_ChannelsName[i]);
                        EditorGUILayout.Space();
                    }
                }
                else
                {
                    DrawTweenParameter(tween);
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
    public static void DrawBezierValueTweenParameter(SerializedProperty property, string name)
    {
        var enable = property.FindPropertyRelative("m_Enable");
        var tweenMaxTime = property.FindPropertyRelative("m_TweenMaxTime");
        var tweenMinTime = property.FindPropertyRelative("m_TweenMinTime");
        var bezier = property.FindPropertyRelative("m_Bezier");
        var evenlySpaced = property.FindPropertyRelative("m_EvenlySpaced");
        var tween = property.FindPropertyRelative("m_Tween");
        var vectorUp = property.FindPropertyRelative("m_VectorUp");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField(name);
            EditorGUILayout.PropertyField(enable);
            if (enable.boolValue)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.PropertyField(tweenMinTime);
                    EditorGUILayout.PropertyField(tweenMaxTime);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.PropertyField(evenlySpaced);
                if (vectorUp != null) EditorGUILayout.PropertyField(vectorUp);
                EditorGUILayout.PropertyField(bezier);
                DrawTweenParameter(tween);
            }
        }
        EditorGUILayout.EndVertical();
    }
    public static void DrawNoisedTweenParameter(SerializedProperty property, string name)
    {
        var noiseMode = property.FindPropertyRelative("m_NoiseMode");
        var tween = property.FindPropertyRelative("m_Tween");
        var noiseChannelConfig = property.FindPropertyRelative("m_NoiseChannelConfig");

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.PropertyField(noiseMode);
            EditorGUILayout.LabelField(name);
            if (noiseMode.boolValue)
            {
                DrawNoiseChannelConfig(noiseChannelConfig);
            }
            else
            {
                DrawTweenParameter(property, "Tween");
            }
        }
        EditorGUILayout.EndVertical();
    }

     public static void DrawMaterialTweenParameter(SerializedProperty property, string name = null)
    {
        var propertyName = property.FindPropertyRelative("m_PropertyName");
        EditorGUILayout.PropertyField(propertyName);
        DrawValueTweenParameter(property,property.displayName);
    }
}








