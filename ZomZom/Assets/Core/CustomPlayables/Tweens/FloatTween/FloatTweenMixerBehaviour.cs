using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FloatTweenMixerBehaviour : TweenMixerBehaviour<FloatTweenBehaviour, TweenableBase<float>, TweenMixerData<float>>
{
    protected List<float> m_DefaultValue = new List<float>();

    private TweenMixerData<float> m_BlendedValue = new TweenMixerData<float>();
    private int tweenableIndex => (masterTrack as FloatTweenTrack).TweenableIndex;
    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        Dictionary<int, string> tweenables = trackBinding.TweenableMembers;

        for (int i = 0; i < tweenables.Count; i++)
        {
            m_DefaultValue.Add(trackBinding.GetTweenableValue(i));
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
    protected override ref TweenMixerData<float> ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        float valueTotalWeight = 0f;

        m_BlendedValue.data = 0;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<FloatTweenBehaviour> playableInput = (ScriptPlayable<FloatTweenBehaviour>)playable.GetInput(i);
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
    protected override ref TweenMixerData<float> AddMixTrack(ref TweenMixerData<float> currentData, ref TweenMixerData<float> lastData)
    {
        currentData.data += lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<float> SubtracMixTrack(ref TweenMixerData<float> currentData, ref TweenMixerData<float> lastData)
    {
        currentData.data -= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<float> MultiplyMixTrack(ref TweenMixerData<float> currentData, ref TweenMixerData<float> lastData)
    {
        currentData.data *= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<float> AverageMixTrack(ref TweenMixerData<float> currentData, ref TweenMixerData<float> lastData)
    {
        currentData.data += lastData.data;
        currentData.data *= 0.5f;
        return ref currentData;
    }
    protected override ref TweenMixerData<float> OverrideMixTrack(ref TweenMixerData<float> currentData, ref TweenMixerData<float> lastData)
    {
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref TweenMixerData<float> processedData)
    {
        trackBinding.SetTweenableValue(tweenableIndex, processedData.data);
    }
}
