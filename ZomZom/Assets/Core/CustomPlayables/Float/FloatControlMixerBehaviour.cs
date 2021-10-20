using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FloatControlMixerBehaviour : PlayableBehaviour
{
    float m_DefaultValue;

    FloatControl m_FloatControlBinding;

    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_FloatControlBinding = playerData as FloatControl;

        if (m_FloatControlBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultValue = m_FloatControlBinding.value;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount ();

        float blendedValue = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<FloatControlBehaviour> inputPlayable = (ScriptPlayable<FloatControlBehaviour>)playable.GetInput(i);
            FloatControlBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedValue += input.value * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        m_FloatControlBinding.value = blendedValue + m_DefaultValue * (1f - totalWeight);
    }

    public override void OnPlayableDestroy (Playable playable)
    {
        m_FirstFrameHappened = false;

        if(m_FloatControlBinding == null)
            return;

        m_FloatControlBinding.value = m_DefaultValue;
    }
}
