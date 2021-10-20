using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;


[TrackColor(0.22f, 0.2f, 1)]
[TrackClipType(typeof(FloatControlClip))]
[TrackBindingType(typeof(FloatControl))]
public class FloatControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<FloatControlMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
       FloatControl trackBinding = director.GetGenericBinding(this) as FloatControl;
       if (trackBinding == null)
           return;
       driver.AddFromName<FloatControl>(trackBinding.gameObject, "m_Value");
#endif
        base.GatherProperties(director, driver);
    }
}
