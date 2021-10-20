using UnityEngine;

[System.Serializable]
public class LightTweenBehaviour: PlayableTweenBehaviourBase
{
    [SerializeField] private GradientTweenParameter m_ColorTweenParameter = new GradientTweenParameter(Color.white);
    [SerializeField] private FloatTweenParameter m_IntensityTweenParameter = new FloatTweenParameter(1,0);

    public GradientTweenParameter ColorTweenParameter=>m_ColorTweenParameter;
    public FloatTweenParameter IntensityTweenParameter=>m_IntensityTweenParameter;
}
