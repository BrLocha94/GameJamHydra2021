using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JazzDev.Pool
{
    public class SoundPoolManager : PoolManagerGeneric<AudioPoolObject>
    {
        [SerializeField] private AudioClipDefinition[] audioClipsDefinition;

        protected Dictionary<string, AudioClipDefinition> audioClipsDictionary;

        [SerializeField]protected bool mute = false;

        protected override void Awake()
        {
            base.Awake();

            audioClipsDictionary = new Dictionary<string, AudioClipDefinition>(audioClipsDefinition.Length);

            for (int i = 0; i < audioClipsDefinition.Length; i++)
            {
                audioClipsDictionary.Add(audioClipsDefinition[i].ClipName, audioClipsDefinition[i]);
            }
        }

        public virtual void Play(string clipName, float volume = 1, float pitch = 1, float delay = 0)
        {
            AudioClipDefinition clipDef = GetAudioClipDefinitionByName(clipName);
            SpawAudio(clipDef.GetAudioClip(), volume, pitch, delay);
        }

        public virtual void Play(string clipName)
        {
            AudioClipDefinition clipDef = GetAudioClipDefinitionByName(clipName);
            SpawAudio(clipDef.GetAudioClip(), clipDef.GetVolume(), clipDef.GetPitch(), clipDef.GetDelay());
        }

        protected virtual void SpawAudio(AudioClip clip, float volume, float pitch, float delay)
        {
            if(mute)return;

            AudioPoolObject audioObject = Spawn(Vector3.zero, Quaternion.identity, this.transform);
            if(audioObject==null)return;

            float totalLifeTime = ((clip.length + 0.05f) / pitch) + delay;
            audioObject.AudioSource.clip = clip;
            audioObject.AudioSource.volume = volume;
            audioObject.AudioSource.pitch = pitch;
            audioObject.AudioSource.PlayDelayed(delay);
            Despawn(audioObject, totalLifeTime);
        }

        public virtual void SetMute(bool mute) => this.mute = mute;

        protected virtual AudioClipDefinition GetAudioClipDefinitionByName(string clipName)
        {
            return audioClipsDictionary[clipName];
        }

        [System.Serializable]
        protected class AudioClipDefinition
        {
            [SerializeField] private string clipName;
            [SerializeField] private AudioClip[] audioClips;
            [SerializeField] private float minVolume = 1f;
            [SerializeField] private float maxVolume = 1f;
            [SerializeField] private float minPitch = 1f;
            [SerializeField] private float maxPitch = 1f;
            [SerializeField] private float minDelay = 0f;
            [SerializeField] private float maxDelay = 0f;

            public string ClipName { get => clipName; }

            public AudioClip GetAudioClip()
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)];
            }
            public float GetVolume()
            {
                return UnityEngine.Random.Range(minVolume, Mathf.Max(minVolume, maxVolume));
            }
            public float GetPitch()
            {
                return UnityEngine.Random.Range(minPitch, Mathf.Max(minPitch, maxPitch));
            }
            public float GetDelay()
            {
                return UnityEngine.Random.Range(minDelay, Mathf.Max(minDelay, maxDelay));
            }
        }
    }
}
