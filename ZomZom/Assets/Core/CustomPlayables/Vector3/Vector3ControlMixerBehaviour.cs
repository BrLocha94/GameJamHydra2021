using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Vector3ControlMixerBehaviour : PlayableBehaviour
{
    Vector3 m_DefaultValue;

    Vector3Control m_Vector3ControlBinding;

    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_Vector3ControlBinding = playerData as Vector3Control;

        if (m_Vector3ControlBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultValue = m_Vector3ControlBinding.value;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount ();

        Vector3 blendedValue = Vector3.zero;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<Vector3ControlBehaviour> inputPlayable = (ScriptPlayable<Vector3ControlBehaviour>)playable.GetInput(i);
            Vector3ControlBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedValue += input.value * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        m_Vector3ControlBinding.value = blendedValue + m_DefaultValue * (1f - totalWeight);
    }

    public override void OnPlayableDestroy (Playable playable)
    {
        m_FirstFrameHappened = false;

        if(m_Vector3ControlBinding == null)
            return;

        m_Vector3ControlBinding.value = m_DefaultValue;
    }
}
