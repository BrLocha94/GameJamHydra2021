using UnityEngine;
using UnityEngine.Playables;

public class ReflectionProbeMixerBehaviour : PlayableBehaviour
{
    float m_DefaultIntensity;

    ReflectionProbe m_ReflectionProbeBinding;

    bool m_FirstFrameHappened;

    UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode;
    UnityEngine.Rendering.ReflectionProbeMode mode;


    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_ReflectionProbeBinding = playerData as ReflectionProbe;

        if (m_ReflectionProbeBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            mode = m_ReflectionProbeBinding.mode;
            refreshMode = m_ReflectionProbeBinding.refreshMode;
            m_ReflectionProbeBinding.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
            m_ReflectionProbeBinding.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            m_DefaultIntensity = m_ReflectionProbeBinding.intensity;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount();

        float blendedValue = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;
        bool update = false;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ReflectionProbeBehaviour> inputPlayable = (ScriptPlayable<ReflectionProbeBehaviour>)playable.GetInput(i);
            ReflectionProbeBehaviour input = inputPlayable.GetBehaviour();

            blendedValue += input.intensity * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately(inputWeight, 0f))
                currentInputs++;

            if(input.updateEnvironment) update = true;
        }

        m_ReflectionProbeBinding.intensity = blendedValue + m_DefaultIntensity * (1f - totalWeight);
        m_ReflectionProbeBinding.RenderProbe();

        if (update)
        {
            DynamicGI.UpdateEnvironment();
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        m_FirstFrameHappened = false;

        if (m_ReflectionProbeBinding == null)
            return;

        m_ReflectionProbeBinding.refreshMode = refreshMode;
        m_ReflectionProbeBinding.mode = mode;
        m_ReflectionProbeBinding.intensity = m_DefaultIntensity;
        m_ReflectionProbeBinding.RenderProbe();
        DynamicGI.UpdateEnvironment();
    }
}
