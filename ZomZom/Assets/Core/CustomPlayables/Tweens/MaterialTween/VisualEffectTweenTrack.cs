using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.VFX;

[TrackColor(0.1f, 0.22f, 0.12f)]
[TrackClipType(typeof(MaterialTweenClip))]
[TrackBindingType(typeof(VisualEffect))]
public class VisualEffectTweenTrack : PlaybleTweenTrack<MaterialTweenBehaviour, VisualEffect, MaterialTweenMixerData>
{
    public int materialIndex;
    public bool clampColor = false;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<VisualEffectTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}
