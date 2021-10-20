using System;
using UnityEngine;
using UnityEngine.Playables;

public class Vector3TweenMixerBehaviour : TweenMixerBehaviour<Vector3TweenBehaviour,Vector3Control, TweenMixerData<Vector3>>
{
    protected Vector3 m_DefaultValue;

    private TweenMixerData<Vector3> m_BlendedValue = new TweenMixerData<Vector3>();

    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();

        if(track == masterTrack)
        {
            m_DefaultValue = trackBinding.value;
        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            trackBinding.value = m_DefaultValue;
        }
    }
    protected override ref TweenMixerData<Vector3> ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        float valueTotalWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<Vector3TweenBehaviour> playableInput = (ScriptPlayable<Vector3TweenBehaviour>)playable.GetInput(i);
            var input = playableInput.GetBehaviour();

            if (playerData == null)
                continue;

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
    protected override ref TweenMixerData<Vector3> AddMixTrack(ref TweenMixerData<Vector3> currentData, ref TweenMixerData<Vector3> lastData)
    {
        currentData.data+= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<Vector3> SubtracMixTrack(ref TweenMixerData<Vector3> currentData, ref TweenMixerData<Vector3> lastData)
    {
        currentData.data-= lastData.data;
        return ref currentData;
    }
    protected override ref TweenMixerData<Vector3> MultiplyMixTrack(ref TweenMixerData<Vector3> currentData, ref TweenMixerData<Vector3> lastData)
    {
        currentData.data = Vector3.Scale(currentData.data,lastData.data);
        return ref currentData;
    }
    protected override ref TweenMixerData<Vector3> AverageMixTrack(ref TweenMixerData<Vector3> currentData, ref TweenMixerData<Vector3> lastData)
    {
        currentData.data+= lastData.data;
        currentData.data*=0.5f;
        return ref currentData;
    }
    protected override ref TweenMixerData<Vector3> OverrideMixTrack(ref TweenMixerData<Vector3> currentData, ref TweenMixerData<Vector3> lastData)
    {
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref TweenMixerData<Vector3> processedData)
    {
        trackBinding.value = processedData.data;
    }
}