using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.8f, 0.1f, 0.2f)]
[TrackClipType(typeof(ColorTweenClip))]
[TrackBindingType(typeof(SpriteRenderer))]
public class SpriteRendererTweenTrack : PlaybleTweenTrack<ColorTweenBehaviour, SpriteRenderer, TweenMixerData<Color>>
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph,go,inputCount);
        var mixer = ScriptPlayable<SpriteRendererTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        SpriteRenderer trackBinding = director.GetGenericBinding(this) as SpriteRenderer;
        if (trackBinding == null)
            return;
        driver.AddFromName<SpriteRenderer>(trackBinding.gameObject, "m_Color");
#endif
        base.GatherProperties(director, driver);
    }
}
