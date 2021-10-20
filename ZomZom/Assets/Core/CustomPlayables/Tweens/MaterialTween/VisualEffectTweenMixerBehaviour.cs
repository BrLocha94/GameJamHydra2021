using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectTweenMixerBehaviour : MaterialTweenMixerBehaviourBase<VisualEffect, VisualEffectTweenTrack>
{
    protected override void ApplyProcessedData(ref MaterialTweenMixerData processedData)
    {
        foreach (var floatData in processedData.floatDataDict)
        {
            trackBinding.SetFloat(floatData.Key, floatData.Value.value);
        }
        foreach (var vectorData in processedData.vectorDataDict)
        {
            if(vectorData.Value.type == VectorData.EVectorType.Vector4)
            {
                trackBinding.SetVector4(vectorData.Key, vectorData.Value.vector);
            }
            else if(vectorData.Value.type == VectorData.EVectorType.Vector3)
            {
                trackBinding.SetVector3(vectorData.Key, vectorData.Value.vector);
            }
            else
            {
                trackBinding.SetVector2(vectorData.Key, vectorData.Value.vector);
            }
            
        }
    }

    protected override bool GetColorMaterialProperty(int id, out Color value)
    {
       throw new System.NotImplementedException();
    }

    protected override bool GetFloatMaterialProperty(int id, out float value)
    {
        throw new System.NotImplementedException();
    }

    protected override bool GetVectorMaterialProperty(int id, out Vector4 value)
    {
        throw new System.NotImplementedException();
    }
}
