using UnityEditor;
using UnityEngine;

public class CallbackTweenClipInspectorBaseEditor : TimelineClipInspectorEditorBase
{
    private SerializedProperty callbackVoidEvents;
    private SerializedProperty callbackBoolEvents;
    private SerializedProperty callbackFloatEvents;
    private SerializedProperty callbackIntEvents;
    private SerializedProperty callbackStringEvents;
    private SerializedProperty callbackAudioEvents;

    private GUIContent eventVoid = new GUIContent("Void");
    private GUIContent eventBool = new GUIContent("Bool");
    private GUIContent eventFloat = new GUIContent("Float");
    private GUIContent eventInt = new GUIContent("Int");
    private GUIContent eventString = new GUIContent("String");
    private GUIContent eventAudio = new GUIContent("Audio");

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawCallbackTweenBehavior();

        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawCallbackTweenBehavior()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Callbacks");
            EditorGUILayout.PropertyField(callbackVoidEvents, eventVoid);
            EditorGUILayout.PropertyField(callbackBoolEvents, eventBool);
            EditorGUILayout.PropertyField(callbackFloatEvents, eventFloat);
            EditorGUILayout.PropertyField(callbackIntEvents, eventInt);
            EditorGUILayout.PropertyField(callbackStringEvents, eventString);
            EditorGUILayout.PropertyField(callbackAudioEvents, eventAudio);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    protected override void GetReferences()
    {
        base.GetReferences();
        GetSerializedReference(ref callbackVoidEvents, "callbackVoidEvents");
        GetSerializedReference(ref callbackBoolEvents, "callbackBoolEvents");
        GetSerializedReference(ref callbackFloatEvents, "callbackFloatEvents");
        GetSerializedReference(ref callbackIntEvents, "callbackIntEvents");
        GetSerializedReference(ref callbackStringEvents, "callbackStringEvents");
        GetSerializedReference(ref callbackAudioEvents, "callbackAudioEvents");
    }
}

