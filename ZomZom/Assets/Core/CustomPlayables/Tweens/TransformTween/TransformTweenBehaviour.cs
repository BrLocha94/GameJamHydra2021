using BezierSolution;
using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TransformTweenBehaviour : PRSTweenBehaviourBase
{
    [SerializeField] private Vector3TweenParameter m_PositionTweenParameter = new Vector3TweenParameter(Vector3.zero, Vector3.zero);
    [SerializeField] private Vector3TweenParameter m_RotationTweenParameter = new Vector3TweenParameter(Vector3.zero, Vector3.zero);
    [SerializeField] private Vector3TweenParameter m_ScaleTweenParameter = new Vector3TweenParameter(Vector3.one, Vector3.one);

    [SerializeField] private TransformPositionTweenParameter m_TransformPositionTweenParameter = new TransformPositionTweenParameter(Vector3.zero);
    [SerializeField] private TransformRotationTweenParameter m_TransformRotationTweenParameter = new TransformRotationTweenParameter(Vector3.zero);
    [SerializeField] private TransformScaleTweenParameter m_TransformScaleTweenParameter = new TransformScaleTweenParameter(Vector3.zero);

    [SerializeField] private BezierPositionTweenParameter m_BezierPositionTweenParameter = new BezierPositionTweenParameter(Vector3.zero);
    [SerializeField] private BezierRotationTweenParameter m_BezierRotationTweenParameter = new BezierRotationTweenParameter(Vector3.zero);

    [SerializeField] private ETweenMode m_TweenMode = ETweenMode.Transform;


    public override bool position
    {
        get
        {
            if (m_TweenMode == ETweenMode.Value) return m_PositionTweenParameter.enable;
            else if (m_TweenMode == ETweenMode.Transform) return m_TransformPositionTweenParameter.enable;
            else return m_BezierPositionTweenParameter.enable;
        }
    }

    public override bool rotation
    {
        get
        {
            if (m_TweenMode == ETweenMode.Value) return m_RotationTweenParameter.enable;
            else if (m_TweenMode == ETweenMode.Transform) return m_TransformRotationTweenParameter.enable;
            else return m_BezierRotationTweenParameter.enable;
        }
    }

    public override bool scale
    {
        get
        {
            if (m_TweenMode == ETweenMode.Value) return m_ScaleTweenParameter.enable;
            else if (m_TweenMode == ETweenMode.Transform) return m_TransformScaleTweenParameter.enable;
            else return true;
        }
    }

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        var graph = playable.GetGraph();

        m_TransformPositionTweenParameter.Resolve(graph);
        m_TransformRotationTweenParameter.Resolve(graph);
        m_TransformScaleTweenParameter.Resolve(graph);

        m_BezierPositionTweenParameter.Resolve(graph);
        m_BezierRotationTweenParameter.Resolve(graph);
    }

    public override Vector3 GetPosition(float t, Vector3 defaultValue)
    {
        if (m_TweenMode == ETweenMode.Transform) return m_TransformPositionTweenParameter.GetValue(t);
        else if (m_TweenMode == ETweenMode.Value) return m_PositionTweenParameter.GetValue(t);
        else return m_BezierPositionTweenParameter.GetValue(t);
    }
    public override Vector3 GetRotation(float t, Vector3 defaultValue)
    {
        if (m_TweenMode == ETweenMode.Transform) return m_TransformRotationTweenParameter.GetValue(t);
        else if (m_TweenMode == ETweenMode.Value) return m_RotationTweenParameter.GetValue(t);
        else return m_BezierRotationTweenParameter.GetValue(t);
    }
    public override Vector3 GetScale(float t, Vector3 defaultValue)
    {
        if (m_TweenMode == ETweenMode.Transform) return m_TransformScaleTweenParameter.GetValue(t);
        else if (m_TweenMode == ETweenMode.Value) return m_ScaleTweenParameter.GetValue(t);
        else return defaultValue;
    }
}

