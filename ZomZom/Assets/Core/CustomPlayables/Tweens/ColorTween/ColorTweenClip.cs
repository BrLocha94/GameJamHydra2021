using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ColorTweenClip : TweenClipBase<ColorTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ColorTweenBehaviour>.Create(graph, template);
        return playable;
    }
}
