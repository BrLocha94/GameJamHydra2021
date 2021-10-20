using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.155f, 0.8623f, 1f)]
[TrackClipType(typeof(FloatTweenClip))]
[TrackBindingType(typeof(TweenableBase<float>))]
public class FloatTweenTrack : PlaybleTweenTrack<FloatTweenBehaviour, TweenableBase<float>, TweenMixerData<float>>
{
    [Min(0)]
    [SerializeField] private int tweenableIndex;
    [SerializeField] private string tweenableMember;
#if UNITY_EDITOR
    private TweenableBase<float> tweenable;
#endif
    public int TweenableIndex=> tweenableIndex;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<FloatTweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixerBehaviour);
        return mixer;
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        tweenable = director.GetGenericBinding(this) as TweenableBase<float>;
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


