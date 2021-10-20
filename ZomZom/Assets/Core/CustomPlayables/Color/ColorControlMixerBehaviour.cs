using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ColorControlMixerBehaviour : PlayableBehaviour
{
    Color m_DefaultValue;

    ColorControl m_ColorControlBinding;

    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_ColorControlBinding = playerData as ColorControl;

        if (m_ColorControlBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultValue = m_ColorControlBinding.value;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount ();

        Color blendedValue = Color.clear;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ColorControlBehaviour> inputPlayable = (ScriptPlayable<ColorControlBehaviour>)playable.GetInput(i);
            ColorControlBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedValue += input.value * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        m_ColorControlBinding.value = blendedValue + m_DefaultValue * (1f - totalWeight);
    }

    public override void OnPlayableDestroy (Playable playable)
    {
        m_FirstFrameHappened = false;

        if(m_ColorControlBinding == null)
            return;

        m_ColorControlBinding.value = m_DefaultValue;
    }
}
