using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TMPTweenBehaviour : DelayedTweenBehaviour
{
    public Vector3TweenParameter m_ScaleParameter = new Vector3TweenParameter(Vector3.one,Vector3.zero);

    public Vector3TweenParameter m_PositionParameter = new Vector3TweenParameter(Vector3.zero,Vector3.zero);

    public Vector3TweenParameter m_RotationParameter = new Vector3TweenParameter(Vector3.zero,Vector3.zero);

    public Vector2TweenParameter m_PivotOffsetParameter = new Vector2TweenParameter(Vector2.zero,Vector2.zero);

    public GradientTweenParameter m_GradientParameter = new GradientTweenParameter(Color.clear);
}
