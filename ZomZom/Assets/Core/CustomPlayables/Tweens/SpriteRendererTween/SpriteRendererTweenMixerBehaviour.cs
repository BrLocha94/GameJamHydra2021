using System;
using UnityEngine;
using UnityEngine.Playables;

public class SpriteRendererTweenMixerBehaviour : TweenMixerBehaviour<ColorTweenBehaviour, SpriteRenderer, TweenMixerData<Color>>
{
    protected Color m_DefaultValue;

    private TweenMixerData<Color> m_BlendedValue = new TweenMixerData<Color>();
    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        if(track == masterTrack)
        {
            m_DefaultValue = trackBinding.color;
        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            trackBinding.color = m_DefaultValue;
        }
    }
    protected override ref TweenMixerData<Color> ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        float valueTotalWeight = 0f;

        m_BlendedValue.data = Color.clear;
       
        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<ColorTweenBehaviour> playableInput = (ScriptPlayable<ColorTweenBehaviour>)playable.GetInput(i);
            var input = playableInput.GetBehaviour();

            float inputWeight = playable.GetInputWeight(i);

            var time = playableInput.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            valueTotalWeight += inputWeight;

            m_BlendedValue.data += input.GetStartEndValue(tweenProgress) * inputWeight;
        }

        m_BlendedValue.data += m_DefaultValue * (1f - valueTotalWeight);

        return ref m_BlendedValue;
    }
    protected override ref TweenMixerData<Color> AddMixTrack(ref TweenMixerData<Color> currentData, ref TweenMixerData<Color> lastData)
    {
        currentData.data += lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<Color> SubtracMixTrack(ref TweenMixerData<Color> currentData, ref TweenMixerData<Color> lastData)
    {
        currentData.data -= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<Color> MultiplyMixTrack(ref TweenMixerData<Color> currentData, ref TweenMixerData<Color> lastData)
    {
        currentData.data *= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<Color> AverageMixTrack(ref TweenMixerData<Color> currentData, ref TweenMixerData<Color> lastData)
    {
        currentData.data += lastData.data;
        currentData.data *= 0.5f;
        return ref currentData;
    }
    protected override ref TweenMixerData<Color> OverrideMixTrack(ref TweenMixerData<Color> currentData, ref TweenMixerData<Color> lastData)
    {
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref TweenMixerData<Color> processedData)
    {
        trackBinding.color = processedData.data;
    }
}
