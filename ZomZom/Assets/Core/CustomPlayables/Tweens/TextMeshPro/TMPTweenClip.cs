using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TMPTweenClip : TweenClipBase<TMPTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TMPTweenBehaviour>.Create(graph, template);
        return playable;
    }
}