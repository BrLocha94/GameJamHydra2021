using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class LightTweenClip: TweenClipBase<LightTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LightTweenBehaviour>.Create(graph, template);
        return playable;
    }
}