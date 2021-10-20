using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class FloatControlClip : PlayableAsset, ITimelineClipAsset
{
    public FloatControlBehaviour template = new FloatControlBehaviour();

    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.ClipIn | ClipCaps.Extrapolation | ClipCaps.Looping | ClipCaps.SpeedMultiplier;
        }
    }


    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FloatControlBehaviour>.Create(graph, template);
        return playable;
    }
}


