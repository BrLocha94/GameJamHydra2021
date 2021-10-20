using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.455f, 0.1623f, 1f)]
[TrackClipType(typeof(ColorTweenClip))]
[TrackBindingType(typeof(ColorControl))]
public class ColorTweenTrack : PlaybleTweenTrack<ColorTweenBehaviour, ColorControl, TweenMixerData<Color>>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<ColorTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        ColorControl trackBinding = director.GetGenericBinding(this) as ColorControl;
        if (trackBinding == null)
            return;
        driver.AddFromName<FloatControl>(trackBinding.gameObject, "m_Value");
#endif
        base.GatherProperties(director, driver);
    }
}
