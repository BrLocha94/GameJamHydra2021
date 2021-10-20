using UnityEngine;

[System.Serializable]
public class TweenAudioCallback : TweenCallbackBase
{

    [SerializeField] CustomAudioClip audioClip;
    public override void Fire()
    {
        audioClip?.Play();
    }
}
