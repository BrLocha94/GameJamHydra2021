using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 1f)]
[TrackClipType(typeof(ReflectionProbeClip))]
[TrackBindingType(typeof(ReflectionProbe))]
public class ReflectionProbeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ReflectionProbeMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        ReflectionProbe trackBinding = director.GetGenericBinding(this) as ReflectionProbe;
        if (trackBinding == null)
            return;
        driver.AddFromName<ReflectionProbe>(trackBinding.gameObject, "m_IntensityMultiplier");
        driver.AddFromName<ReflectionProbe>(trackBinding.gameObject, "m_Mode");
        driver.AddFromName<ReflectionProbe>(trackBinding.gameObject, "m_RefreshMode");
#endif
        base.GatherProperties(director, driver);
    }
}
