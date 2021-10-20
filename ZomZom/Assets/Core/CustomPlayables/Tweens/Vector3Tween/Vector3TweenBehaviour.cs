using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class Vector3TweenBehaviour : PlayableStartEndTweenBehaviour<Vector3>
{
    [SerializeField] private Vector3TweenParameter startEndValueTweenParameter = new Vector3TweenParameter(Vector3.zero,Vector3.zero,true);
    public override Vector3 GetStartEndValue(float t)
    {
        return startEndValueTweenParameter.GetValue(t);
    }
}

