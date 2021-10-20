using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.870f)]
[TrackClipType(typeof(ParticleSystemTweenClip))]
[TrackBindingType(typeof(ParticleSystem))]
public class ParticleSystemTweenTrack : PlaybleTweenTrack<ParticleSystemTweenBehaviour, ParticleSystem, ParticleSystemTweenMixerData>
{
    [SerializeField] private bool m_ConvertPosition = false;
    [SerializeField] private ExposedReference<Camera> m_FromCamera;
    [SerializeField] private ExposedReference<Camera> m_ToCamera;
    public bool convertPosition => m_ConvertPosition;
    public Camera fromCamera { private set; get; }
    public Camera toCamera { private set; get; }

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<ParticleSystemMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        fromCamera = m_FromCamera.Resolve(graph.GetResolver());
        toCamera = m_ToCamera.Resolve(graph.GetResolver());
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}
