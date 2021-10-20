using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomAudioClip))]
public class CustomAudioClipEditor : CustomAudioClipEditorBase
{
    protected override AudioClip GetAudioClip()
    {
       return (target as CustomAudioClip).audioClip;
    }
}





