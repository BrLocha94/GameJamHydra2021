using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.870f)]
[TrackClipType(typeof(CameraMatrixTweenClip))]
[TrackBindingType(typeof(Camera))]
public class CameraMatrixTweenTrack : PRSTweenTrack<PRSTweenBehaviour, Camera, TransformTweenMixerData>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<CameraMatrixTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixer.GetBehaviour());
        return mixer;
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        
    }
}
