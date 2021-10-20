using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomAudioClip", menuName = "Data/Audio/Custom Audio Clip")]
public class CustomAudioClip : ScriptableObject
{
    public AudioClip audioClip;
    [Range(0, 255)] public int priority = 128;

    public float Volume
    {
        get
        {
            if (useRandomVolume) return Random.Range(minVolume, maxVolume);
            else return volume;
        }
    }
    public float Delay
    {
        get
        {
            if (useRandomDelay) return Random.Range(minDelay, maxDelay);
            else return delay;
        }
    }
    public float Pitch
    {
        get
        {
            if (useRandomPitch) return Random.Range(minPitch, maxPitch);
            else return pitch;
        }
    }
    public bool Audio2D => audio2D;

    [Header("Volume")]
    [Range(0f, 1f)] public float volume = 1f;
    [SerializeField] bool useRandomVolume = false;
    [Range(0f, 1f)] [SerializeField] float minVolume = 0.01f;
    [Range(0f, 1f)] [SerializeField] float maxVolume = 1.0f;


    [Header("Delay")]
    [Min(0f)] public float delay = 0f;
    [SerializeField] bool useRandomDelay = false;
    [Min(0f)] [SerializeField] float minDelay = 0.0f;
    [Min(0f)] [SerializeField] float maxDelay = 0.0f;

    [Header("Pitch")]
    [Range(0.1f, 3f)] public float pitch = 1.0f;
    [SerializeField] bool useRandomPitch = false;
    [Range(0.1f, 3f)] [SerializeField] float minPitch = 1.0f;
    [Range(0.1f, 3f)] [SerializeField] float maxPitch = 1.0f;

    [Header("Misc")]
    [SerializeField] bool audio2D = false;
    [Min(0)] [SerializeField] float playCoolDown = 0;
    public bool bypassPlay = false;

    private double lastPlayTime;

    /// <summary>
    /// Calls: SoundManager.Instance.PlaySingle();
    /// </summary>
    public void Play()
    {
        if (!bypassPlay && Application.isPlaying)
        {
            if (playCoolDown == 0)
            {
                PlayInternal();
            }
            else if (Time.timeAsDouble - playCoolDown >= lastPlayTime)
            {
                PlayInternal();
                lastPlayTime = Time.timeAsDouble;
            }
        }
    }

    public void Reset()
    {
        lastPlayTime = 0;
    }
    private void PlayInternal()
    {
       /* var soundManager = SoundManager.Instance;

        if (soundManager != null)
        {
            soundManager.PlaySingle(this);
        }*/

        Debug.Log("SoundManager not implemented");
    }

}

#if UNITY_EDITOR
public class CustomAudioClipMenu
{
    [UnityEditor.MenuItem("Assets/Create Custom Audio Asset")]
    private static void CreateCustomAudioAsset()
    {
        var audioClips = UnityEditor.Selection.GetFiltered<AudioClip>(UnityEditor.SelectionMode.Unfiltered);

        for (int i = 0; i < audioClips.Length; i++)
        {
            var currentAudioClip = audioClips[i];
            string path = UnityEditor.AssetDatabase.GetAssetPath(currentAudioClip);
            path = path.Remove(path.LastIndexOf("/") + 1);
            var name = string.Concat("SFX_", currentAudioClip.name);
            var newAsset = CustomAudioClip.CreateInstance<CustomAudioClip>();
            newAsset.name = name;
            newAsset.audioClip = currentAudioClip;
            path = string.Concat(path, name, ".asset");
            UnityEditor.AssetDatabase.CreateAsset(newAsset, path);
        }
    }
    [UnityEditor.MenuItem("Assets/Create Custom Audio Asset", true)]
    private static bool CreateCustomAudioAssetValidation()
    {
        return UnityEditor.Selection.GetFiltered<AudioClip>(UnityEditor.SelectionMode.Unfiltered).Length > 0;
    }
}
#endif
