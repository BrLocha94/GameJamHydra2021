using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MaterialTweenMixerBehaviour : MaterialTweenMixerBehaviourBase<Renderer, MaterialTweenTrack>
{
    protected int propertyID;
    private int m_MaterialIndex => m_MasterTrack.materialIndex;
    private bool m_ApplyToSharedMaterial => m_MasterTrack.applyToSharedMaterialAsset;

    //Material Property
    private MaterialPropertyBlock m_MainPropertyBlock;
    private MaterialPropertyBlock m_DefaultMainPropertyBlock;

    //SharedMaterial
    private Material sharedMaterial;

    protected override void GetDefaultValues()
    {
        base.GetDefaultValues();

        bool indexCondition()
        {
            return m_MaterialIndex >= 0 && m_MaterialIndex < trackBinding.sharedMaterials.Length;
        }
        processFrameConditions.Add(new Condition(indexCondition));

         m_MainPropertyBlock = new MaterialPropertyBlock();

         m_DefaultMainPropertyBlock = new MaterialPropertyBlock();

         trackBinding.GetPropertyBlock(m_DefaultMainPropertyBlock);

         m_MainPropertyBlock.Clear();
         trackBinding.SetPropertyBlock(m_MainPropertyBlock);
       
    }

    protected override void PreProcessTweenFrameMasterTrack()
    {
        base.PreProcessTweenFrameMasterTrack();



        if (m_ApplyToSharedMaterial)
        {
            sharedMaterial = trackBinding.sharedMaterials[m_MaterialIndex];

            if (m_DefaultMainPropertyBlock != null)
            {
                trackBinding.SetPropertyBlock(m_DefaultMainPropertyBlock);
            }
        }
        else
        {
            m_MainPropertyBlock.Clear();
            trackBinding.SetPropertyBlock(m_MainPropertyBlock, m_MaterialIndex);
        }
    }
    protected override void ApplyProcessedData(ref MaterialTweenMixerData processedData)
    {
        ApplyToMaterial(ref processedData, m_ApplyToSharedMaterial);
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (trackBinding != null)
        {
            bool isMaterialIndexValid = processFrameConditions.MetConditions();

            if (m_DefaultMainPropertyBlock != null && isMaterialIndexValid)
            {
                trackBinding.SetPropertyBlock(m_DefaultMainPropertyBlock, m_MaterialIndex);
            }

            if (sharedMaterial != null && isMaterialIndexValid)
            {
                foreach (var colorData in m_DefaultValue.colorDataDict)
                {
                    Color color = m_Track.clampColor ? colorData.Value.color.ClampMagnitude(1) : colorData.Value.color;
                    sharedMaterial.SetColor(colorData.Key, color);
                }
                foreach (var floatData in m_DefaultValue.floatDataDict)
                {
                    sharedMaterial.SetFloat(floatData.Key, floatData.Value.value);
                }
                foreach (var vectorData in m_DefaultValue.vectorDataDict)
                {
                    sharedMaterial.SetVector(vectorData.Key, vectorData.Value.vector);
                }
            }
        }
    }
    protected override bool GetVectorMaterialProperty(int id, out Vector4 value)
    {
        value = default(Vector4);
        Material mat = trackBinding.sharedMaterials[m_MaterialIndex];
        if (mat.HasProperty(id))
        {
            value = mat.GetVector(id);
            return true;
        }
        return false;
    }
    protected override bool GetFloatMaterialProperty(int id, out float value)
    {
        value = default(float);
        Material mat = trackBinding.sharedMaterials[m_MaterialIndex];
        if (mat.HasProperty(id))
        {
            value = mat.GetFloat(id);
            return true;
        }
        return false;
    }
    protected override bool GetColorMaterialProperty(int id, out Color value)
    {
        value = default(Color);
        Material mat = trackBinding.sharedMaterials[m_MaterialIndex];
        if (mat.HasProperty(id))
        {
            value = mat.GetColor(id);
            return true;
        }
        return false;
    }
    protected void ApplyToMaterial(ref MaterialTweenMixerData processedData, bool applyToShared)
    {

        MaterialTweenMixerBehaviour masterMixer = m_MasterTrack.mixerBehaviour as MaterialTweenMixerBehaviour;

        foreach (var colorData in processedData.colorDataDict)
        {
            Color color = m_Track.clampColor ? colorData.Value.color.ClampMagnitude(1) : colorData.Value.color;

            if (applyToShared)
            {
                masterMixer.sharedMaterial.SetColor(colorData.Key, color);
            }
            else
            {
                masterMixer.m_MainPropertyBlock.SetColor(colorData.Key, color);
            }

        }
        foreach (var floatData in processedData.floatDataDict)
        {
            if (applyToShared)
            {
                masterMixer.sharedMaterial.SetFloat(floatData.Key, floatData.Value.value);
            }
            else
            {
                masterMixer.m_MainPropertyBlock.SetFloat(floatData.Key, floatData.Value.value);
            }
        }
        foreach (var vectorData in processedData.vectorDataDict)
        {
            if (applyToShared)
            {
                masterMixer.sharedMaterial.SetVector(vectorData.Key, vectorData.Value.vector);
            }
            else
            {
                masterMixer.m_MainPropertyBlock.SetVector(vectorData.Key, vectorData.Value.vector);
            }
        }

        if (!applyToShared)
        {
            trackBinding.SetPropertyBlock(masterMixer.m_MainPropertyBlock, m_MaterialIndex);
        }
    }
}
