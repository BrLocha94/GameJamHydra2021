using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;


[TrackColor(0.22f, 0.2f, 1)]
[TrackClipType(typeof(ColorControlClip))]
[TrackBindingType(typeof(ColorControl))]
public class ColorControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ColorControlMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
       ColorControl trackBinding = director.GetGenericBinding(this) as ColorControl;
       if (trackBinding == null)
           return;
       driver.AddFromName<ColorControl>(trackBinding.gameObject, "m_Value");
#endif
        base.GatherProperties(director, driver);
    }
}
