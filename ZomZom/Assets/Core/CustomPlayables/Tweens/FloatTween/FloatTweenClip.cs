using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class FloatTweenClip : TweenClipBase<FloatTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FloatTweenBehaviour>.Create(graph, template);
        return playable;
    }
}