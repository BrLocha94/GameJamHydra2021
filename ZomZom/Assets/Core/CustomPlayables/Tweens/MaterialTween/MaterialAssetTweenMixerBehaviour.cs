using UnityEngine;
using UnityEngine.Playables;

public class MaterialAssetTweenMixerBehaviour : MaterialTweenMixerBehaviourBase<Material, MaterialAssetTweenTrack>
{
    protected Material material;
    protected override void OnFirstFrame()
    {
        base.OnFirstFrame();
        material = trackBinding;
    }
    protected override void ApplyProcessedData(ref MaterialTweenMixerData processedData)
    {
        foreach (var colorData in processedData.colorDataDict)
        {
            Color color = m_Track.clampColor ? colorData.Value.color.ClampMagnitude(1) : colorData.Value.color;
            material.SetColor(colorData.Key, color);
        }
        foreach (var floatData in processedData.floatDataDict)
        {
            material.SetFloat(floatData.Key, floatData.Value.value);
        }
        foreach (var vectorData in processedData.vectorDataDict)
        {
            material.SetVector(vectorData.Key, vectorData.Value.vector);
        }
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null && postplaybackResetToDefault)
        {
            foreach (var colorData in m_DefaultValue.colorDataDict)
            {
                material.SetColor(colorData.Key, colorData.Value.defaultColor);
            }
            foreach (var floatData in m_DefaultValue.floatDataDict)
            {
                material.SetFloat(floatData.Key, floatData.Value.defaultValue);
            }
            foreach (var vectorData in m_DefaultValue.vectorDataDict)
            {
                material.SetVector(vectorData.Key, vectorData.Value.defaultVector);
            }
        }
    }    

    protected override bool GetVectorMaterialProperty(int id, out Vector4 value)
    {
        value = default(Vector4);

        if(material.HasProperty(id))
        {
            value = material.GetVector(id);
            return true;
        }
        return false;
    }
    protected override bool GetFloatMaterialProperty(int id, out float value)
    {
        value = default(float);

        if(material.HasProperty(id))
        {
            value = material.GetFloat(id);
            return true;
        }
        return false;
    }
    protected override bool GetColorMaterialProperty(int id, out Color value)
    {
        value = default(Color);

        if(material.HasProperty(id))
        {
            value = material.GetColor(id);
            return true;
        }
        return false;
    }
}