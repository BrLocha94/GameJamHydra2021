using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class TweenClipBase<T> : PlayableAsset, ITimelineClipAsset where T: PlayableCallbackBehaviour, new()
{
    public T template = new T();
    public virtual ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.Extrapolation  | ClipCaps.Blending | ClipCaps.SpeedMultiplier;
        }
    }}