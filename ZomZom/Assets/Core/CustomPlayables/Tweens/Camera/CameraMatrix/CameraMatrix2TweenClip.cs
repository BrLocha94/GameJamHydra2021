using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CameraMatrix2TweenClip : TweenClipBase<CameraMatrix2TweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraMatrix2TweenBehaviour>.Create(graph, template);
        return playable;
    }
}
