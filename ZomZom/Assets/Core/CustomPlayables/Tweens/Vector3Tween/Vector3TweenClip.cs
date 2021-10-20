using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class Vector3TweenClip : TweenClipBase<Vector3TweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<Vector3TweenBehaviour>.Create(graph, template);
        return playable;
    }
}