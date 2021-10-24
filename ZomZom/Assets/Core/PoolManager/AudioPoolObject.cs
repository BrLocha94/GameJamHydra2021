using UnityEngine;


namespace JazzDev.Pool
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPoolObject : PoolObject
    {
        private AudioSource audioSource;
        public AudioSource AudioSource { get => audioSource; }

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
}
