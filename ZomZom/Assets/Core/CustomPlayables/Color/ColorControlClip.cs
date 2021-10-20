using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ColorControlClip :  PlayableAsset, ITimelineClipAsset
{
    public ColorControlBehaviour template = new ColorControlBehaviour();
    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ColorControlBehaviour>.Create(graph, template);
        return playable;
    }
}
    

