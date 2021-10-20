using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.55f, 0.2f, 0.1f)]
[TrackClipType(typeof(MaterialTweenClip))]
[TrackBindingType(typeof(Material))]
public class MaterialAssetTweenTrack : PlaybleTweenTrack<MaterialTweenBehaviour, Material, MaterialTweenMixerData>
{
    public bool clampColor = false;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<MaterialAssetTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}