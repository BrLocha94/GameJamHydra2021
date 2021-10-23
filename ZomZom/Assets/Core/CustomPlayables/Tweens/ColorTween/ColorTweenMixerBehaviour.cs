using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ColorTweenMixerBehaviour : TweenMixerBehaviour<ColorTweenBehaviour, TweenableBase<Color>, TweenMixerData<Color>>
{
   protected List<Color> m_DefaultValue = new List<Color>();

    private TweenMixerData<Color> m_BlendedValue = new TweenMixerData<Color>();

    private int tweenableIndex=> (masterTrack as ColorTweenTrack).TweenableIndex;

    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        if (track == masterTrack)
        {
            Dictionary<int, string> tweenables = trackBinding.TweenableMembers;

            for (int i = 0; i < tweenables.Count; i++)
            {
                m_DefaultValue.Add(trackBinding.GetTweenableValue(i));
            }

        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            trackBinding.SetTweenableValue(tweenableIndex, m_DefaultValue[tweenableIndex]);
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

        m_BlendedValue.data += m_DefaultValue[tweenableIndex] * (1f - valueTotalWeight);

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
        trackBinding.SetTweenableValue(tweenableIndex, processedData.data);
    }
}
