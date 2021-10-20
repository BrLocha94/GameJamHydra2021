using UnityEditor;
using UnityEngine;


public abstract class CustomAudioClipEditorBase : Editor
{
    private EditorAudioPreviewHandler editorAudioPreviewHandler = new EditorAudioPreviewHandler();

    private string buttonText;

    private void OnEnable() => editorAudioPreviewHandler.Initialize();

    private void OnDisable() => editorAudioPreviewHandler.StopAudio();

    protected abstract AudioClip GetAudioClip();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool isPlaying = editorAudioPreviewHandler.IsPlaying();

        buttonText = isPlaying ? "Stop" : "Play";

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Audio clip preview");

            if (GUILayout.Button(buttonText))
            {
                if (!isPlaying)
                {
                    editorAudioPreviewHandler.targetAudioClip = GetAudioClip(); //(target as CustomAudioClip).audioClip;
                    editorAudioPreviewHandler.PlayAudio();
                }
                else
                {
                    editorAudioPreviewHandler.StopAudio();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}



