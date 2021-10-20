using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.155f, 0.8623f, 1f)]
[TrackClipType(typeof(CallbackTweenClip))]
[TrackBindingType(typeof(GameObject), TrackBindingFlags.None)]
public class CallbackTweenTrack : TrackAsset
{
    protected override void OnCreateClip(TimelineClip clip)
    {
        base.OnCreateClip(clip);
        var loopClip = clip.asset as CallbackTweenClip;
        loopClip.template.clip = clip;
    }
}
