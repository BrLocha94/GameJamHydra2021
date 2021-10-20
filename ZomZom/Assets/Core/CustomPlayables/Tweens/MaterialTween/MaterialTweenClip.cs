using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MaterialTweenClip : TweenClipBase<MaterialTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MaterialTweenBehaviour>.Create(graph, template);
        return playable;
    }
}
