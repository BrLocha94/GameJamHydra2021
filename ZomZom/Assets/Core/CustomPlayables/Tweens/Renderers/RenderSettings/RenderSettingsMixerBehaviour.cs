using UnityEngine;
using UnityEngine.Playables;

public class RenderSettingsMixerBehaviour : TweenMixerBehaviour<RenderSettingsBehaviour, Object, RenderSettingsTweenMixerData>
{
    RenderSettingsTweenMixerData blendedRenderSettings;
    RenderSettingsTweenMixerData lastRenderSettingsValue;
    public override bool useTrackBinding => false;

    private double lastTime;

    private RenderSettingsTrack m_MasterTrack
    {
        get
        {
            return masterTrack as RenderSettingsTrack;
        }
    }

    protected override void GetDefaultValues()
    {
        base.GetDefaultValues();
        defaultData.ambientSky = RenderSettings.ambientSkyColor;
        defaultData.ambientEquator = RenderSettings.ambientEquatorColor;
        defaultData.ambientGround = RenderSettings.ambientGroundColor;
        defaultData.ambientLight = RenderSettings.ambientLight;
        defaultData.ambientIntensity = RenderSettings.ambientIntensity;
        defaultData.reflectionIntensity = RenderSettings.reflectionIntensity;
    }
    protected override ref RenderSettingsTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {

        int inputCount = playable.GetInputCount();

        blendedRenderSettings.ambientSky = Color.clear;
        blendedRenderSettings.ambientEquator = Color.clear;
        blendedRenderSettings.ambientGround = Color.clear;
        blendedRenderSettings.ambientLight = Color.clear;
        blendedRenderSettings.ambientIntensity = 0f;
        blendedRenderSettings.reflectionIntensity = 0f;

        float ambientSkyWeight = 0f;
        float ambientEquatorWeight = 0f;
        float ambientGroundWeight = 0f;
        float ambientLightWeight = 0f;
        float ambientIntensityWeight = 0f;
        float reflectionIntensityWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<RenderSettingsBehaviour> inputPlayable = (ScriptPlayable<RenderSettingsBehaviour>)playable.GetInput(i);
            RenderSettingsBehaviour input = inputPlayable.GetBehaviour();

            var time = inputPlayable.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            if (input.skyColorGradientTweenParameter.enable)
            {
                blendedRenderSettings.ambientSky += input.skyColorGradientTweenParameter.GetValue(tweenProgress) * inputWeight;
                ambientSkyWeight += inputWeight;
            }
            if (input.equatorColorGradientTweenParameter.enable)
            {
                blendedRenderSettings.ambientEquator += input.equatorColorGradientTweenParameter.GetValue(tweenProgress) * inputWeight;
                ambientEquatorWeight += inputWeight;
            }
            if (input.groundColorGradientTweenParameter.enable)
            {
                blendedRenderSettings.ambientGround += input.groundColorGradientTweenParameter.GetValue(tweenProgress) * inputWeight;
                ambientGroundWeight += inputWeight;
            }
            if (input.ambientColorGradientTweenParameter.enable)
            {
                blendedRenderSettings.ambientLight += input.ambientColorGradientTweenParameter.GetValue(tweenProgress) * inputWeight;
                ambientLightWeight += inputWeight;
            }
            if (input.reflectionIntensity.enable)
            {
                blendedRenderSettings.reflectionIntensity += input.reflectionIntensity.GetValue(tweenProgress) * inputWeight;
                reflectionIntensityWeight += inputWeight;
            }
            if (input.ambientIntensity.enable)
            {
                blendedRenderSettings.ambientIntensity += input.ambientIntensity.GetValue(tweenProgress) * inputWeight;
                ambientIntensityWeight += inputWeight;
            }
        }

        blendedRenderSettings.ambientSky += defaultData.ambientSky * (1f - ambientSkyWeight);
        blendedRenderSettings.ambientEquator += defaultData.ambientEquator * (1f - ambientEquatorWeight);
        blendedRenderSettings.ambientGround += defaultData.ambientGround * (1f - ambientGroundWeight);
        blendedRenderSettings.ambientLight += defaultData.ambientLight * (1f - ambientLightWeight);
        blendedRenderSettings.ambientIntensity += defaultData.ambientIntensity * (1f - ambientIntensityWeight);
        blendedRenderSettings.reflectionIntensity += defaultData.reflectionIntensity * (1f - reflectionIntensityWeight);

        return ref blendedRenderSettings;
    }
    protected override void ApplyProcessedData(ref RenderSettingsTweenMixerData processedData)
    {
        if(!DynamicGI.isConverged)return;

        if (SettingsHasChanged(ref processedData))
        {
            RenderSettings.ambientSkyColor = processedData.ambientSky;
            RenderSettings.ambientEquatorColor = processedData.ambientEquator;
            RenderSettings.ambientGroundColor = processedData.ambientGround;
            RenderSettings.ambientLight = processedData.ambientLight;
            RenderSettings.ambientIntensity = processedData.ambientIntensity;
            RenderSettings.reflectionIntensity = processedData.reflectionIntensity;
            DynamicGI.UpdateEnvironment();
            lastRenderSettingsValue = processedData;
        }
    }
    private bool SettingsHasChanged(ref RenderSettingsTweenMixerData processedData)
    {
        if (processedData.ambientEquator != lastRenderSettingsValue.ambientEquator) return true;
        if (processedData.ambientGround != lastRenderSettingsValue.ambientGround) return true;
        if (processedData.ambientIntensity != lastRenderSettingsValue.ambientIntensity) return true;
        if (processedData.ambientLight != lastRenderSettingsValue.ambientLight) return true;
        if (processedData.ambientSky != lastRenderSettingsValue.ambientSky) return true;
        if (processedData.reflectionIntensity != lastRenderSettingsValue.reflectionIntensity) return true;
        return false;
    }
    protected override ref RenderSettingsTweenMixerData AddMixTrack(ref RenderSettingsTweenMixerData currentData, ref RenderSettingsTweenMixerData lastData)
    {
        currentData.ambientSky += lastData.ambientSky;
        currentData.ambientEquator += lastData.ambientEquator;
        currentData.ambientGround += lastData.ambientGround;
        currentData.ambientLight += lastData.ambientLight;
        currentData.ambientIntensity += lastData.ambientIntensity;
        currentData.reflectionIntensity += lastData.reflectionIntensity;
        return ref currentData;
    }
    protected override ref RenderSettingsTweenMixerData AverageMixTrack(ref RenderSettingsTweenMixerData currentData, ref RenderSettingsTweenMixerData lastData)
    {
        currentData.ambientSky += lastData.ambientSky;
        currentData.ambientEquator += lastData.ambientEquator;
        currentData.ambientGround += lastData.ambientGround;
        currentData.ambientLight += lastData.ambientLight;
        currentData.ambientIntensity += lastData.ambientIntensity;
        currentData.reflectionIntensity += lastData.reflectionIntensity;

        currentData.ambientSky *= 0.5f;
        currentData.ambientEquator *= 0.5f;
        currentData.ambientGround *= 0.5f;
        currentData.ambientLight *= 0.5f;
        currentData.ambientIntensity *= 0.5f;
        currentData.reflectionIntensity *= 0.5f;

        return ref currentData;
    }
    protected override ref RenderSettingsTweenMixerData MultiplyMixTrack(ref RenderSettingsTweenMixerData currentData, ref RenderSettingsTweenMixerData lastData)
    {
        currentData.ambientSky *= lastData.ambientSky;
        currentData.ambientEquator *= lastData.ambientEquator;
        currentData.ambientGround *= lastData.ambientGround;
        currentData.ambientLight *= lastData.ambientLight;
        currentData.ambientIntensity *= lastData.ambientIntensity;
        currentData.reflectionIntensity *= lastData.reflectionIntensity;
        return ref currentData;
    }
    protected override ref RenderSettingsTweenMixerData OverrideMixTrack(ref RenderSettingsTweenMixerData currentData, ref RenderSettingsTweenMixerData lastData)
    {
        return ref lastData;
    }
    protected override ref RenderSettingsTweenMixerData SubtracMixTrack(ref RenderSettingsTweenMixerData currentData, ref RenderSettingsTweenMixerData lastData)
    {
        currentData.ambientSky -= lastData.ambientSky;
        currentData.ambientEquator -= lastData.ambientEquator;
        currentData.ambientGround -= lastData.ambientGround;
        currentData.ambientLight -= lastData.ambientLight;
        currentData.ambientIntensity -= lastData.ambientIntensity;
        currentData.reflectionIntensity -= lastData.reflectionIntensity;

        return ref currentData;
    }

    protected override void ResetToDefaultValues()
    {
        base.ResetToDefaultValues();

        RenderSettings.ambientSkyColor = defaultData.ambientSky;
        RenderSettings.ambientEquatorColor = defaultData.ambientEquator;
        RenderSettings.ambientGroundColor = defaultData.ambientGround;
        RenderSettings.ambientLight = defaultData.ambientLight;
        RenderSettings.ambientIntensity = defaultData.ambientIntensity;
        RenderSettings.reflectionIntensity = defaultData.reflectionIntensity;
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        DynamicGI.UpdateEnvironment();
    }
}
