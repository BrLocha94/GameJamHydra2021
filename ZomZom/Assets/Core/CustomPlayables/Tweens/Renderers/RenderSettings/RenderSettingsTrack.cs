using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 1f)]
[TrackClipType(typeof(RenderSettingsClip))]
public class RenderSettingsTrack : PlaybleTweenTrack<RenderSettingsBehaviour, Object, RenderSettingsTweenMixerData>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<RenderSettingsMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}
