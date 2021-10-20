using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum ETweenMixerDataMode : int { Add = 0, Subtrac = 1, Multiply = 2, Average = 3, Override = 4 }
public abstract class TweenMixerBehaviour<BEHAVIOUR, COMPONENT, MIXERDATA> : PlayableBehaviour
    where BEHAVIOUR : PlayableCallbackBehaviour, new()
    where COMPONENT : class
    where MIXERDATA : ITweenMixerData
{
    protected bool m_FirstFrameHappened;
    protected COMPONENT trackBinding;
    protected MIXERDATA defaultData;
    protected MIXERDATA processedData;

    protected Conditions processFrameConditions = new Conditions();
    public virtual bool useTrackBinding => true;
    public bool isSubMixer => layerIndex != -1;
    public bool isMasterTrackMixer => track == masterTrack;

    protected bool postplaybackResetToDefault => masterTrack.postplaybackResetToDefault;
    public int layerIndex;
    protected ETweenMixerDataMode mixerDataMode => track.mixerDataMode;
    public PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> masterTrack;
    public PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> track;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {

        if (trackBinding == null && useTrackBinding)
        {
            trackBinding = playerData as COMPONENT;
            if (trackBinding == null) return;
        }

        if (!m_FirstFrameHappened)
        {
            if (isMasterTrackMixer) GetDefaultValues();
            OnFirstFrame();
            m_FirstFrameHappened = true;
        }

        if (!processFrameConditions.MetConditions()) return;

        if (isSubMixer)
        {
            PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> previousTrack;

            if (masterTrack.GetPreviousValidTrack(layerIndex, out previousTrack))
            {
                processedData = previousTrack.mixerBehaviour.processedData;
            }
            else
            {
                processedData = masterTrack.mixerBehaviour.processedData;
            }

            var currentData = ProcessTweenFrame(playable, info, playerData);

            processedData = MixTrackData(ref processedData, ref currentData);

            var lastValidTrack = masterTrack.GetLastValidTrack();

            if (lastValidTrack == null || layerIndex == lastValidTrack.mixerBehaviour.layerIndex)
            {
                ApplyProcessedData(ref processedData);
            }
        }
        else
        {
            PreProcessTweenFrameMasterTrack();
            processedData = ProcessTweenFrame(playable, info, playerData);
            if (!masterTrack.HasValidSubTracks()) ApplyProcessedData(ref processedData);
        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        m_FirstFrameHappened = false;

        if (isMasterTrackMixer && trackBinding != null && postplaybackResetToDefault)
        {
            ResetToDefaultValues();
        }
    }
    protected abstract ref MIXERDATA ProcessTweenFrame(Playable playable, FrameData info, object playerData);
    protected abstract ref MIXERDATA AddMixTrack(ref MIXERDATA currentData, ref MIXERDATA lastData);
    protected abstract ref MIXERDATA SubtracMixTrack(ref MIXERDATA currentData, ref MIXERDATA lastData);
    protected abstract ref MIXERDATA MultiplyMixTrack(ref MIXERDATA currentData, ref MIXERDATA lastData);
    protected abstract ref MIXERDATA AverageMixTrack(ref MIXERDATA currentData, ref MIXERDATA lastData);
    protected abstract ref MIXERDATA OverrideMixTrack(ref MIXERDATA currentData, ref MIXERDATA lastData);
    protected abstract void ApplyProcessedData(ref MIXERDATA processedData);
    protected ref MIXERDATA MixTrackData(ref MIXERDATA currentData, ref MIXERDATA lastData)
    {
        switch (mixerDataMode)
        {
            case ETweenMixerDataMode.Subtrac: return ref SubtracMixTrack(ref currentData, ref lastData);
            case ETweenMixerDataMode.Multiply: return ref MultiplyMixTrack(ref currentData, ref lastData);
            case ETweenMixerDataMode.Average: return ref AverageMixTrack(ref currentData, ref lastData);
            case ETweenMixerDataMode.Override: return ref OverrideMixTrack(ref currentData, ref lastData);
            default: return ref AddMixTrack(ref currentData, ref lastData);
        }
    }
    protected virtual void OnFirstFrame() { }
    protected virtual void GetDefaultValues() { }
    protected virtual void ResetToDefaultValues() { }
    protected virtual void PreProcessTweenFrameMasterTrack() { }
}

public interface ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }
}
public struct TweenMixerData<T> : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }

    public T data;
}
public struct LightMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }

    public float intensity;
    public Color color;
}
public struct TransformTweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public Vector3 position;
    public Vector3 rotation;//Euler angle rotation
    public Vector3 scale;
    public float weight { get; set; }
}
public struct TMPTweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public Vector3[] position;
    public Vector3[] rotation;
    public Vector3[] scale;
    public Vector2[] pivotOffset;
    public Color[] color;
    public float weight { get; set; }
}
public struct ParticleSystemTweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public Vector3[] position;
    public float[] remainingLifetime;
    public float lifeTime;
    public float weight { get; set; }
}
public struct LineRendererTweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }
    public Vector3[] position;
    public Color startColor;
    public Color endColor;
}
public struct MaterialTweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public Dictionary<int, ColorData> colorDataDict;
    public Dictionary<int, VectorData> vectorDataDict;
    public Dictionary<int, FloatData> floatDataDict;
    public float weight { get; set; }
}
public struct LightTweenMixerData : ITweenMixerData
{
    internal float intensity;
    internal Color color;
    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }
}
public struct RenderSettingsTweenMixerData : ITweenMixerData
{
    public Color ambientSky;
    public Color ambientEquator;
    public Color ambientGround;
    public Color ambientLight;
    public float ambientIntensity;
    public float reflectionIntensity;

    public ETweenMixerDataMode mixerMode { get; set; }
    public float weight { get; set; }
}
public struct CameraMatrix2TweenMixerData : ITweenMixerData
{
    public ETweenMixerDataMode mixerMode { get; set; }
    public Vector3 transPos;
    public Vector3 transRot;
    public Vector3 transScale;
    public Vector2 projectionObliqueness;
    public float fieldOfView;
    public float weight { get; set; }
}