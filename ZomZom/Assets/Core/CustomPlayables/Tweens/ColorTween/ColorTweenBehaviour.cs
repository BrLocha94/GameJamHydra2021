using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ColorTweenBehaviour : PlayableStartEndTweenBehaviour<Color>
{
    [SerializeField] private GradientTweenParameter startEndValueTweenParameter = new GradientTweenParameter(Color.white,true);
    public override Color GetStartEndValue(float t)
    {
        return startEndValueTweenParameter.GetValue(t);
    }
}