using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;


[TrackColor(0.22f, 0.2f, 1)]
[TrackClipType(typeof(Vector3ControlClip))]
[TrackBindingType(typeof(Vector3Control))]
public class Vector3ControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<Vector3ControlMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
       Vector3Control trackBinding = director.GetGenericBinding(this) as Vector3Control;
       if (trackBinding == null)
           return;
       driver.AddFromName<Vector3Control>(trackBinding.gameObject, "m_Value");
#endif
        base.GatherProperties(director, driver);
    }
}
