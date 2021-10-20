using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.55f, 1f, 0.5f)]
[TrackClipType(typeof(TransformTweenClip))]
[TrackBindingType(typeof(Transform))]
public class TransformTweenTrack : PRSTweenTrack<TransformTweenBehaviour, Transform, TransformTweenMixerData>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<TransformTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}
