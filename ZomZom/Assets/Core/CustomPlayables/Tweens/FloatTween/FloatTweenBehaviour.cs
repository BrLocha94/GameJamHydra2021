using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class FloatTweenBehaviour : PlayableStartEndTweenBehaviour<float>
{
    [SerializeField] private FloatTweenParameter startEndValueTweenParameter = new FloatTweenParameter(0,0,true);
    public override float GetStartEndValue(float t)
    {
        return startEndValueTweenParameter.GetValue(t);
    }
}