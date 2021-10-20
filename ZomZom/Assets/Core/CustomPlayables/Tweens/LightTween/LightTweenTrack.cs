using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 1f)]
[TrackClipType(typeof(LightTweenClip))]
[TrackBindingType(typeof(Light))]
public class LightTweenTrack : PlaybleTweenTrack<LightTweenBehaviour, Light, LightTweenMixerData>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<LightTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    /*public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        Light trackBinding = director.GetGenericBinding(this) as Light;
        if (trackBinding == null)
            return;
        driver.AddFromName<Light>(trackBinding.gameObject, "m_Intensity");
        driver.AddFromName<Light>(trackBinding.gameObject, "m_Color");
#endif
        base.GatherProperties(director, driver);
    }*/
}
