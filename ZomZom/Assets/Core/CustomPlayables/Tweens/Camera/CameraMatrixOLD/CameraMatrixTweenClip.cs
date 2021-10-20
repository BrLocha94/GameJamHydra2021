using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CameraMatrixTweenClip : TweenClipBase<PRSTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PRSTweenBehaviour>.Create(graph, template);
        return playable;
    }
}