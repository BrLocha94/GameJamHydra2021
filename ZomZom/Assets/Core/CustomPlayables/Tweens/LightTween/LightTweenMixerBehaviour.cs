using System;
using UnityEngine;
using UnityEngine.Playables;

public class LightTweenMixerBehaviour : TweenMixerBehaviour<LightTweenBehaviour, Light, LightTweenMixerData>
{
    protected LightTweenMixerData m_DefaultValue;
    private LightTweenMixerData m_BlendedValue = new LightTweenMixerData();
    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        if(track == masterTrack)
        {
            m_DefaultValue.intensity = trackBinding.intensity;
            m_DefaultValue.color = trackBinding.color;
        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            trackBinding.color = m_DefaultValue.color;
            trackBinding.intensity = m_DefaultValue.intensity;
        }
    }
    protected override ref LightTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        float colorTotalWeight = 0f;
        float intensityTotalWeight = 0f;

        m_BlendedValue.color = Color.clear;
        m_BlendedValue.intensity = 0;
       
        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<LightTweenBehaviour> playableInput = (ScriptPlayable<LightTweenBehaviour>)playable.GetInput(i);
            var input = playableInput.GetBehaviour();

            float inputWeight = playable.GetInputWeight(i);

            var time = playableInput.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            if(input.ColorTweenParameter.enable)
            {
                colorTotalWeight += inputWeight;
                m_BlendedValue.color += input.ColorTweenParameter.GetValue(tweenProgress) * inputWeight;
            }

            if(input.IntensityTweenParameter.enable)
            {
                intensityTotalWeight += inputWeight;
                m_BlendedValue.intensity += input.IntensityTweenParameter.GetValue(tweenProgress) * inputWeight;
            }
        }

        m_BlendedValue.color += m_DefaultValue.color * (1f - colorTotalWeight);
        m_BlendedValue.intensity += m_DefaultValue.intensity * (1f - intensityTotalWeight);

        return ref m_BlendedValue;
    }
    protected override ref LightTweenMixerData AddMixTrack(ref LightTweenMixerData currentData, ref LightTweenMixerData lastData)
    {
        currentData.color+=lastData.color;
        currentData.intensity+= lastData.intensity;
        return ref currentData;
    }
    protected override ref LightTweenMixerData SubtracMixTrack(ref LightTweenMixerData currentData, ref LightTweenMixerData lastData)
    {
         currentData.color -= lastData.color;
        currentData.intensity -= lastData.intensity;
        return ref currentData;
    }
    protected override ref LightTweenMixerData MultiplyMixTrack(ref LightTweenMixerData currentData, ref LightTweenMixerData lastData)
    {
        currentData.color *= lastData.color;
        currentData.intensity*= lastData.intensity;
        return ref currentData;
    }
    protected override ref LightTweenMixerData AverageMixTrack(ref LightTweenMixerData currentData, ref LightTweenMixerData lastData)
    {
        currentData.color+=lastData.color;
        currentData.intensity+= lastData.intensity;
        currentData.color *= 0.5f;
        currentData.intensity *= 0.5f;
        return ref currentData;
    }
    protected override ref LightTweenMixerData OverrideMixTrack(ref LightTweenMixerData currentData, ref LightTweenMixerData lastData)
    {
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref LightTweenMixerData processedData)
    {
        trackBinding.color = processedData.color;
        trackBinding.intensity = processedData.intensity;
    }
}
