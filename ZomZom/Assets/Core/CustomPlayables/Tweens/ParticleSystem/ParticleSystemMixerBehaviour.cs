using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ParticleSystemMixerBehaviour : TweenMixerBehaviour<ParticleSystemTweenBehaviour, ParticleSystem, ParticleSystemTweenMixerData>
{
    protected ParticleSystem.Particle[] m_Particles;
    protected int currentAmount;
    protected int lastCurrentAmount = -1;
    protected float time { private set; get; }
    protected bool updateBehaviourValues = false;

    private  ParticleSystemTweenMixerData m_BlendedValue = new ParticleSystemTweenMixerData();

    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();
        var mainModule = trackBinding.main;

        trackBinding.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        mainModule.duration = (float)this.masterTrack.duration;
        mainModule.startLifetime = mainModule.duration;
    }
    protected override void PreProcessTweenFrameMasterTrack()
    {
        base.PreProcessTweenFrameMasterTrack();
        trackBinding.Simulate(time, true, true);
        m_Particles = new ParticleSystem.Particle[trackBinding.main.maxParticles];
        currentAmount = trackBinding.GetParticles(m_Particles);

        if (currentAmount != lastCurrentAmount)
        {
            updateBehaviourValues = true;
            lastCurrentAmount = currentAmount;
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            //m_TrackBinding.value = m_DefaultValue;
        }
    }
    protected override ref ParticleSystemTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        if (track == masterTrack) time = GetTime(playable);

        int inputCount = playable.GetInputCount();


        m_BlendedValue.position = new Vector3[currentAmount];
        m_BlendedValue.remainingLifetime = new float[currentAmount];

        for (int j = 0; j < currentAmount; j++)
        {
            for (int i = 0; i < inputCount; i++)
            {

                var playableInput = (ScriptPlayable<ParticleSystemTweenBehaviour>)playable.GetInput(i);
                ParticleSystemTweenBehaviour input = playableInput.GetBehaviour();

                if (updateBehaviourValues)
                {
                    input.particlesCount = currentAmount;
                    input.CalculateRandomValues();
                }

                int index = input.randomOrderList[j];
                int particlesGroup = Mathf.Clamp(input.particlesGroup, 1, currentAmount);
                var originalT = playableInput.GetTime();
                var calculatedDelay = input.delay * ((currentAmount - 1) / input.particlesGroup);
                var delayedTime = (originalT * (1 + calculatedDelay)) - input.delay * (index % (currentAmount / particlesGroup)) * input.clipDuration;

                playableInput.SetTime(delayedTime);

                var time = playableInput.GetTime();
                float normalizedTime = (float)(time / input.clipDuration);
                float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);
                float inputWeight = playable.GetInputWeight(i);

                bool evenlySpaced = true;
                float spacing = input.randomSpacing ? input.randomSpacingList[j] : (float)j / (float)(currentAmount - 1);

                m_BlendedValue.position[j] += input.GetStartEndValue(tweenProgress, spacing) * inputWeight;
                m_BlendedValue.remainingLifetime[j] += (float)this.masterTrack.duration - (tweenProgress * (float)input.clipDuration);
                playableInput.SetTime(originalT);
            }
        }

        //blendedValue.data += m_DefaultValue * (1f - valueTotalWeight);
        updateBehaviourValues = false;
        return ref m_BlendedValue;
    }
    protected override ref ParticleSystemTweenMixerData AddMixTrack(ref ParticleSystemTweenMixerData currentData, ref ParticleSystemTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.position.Length; i++)
        {
            currentData.position[i] += lastData.position[i];
        }

        return ref currentData;
    }
    protected override ref ParticleSystemTweenMixerData SubtracMixTrack(ref ParticleSystemTweenMixerData currentData, ref ParticleSystemTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.position.Length; i++)
        {
            currentData.position[i] -= lastData.position[i];
        }
        return ref currentData;
    }
    protected override ref ParticleSystemTweenMixerData MultiplyMixTrack(ref ParticleSystemTweenMixerData currentData, ref ParticleSystemTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.position.Length; i++)
        {
            currentData.position[i] = Vector3.Scale(currentData.position[i], lastData.position[i]);
        }
        return ref currentData;
    }
    protected override ref ParticleSystemTweenMixerData AverageMixTrack(ref ParticleSystemTweenMixerData currentData, ref ParticleSystemTweenMixerData lastData)
    {
        for (int i = 0; i < currentData.position.Length; i++)
        {
            currentData.position[i] += lastData.position[i];
            currentData.position[i] *= 0.5f;
        }
        return ref currentData;
    }
    protected override ref ParticleSystemTweenMixerData OverrideMixTrack(ref ParticleSystemTweenMixerData currentData, ref ParticleSystemTweenMixerData lastData)
    {
        return ref currentData;
    }
    protected override void ApplyProcessedData(ref ParticleSystemTweenMixerData processedData)
    {
        currentAmount = trackBinding.GetParticles(m_Particles);

        for (int i = 0; i < currentAmount; i++)
        {
            m_Particles[i].position = ConvertPosition(processedData.position[i]);
            m_Particles[i].startLifetime = (float)this.masterTrack.duration;
            m_Particles[i].remainingLifetime = Mathf.Max(processedData.remainingLifetime[i], 0.00001f);
        }


        trackBinding.SetParticles(m_Particles, currentAmount);


        //m_TrackBinding.value = processedData.data;
    }
    protected float GetTime(Playable playable)
    {
        int inputCount = playable.GetInputCount();
        float tweenProgress = 0;
        for (int i = 0; i < inputCount; i++)
        {
            var playableInput = (ScriptPlayable<ParticleSystemTweenBehaviour>)playable.GetInput(i);
            ParticleSystemTweenBehaviour input = playableInput.GetBehaviour();
            var time = playableInput.GetTime();
            tweenProgress = (float)time;
            /*float normalizedTime = (float)(time / input.clipDuration);
            tweenProgress = input.EvaluateCurrentCurve(normalizedTime);*/
            break;
        }

        return tweenProgress;
    }

    private Vector3 ConvertPosition(Vector3 position)
    {
        var track = masterTrack as ParticleSystemTweenTrack;

        if (track.convertPosition)
        {
            var screenPos = track.fromCamera.WorldToScreenPoint(position);
            return track.toCamera.ScreenToWorldPoint(screenPos);
        }
        return position;
    }
}