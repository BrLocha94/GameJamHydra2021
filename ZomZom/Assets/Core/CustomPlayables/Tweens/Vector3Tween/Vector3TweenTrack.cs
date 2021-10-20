using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.870f)]
[TrackClipType(typeof(Vector3TweenClip))]
[TrackBindingType(typeof(Vector3Control))]
public class Vector3TweenTrack : PlaybleTweenTrack<Vector3TweenBehaviour, Vector3Control, TweenMixerData<Vector3>>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<Vector3TweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        Vector3Control trackBinding = director.GetGenericBinding(this) as Vector3Control;
        if (trackBinding == null)
            return;
        driver.AddFromName<Vector3Control>(trackBinding.gameObject, "m_Value");
#endif
        base.GatherProperties(director, driver);
    }
}
