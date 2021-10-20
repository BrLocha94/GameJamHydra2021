using BezierSolution;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TransformTweenClip : TweenClipBase<TransformTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TransformTweenBehaviour>.Create(graph, template);
        TransformTweenBehaviour clone = playable.GetBehaviour();
        return playable;
    }
}
