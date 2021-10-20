using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Animations;
using System.Collections.Generic;

[TrackColor(0.155f, 0.8623f, 1f)]
[TrackClipType(typeof(TMPTweenClip))]
[TrackBindingType(typeof(TMP_Text))]
public class TMPTweenTrack : PlaybleTweenTrack<TMPTweenBehaviour, TMP_Text, TMPTweenMixerData>
{
    public TMP_MeshInfo[] cachedMeshInfo;

    public bool m_Scale = true;
    public bool m_Position = true;
    public bool m_Rotation = true;
    public bool m_PivotOffset = true;
    public bool m_ColorGradient = true;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<TMPTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
}
