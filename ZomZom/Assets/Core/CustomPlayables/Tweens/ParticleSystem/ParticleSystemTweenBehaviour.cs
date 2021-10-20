using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ParticleSystemTweenBehaviour : PlayableTweenBehaviourBase
{
    [Min(0)]
    public float delay = 0.0f;

    [Min(1)]
    public int particlesGroup = 4;

    public bool randomSpacing = false;

    [SerializeField] private bool randomOrder = false;
    [SerializeField] private int randomOrderSeed = 0;

    public int particlesCount;

    [SerializeField] private BezierPositionTweenParameter m_BezierPositionTweenParameter = new BezierPositionTweenParameter(Vector3.zero, enable: true);
    [SerializeField] private BezierPositionTweenParameter m_BezierPosition2TweenParameter = new BezierPositionTweenParameter(Vector3.zero, enable: true);
    [SerializeField] private FloatTweenParameter m_BlendBezierTweenParameter = new FloatTweenParameter(0.5f, 0.5f, enable: true);
    public Vector3 GetStartEndValue(float t, float spacing)
    {
        Vector3 pos1 = m_BezierPositionTweenParameter.GetValue(t);

        if (m_BezierPosition2TweenParameter.enable)
        {
            Vector3 pos2 = m_BezierPosition2TweenParameter.GetValue(t);

            return Vector3.LerpUnclamped(pos1, pos2, spacing);
        }
        else
        {
            return m_BezierPositionTweenParameter.GetValue(t);
        }

    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        CalculateRandomValues();
    }

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        var graph = playable.GetGraph();
        m_BezierPositionTweenParameter.Resolve(graph);
        m_BezierPosition2TweenParameter.Resolve(graph);
    }

    public List<int> randomOrderList { private set; get; } = new List<int>();
    public List<float> randomSpacingList { private set; get; } = new List<float>();
    public void CalculateRandomValues()
    {
        randomOrderList.Clear();
        randomSpacingList.Clear();
        var spacingRnd = new System.Random(randomOrderSeed);
        for (int i = 0; i < particlesCount; i++)
        {
            randomOrderList.Add(i);
            randomSpacingList.Add((float)spacingRnd.NextDouble());
        }

        if (randomOrder) randomOrderList.Shuffle(spacingRnd);
    }
}

