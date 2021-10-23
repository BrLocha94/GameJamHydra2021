using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.455f, 0.1623f, 1f)]
[TrackClipType(typeof(ColorTweenClip))]
[TrackBindingType(typeof(TweenableBase<Color>))]
public class ColorTweenTrack : PlaybleTweenTrack<ColorTweenBehaviour, TweenableBase<Color>, TweenMixerData<Color>>
{
    [Min(0)]
    [SerializeField] private int tweenableIndex;
    [SerializeField] private string tweenableMember;
#if UNITY_EDITOR
    private TweenableBase<Color> tweenable;
#endif
    public int TweenableIndex=> tweenableIndex;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<ColorTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        tweenable = director.GetGenericBinding(this) as TweenableBase<Color>;
        Inspector();
#endif
    }

    
#if UNITY_EDITOR

    private void OnValidate()
    {
        Inspector();
    }

    private void Inspector()
    {
        if (tweenable != null)
        {
            tweenableIndex = Mathf.Min(tweenableIndex, tweenable.TweenableMembers.Count - 1);
            tweenableMember = tweenable.TweenableMembers[tweenableIndex];
        }
    }

#endif
}
