using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ParticleSystemTweenClip : TweenClipBase<ParticleSystemTweenBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ParticleSystemTweenBehaviour>.Create(graph, template);
        return playable;
    }
}