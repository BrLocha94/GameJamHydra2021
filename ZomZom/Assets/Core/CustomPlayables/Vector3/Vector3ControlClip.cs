using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class Vector3ControlClip :  PlayableAsset, ITimelineClipAsset
{
    public Vector3ControlBehaviour template = new Vector3ControlBehaviour();
    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<Vector3ControlBehaviour>.Create(graph, template);
        return playable;
    }
}
    

