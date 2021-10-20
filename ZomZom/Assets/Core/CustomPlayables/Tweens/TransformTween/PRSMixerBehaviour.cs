using System;
using UnityEngine;
using UnityEngine.Playables;

public abstract class PRSMixerBehaviour<COMPONENT, BEHAVIOUR> : TweenMixerBehaviour<BEHAVIOUR, COMPONENT, TransformTweenMixerData>
    where BEHAVIOUR : PRSTweenBehaviourBase, new()
    where COMPONENT : class
{

    protected Vector3 m_DefaultPosition = Vector3.zero;
    public Vector3 defaultPosition => m_DefaultPosition;

    protected Vector3 m_DefaultRotation = Vector3.zero;
    public Vector3 defaultRotation => m_DefaultRotation;

    protected Vector3 m_DefaultScale = Vector3.zero;
    public Vector3 defaultScale => m_DefaultScale;

    protected abstract PRSTweenTrack<BEHAVIOUR, COMPONENT, TransformTweenMixerData> m_Track { get; }

    protected abstract PRSTweenTrack<BEHAVIOUR, COMPONENT, TransformTweenMixerData> m_MasterTrack { get; }
    protected abstract void SetPosition(Vector3 pos);
    protected abstract void SetRotation(Vector3 rot);
    protected abstract void SetScale(Vector3 scale);

    private TransformTweenMixerData m_MixerData = new TransformTweenMixerData();

    private void ConvertPosition(ref Vector3 position)
    {
        if (m_Track.convertPosition)
        {
            var screenPos = m_Track.fromCamera.WorldToScreenPoint(position);
            position = m_Track.toCamera.ScreenToWorldPoint(screenPos);
        }
    }
    protected override ref TransformTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        m_MixerData.position = Vector3.zero;
        m_MixerData.rotation = Vector3.zero;
        m_MixerData.scale = Vector3.zero;

        int inputCount = playable.GetInputCount();

        float positionTotalWeight = 0f;
        float rotationTotalWeight = 0f;
        float scaleTotalWeight = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<BEHAVIOUR> playableInput = (ScriptPlayable<BEHAVIOUR>)playable.GetInput(i);
            BEHAVIOUR input = playableInput.GetBehaviour();

            var time = playableInput.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            float inputWeight = playable.GetInputWeight(i);
            totalWeight += inputWeight;

            if (input.position && m_Track.trackPosition && m_MasterTrack.trackPosition)
            {
                positionTotalWeight += inputWeight;
                m_MixerData.position += input.GetPosition(tweenProgress, m_DefaultPosition) * inputWeight;
            }
            if (input.rotation && m_Track.trackRotation && m_MasterTrack.trackRotation)
            {
                rotationTotalWeight += inputWeight;
                m_MixerData.rotation += input.GetRotation(tweenProgress, m_DefaultRotation) * inputWeight;
            }
            if (input.scale && m_Track.trackScale && m_MasterTrack.trackScale)
            {
                scaleTotalWeight += inputWeight;
                m_MixerData.scale += input.GetScale(tweenProgress, m_DefaultScale) * inputWeight;
            }
        }


        bool relative = m_Track.relative;

        if (m_Track.trackPosition && isMasterTrackMixer) m_MixerData.position += m_DefaultPosition * (relative?1:(1f - positionTotalWeight));
        if (m_Track.trackRotation && isMasterTrackMixer) m_MixerData.rotation += m_DefaultRotation * (relative?1:(1f - rotationTotalWeight));
        if (m_Track.trackScale && isMasterTrackMixer) m_MixerData.scale += m_DefaultScale * (relative?1:(1f - scaleTotalWeight));

        m_MixerData.weight = totalWeight;

        ConvertPosition(ref m_MixerData.position);

        return ref m_MixerData;

    }
    protected override ref TransformTweenMixerData AddMixTrack(ref TransformTweenMixerData currentData, ref TransformTweenMixerData lastData)
    {
        currentData.position += lastData.position;
        currentData.rotation += lastData.rotation;
        currentData.scale += lastData.scale;
        return ref currentData;
    }
    protected override ref TransformTweenMixerData SubtracMixTrack(ref TransformTweenMixerData currentData, ref TransformTweenMixerData lastData)
    {
        currentData.position -= lastData.position;
        currentData.rotation -= lastData.rotation;
        currentData.scale -= lastData.scale;
        return ref currentData;
    }
    protected override ref TransformTweenMixerData AverageMixTrack(ref TransformTweenMixerData currentData, ref TransformTweenMixerData lastData)
    {
        if (lastData.weight > 0 && currentData.weight > 0)
        {
            currentData.position += lastData.position;
            currentData.position *= 0.5f;

            currentData.rotation += lastData.rotation;
            currentData.rotation *= 0.5f;

            currentData.scale += lastData.scale;
            currentData.scale *= 0.5f;
            return ref currentData;
        }
        if (currentData.weight > 0 && lastData.weight == 0) return ref currentData;
        return ref lastData;
    }
    protected override ref TransformTweenMixerData MultiplyMixTrack(ref TransformTweenMixerData currentData, ref TransformTweenMixerData lastData)
    {
        currentData.position = Vector3.Scale(currentData.position, lastData.position);
        currentData.rotation = Vector3.Scale(currentData.rotation, lastData.rotation);
        currentData.scale = Vector3.Scale(currentData.scale, lastData.scale);

        return ref currentData;
    }
    protected override ref TransformTweenMixerData OverrideMixTrack(ref TransformTweenMixerData currentData, ref TransformTweenMixerData lastData)
    {
        if (lastData.weight > 0)
        {
            if (m_Track.trackPosition) currentData.position = Vector3.Lerp(currentData.position, lastData.position, lastData.weight);
            if (m_Track.trackRotation) currentData.rotation = Vector3.Lerp(currentData.rotation, lastData.rotation, lastData.weight);
            if (m_Track.trackScale) currentData.scale = Vector3.Lerp(currentData.scale, lastData.scale, lastData.weight);
        }
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref TransformTweenMixerData processedData)
    {
        if (m_MasterTrack.trackPosition) { SetPosition(processedData.position); }

        if (m_MasterTrack.trackRotation) { SetRotation(processedData.rotation); }

        if (m_MasterTrack.trackScale) { SetScale(processedData.scale); }
    }
}