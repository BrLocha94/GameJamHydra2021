using System;
using JazzDev.Easing;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class PlayableTweenBehaviourBase : PlayableCallbackBehaviour
{
    [SerializeField] protected EaseStyle easeStyle;
    [SerializeField] protected bool useCustomCurve;
    [SerializeField] protected bool useCurveAsset;
    [SerializeField] protected AnimationCurve customCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] protected SO_AnimationCurveData animationCurveAsset;
    [SerializeField] protected float startTime = 0;
    [SerializeField] protected float endTime = 1;

    protected Ease currentEaseFunction = EaseMethods.LinearEaseNone;

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        currentEaseFunction = EaseMethods.GetEase(easeStyle);
    }
    public virtual float EvaluateCurrentCurve(float time)
    {
        time = Mathf.Lerp(startTime,endTime,time);

        if (useCustomCurve)
        {
            if (useCurveAsset)
            {
                if (animationCurveAsset != null) {return animationCurveAsset.Data.Evaluate(time);}
                return time;
            }
            return customCurve.Evaluate(time);
        }
        else { return EaseMethods.EasedLerp(currentEaseFunction, 0, 1, time); }
    }
}
