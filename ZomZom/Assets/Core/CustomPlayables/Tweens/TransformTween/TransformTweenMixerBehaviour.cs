using System;
using UnityEngine;
using UnityEngine.Playables;

public class TransformTweenMixerBehaviour : TransformTweenMixerBehaviourBase<TransformTweenBehaviour>
{
    protected override PRSTweenTrack<TransformTweenBehaviour, Transform, TransformTweenMixerData> m_Track
    {
        get => track as TransformTweenTrack;
    }
    protected override PRSTweenTrack<TransformTweenBehaviour, Transform, TransformTweenMixerData> m_MasterTrack
    {
        get => masterTrack as TransformTweenTrack;
    }
}