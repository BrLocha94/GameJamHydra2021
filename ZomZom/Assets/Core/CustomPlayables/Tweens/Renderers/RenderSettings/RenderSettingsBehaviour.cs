using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class RenderSettingsBehaviour : PlayableTweenBehaviourBase
{
    public GradientTweenParameter skyColorGradientTweenParameter;
    public GradientTweenParameter equatorColorGradientTweenParameter;
    public GradientTweenParameter groundColorGradientTweenParameter;
    public GradientTweenParameter ambientColorGradientTweenParameter;
    public FloatTweenParameter ambientIntensity;
    public FloatTweenParameter reflectionIntensity;
}
