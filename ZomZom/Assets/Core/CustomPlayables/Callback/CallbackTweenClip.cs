using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CallbackTweenClip : TweenClipBase<CallbackTweenBehaviour>
{
    public override ClipCaps clipCaps => ClipCaps.SpeedMultiplier;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CallbackTweenBehaviour>.Create(graph, template);
        return playable;
    }

}