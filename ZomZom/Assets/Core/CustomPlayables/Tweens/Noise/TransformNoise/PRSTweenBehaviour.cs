using JazzDev.Noiser;
using System;
using UnityEngine;

[Serializable]
public class PRSTweenBehaviour : PRSTweenBehaviourBase
{
    public float globalFrequency = 1;
    public float globalAmplitude = 1;

    [SerializeField] protected Vector3TweenParameter position = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    [SerializeField] protected Vector3TweenParameter rotation = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);
    [SerializeField] protected Vector3TweenParameter scale = new Vector3TweenParameter(Vector3.zero, Vector3.zero, false);

    public override Vector3 GetPosition(float t, Vector3 defaultPosition)
    {
        if(position.enable) return position.GetValue(t);
        return defaultPosition;
    }
    public override Vector3 GetRotation(float t, Vector3 defaultRotation)
    {
        if(rotation.enable) return rotation.GetValue(t);
        return defaultRotation;
    }
    public override Vector3 GetScale(float t, Vector3 defaultScale)
    {
        if(scale.enable) return scale.GetValue(t);
        return defaultScale;
    }
}


