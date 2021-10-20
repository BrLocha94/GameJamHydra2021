using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.55f, 1f, 0.5f)]
[TrackClipType(typeof(MaterialTweenClip))]
[TrackBindingType(typeof(Renderer))]
public class MaterialTweenTrack : PlaybleTweenTrack<MaterialTweenBehaviour, Renderer, MaterialTweenMixerData>
{
    public int materialIndex;
    public bool clampColor = false;
    public bool applyToSharedMaterialAsset = false;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<MaterialTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        Debug.Log(mixer.GetHashCode());
        return mixer;
    }
}