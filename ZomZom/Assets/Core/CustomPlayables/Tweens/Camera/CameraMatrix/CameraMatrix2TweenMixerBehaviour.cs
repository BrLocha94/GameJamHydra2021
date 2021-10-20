using System;
using UnityEngine;
using UnityEngine.Playables;

public class CameraMatrix2TweenMixerBehaviour : TweenMixerBehaviour<CameraMatrix2TweenBehaviour, Camera, CameraMatrix2TweenMixerData>
{
    protected CameraMatrix2TweenMixerData m_MixerData;
    protected CameraMatrix2TweenTrack m_Track
    {
        get
        {
            return track as CameraMatrix2TweenTrack;
        }
    }
    protected CameraMatrix2TweenTrack m_MasterTrack
    {
        get
        {
            return masterTrack as CameraMatrix2TweenTrack;
        }
    }
    protected override void ResetToDefaultValues()
    {
        base.ResetToDefaultValues();
        trackBinding.ResetWorldToCameraMatrix();
        trackBinding.ResetProjectionMatrix();
    }
    protected override void GetDefaultValues()
    {
        base.GetDefaultValues();
        defaultData.fieldOfView = trackBinding.fieldOfView;
    }

    protected override ref CameraMatrix2TweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        m_MixerData.transPos = Vector3.zero;
        m_MixerData.transRot = Vector3.zero;
        m_MixerData.transScale = Vector3.zero;
        m_MixerData.fieldOfView = 0;
        m_MixerData.projectionObliqueness = Vector2.zero;

        int inputCount = playable.GetInputCount();

        float transPosTotalWeight = 0f;
        float transRotTotalWeight = 0f;
        float transScaleTotalWeight = 0f;
        float projPosTotalWeight = 0f;
        float fieldOfViewTotalWeight = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<CameraMatrix2TweenBehaviour> playableInput = (ScriptPlayable<CameraMatrix2TweenBehaviour>)playable.GetInput(i);
            CameraMatrix2TweenBehaviour input = playableInput.GetBehaviour();

            var time = playableInput.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            float inputWeight = playable.GetInputWeight(i);
            totalWeight += inputWeight;

            if (input.position.enable && m_Track.trackPosition && m_MasterTrack.trackPosition)
            {
                transPosTotalWeight += inputWeight;
                m_MixerData.transPos += input.position.GetValue(tweenProgress) * inputWeight;
            }
            if (input.rotation.enable && m_Track.trackRotation && m_MasterTrack.trackRotation)
            {
                transRotTotalWeight += inputWeight;
                m_MixerData.transRot += input.rotation.GetValue(tweenProgress) * inputWeight;
            }
            if (input.scale.enable && m_Track.trackScale && m_MasterTrack.trackScale)
            {
                transScaleTotalWeight += inputWeight;
                m_MixerData.transScale += input.scale.GetValue(tweenProgress) * inputWeight;
            }
            if (input.projectionPosition.enable && m_Track.trackProjectionObliqueness && m_MasterTrack.trackProjectionObliqueness)
            {
                projPosTotalWeight += inputWeight;
                m_MixerData.projectionObliqueness += input.projectionPosition.GetValue(tweenProgress) * inputWeight;
            }
            if (input.fieldOfView.enable && m_Track.trackFOV && m_MasterTrack.trackFOV)
            {
                fieldOfViewTotalWeight += inputWeight;
                m_MixerData.fieldOfView += input.fieldOfView.GetValue(tweenProgress) * inputWeight;
            }
        }

        bool isMasterTrack = isMasterTrackMixer;

        if (m_Track.trackPosition && isMasterTrack) m_MixerData.transPos += defaultData.transPos * (1f - transPosTotalWeight);
        if (m_Track.trackRotation && isMasterTrack) m_MixerData.transRot += defaultData.transRot * (1f - transRotTotalWeight);
        if (m_Track.trackScale && isMasterTrack) m_MixerData.transScale += defaultData.transScale * (1f - transScaleTotalWeight);
        if (m_Track.trackProjectionObliqueness && isMasterTrack) m_MixerData.projectionObliqueness += defaultData.projectionObliqueness * (1f - projPosTotalWeight);
        if (m_Track.trackFOV && isMasterTrack) m_MixerData.fieldOfView += defaultData.fieldOfView * (1f - fieldOfViewTotalWeight);

        m_MixerData.weight = totalWeight;

        return ref m_MixerData;
    }
    protected override void ApplyProcessedData(ref CameraMatrix2TweenMixerData processedData)
    {
        #region Transformation Matrix

        Vector3 calculatedScale = processedData.transScale;

        Matrix4x4 transformationMatrix = Matrix4x4.Rotate(Quaternion.Euler(processedData.transRot));
        calculatedScale += Vector3.one;
        calculatedScale = Vector3.Scale(calculatedScale, new Vector3(1, 1, -1));
        if (Vector3.Magnitude(calculatedScale) > 0) transformationMatrix *= Matrix4x4.Scale(calculatedScale);
        else transformationMatrix *= Matrix4x4.Scale(new Vector3(1, 1, -1));

        transformationMatrix *= Matrix4x4.Translate(processedData.transPos);

        if (transformationMatrix.ValidTRS())
        {

            trackBinding.worldToCameraMatrix = transformationMatrix * trackBinding.transform.worldToLocalMatrix;
        }
        else
        {
            trackBinding.ResetWorldToCameraMatrix();
        }

        #endregion
        #region Projection Matrix

        float nearClip = trackBinding.nearClipPlane;
        float farClip = trackBinding.farClipPlane;
        float aspect = trackBinding.aspect;
        float fieldOfView = processedData.fieldOfView;

        Matrix4x4 projectionMaxtrix = Matrix4x4.Perspective(fieldOfView,
             aspect, nearClip, farClip);

        Vector2 offset = processedData.projectionObliqueness / Mathf.PI;
        projectionMaxtrix[0, 2] = offset.x;
        projectionMaxtrix[1, 2] = offset.y;
        trackBinding.projectionMatrix = projectionMaxtrix;

        #endregion
    }
    protected override ref CameraMatrix2TweenMixerData AddMixTrack(ref CameraMatrix2TweenMixerData currentData, ref CameraMatrix2TweenMixerData lastData)
    {
        currentData.transPos += lastData.transPos;
        currentData.transRot += lastData.transRot;
        currentData.transScale += lastData.transScale;
        currentData.projectionObliqueness += lastData.projectionObliqueness;
        currentData.fieldOfView += lastData.fieldOfView;
        return ref currentData;
    }
    protected override ref CameraMatrix2TweenMixerData SubtracMixTrack(ref CameraMatrix2TweenMixerData currentData, ref CameraMatrix2TweenMixerData lastData)
    {
        currentData.transPos -= lastData.transPos;
        currentData.transRot -= lastData.transRot;
        currentData.transScale -= lastData.transScale;
        currentData.projectionObliqueness -= lastData.projectionObliqueness;
        currentData.fieldOfView -= lastData.fieldOfView;
        return ref currentData;
    }
    protected override ref CameraMatrix2TweenMixerData AverageMixTrack(ref CameraMatrix2TweenMixerData currentData, ref CameraMatrix2TweenMixerData lastData)
    {
        if (lastData.weight > 0 && currentData.weight > 0)
        {
            currentData.transPos += lastData.transPos;
            currentData.transPos *= 0.5f;

            currentData.transRot += lastData.transRot;
            currentData.transRot *= 0.5f;

            currentData.transScale += lastData.transScale;
            currentData.transScale *= 0.5f;

            currentData.projectionObliqueness += lastData.projectionObliqueness;
            currentData.projectionObliqueness *= 0.5f;

            currentData.fieldOfView += lastData.fieldOfView;
            currentData.fieldOfView *= 0.5f;

            return ref currentData;
        }
        if (currentData.weight > 0 && lastData.weight == 0) return ref currentData;
        return ref lastData;
    }
    protected override ref CameraMatrix2TweenMixerData MultiplyMixTrack(ref CameraMatrix2TweenMixerData currentData, ref CameraMatrix2TweenMixerData lastData)
    {
        currentData.transPos = Vector3.Scale(currentData.transPos, lastData.transPos);
        currentData.transRot = Vector3.Scale(currentData.transRot, lastData.transRot);
        currentData.transScale = Vector3.Scale(currentData.transScale, lastData.transScale);
        currentData.projectionObliqueness = Vector2.Scale(currentData.projectionObliqueness, lastData.projectionObliqueness);
        currentData.fieldOfView *= lastData.fieldOfView;

        return ref currentData;
    }
    protected override ref CameraMatrix2TweenMixerData OverrideMixTrack(ref CameraMatrix2TweenMixerData currentData, ref CameraMatrix2TweenMixerData lastData)
    {
        if (lastData.weight > 0)
        {
            float weight = lastData.weight;
            if (m_Track.trackPosition) currentData.transPos = Vector3.Lerp(currentData.transPos, lastData.transPos, weight);
            if (m_Track.trackRotation) currentData.transRot = Vector3.Lerp(currentData.transRot, lastData.transRot, weight);
            if (m_Track.trackScale) currentData.transScale = Vector3.Lerp(currentData.transScale, lastData.transScale, weight);
            if (m_Track.trackProjectionObliqueness) currentData.projectionObliqueness = Vector3.Lerp(currentData.projectionObliqueness, lastData.projectionObliqueness, weight);          
            if (m_Track.trackFOV) currentData.fieldOfView = Mathf.Lerp(currentData.fieldOfView, lastData.fieldOfView, weight);
        }
        return ref currentData;
    }
}