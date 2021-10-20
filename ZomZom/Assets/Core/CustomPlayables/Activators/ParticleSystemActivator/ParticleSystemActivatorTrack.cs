using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 1f)]
[TrackClipType(typeof(ParticleSystemActivatorClip))]
[TrackBindingType(typeof(ParticleSystem))]
public class ParticleSystemActivatorTrack : TrackAsset
{
    ParticleSystemActivatorMixerPlayable m_ActivationMixer;
    public ParticleSystemStopBehavior stopBehaviour = ParticleSystemStopBehavior.StopEmitting;
    public bool includeChildren = true;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ParticleSystemActivatorMixerPlayable.Create(graph, inputCount);
        m_ActivationMixer = mixer.GetBehaviour();
        m_ActivationMixer.track = this;
        return mixer;
    }
}
