using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class EditorAudioPreviewHandler
{
    public AudioClip targetAudioClip;
    private MethodInfo playMethod;
    private MethodInfo stopMethod;
    private MethodInfo isPlayingMethod;

    public void Initialize()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        playMethod = audioUtilClass.GetMethod("PlayPreviewClip");
        stopMethod = audioUtilClass.GetMethod("StopAllPreviewClips");
        isPlayingMethod = audioUtilClass.GetMethod("IsPreviewClipPlaying");
    }
    public void PlayAudio()
    {
        if (playMethod != null && targetAudioClip != null)
        {
            playMethod.Invoke(null, new object[] { targetAudioClip, 0, false });
        }
    }
    public void StopAudio()
    {
        if (stopMethod != null && targetAudioClip != null)
        {
            stopMethod.Invoke(null, new object[] { });
        }
    }
    public bool IsPlaying()
    {
        if (isPlayingMethod != null && targetAudioClip != null) return (bool)isPlayingMethod.Invoke(null, new object[] {});
        return false;
    }
}
