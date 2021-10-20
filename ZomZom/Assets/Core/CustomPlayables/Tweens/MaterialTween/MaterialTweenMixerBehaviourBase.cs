using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class MaterialTweenMixerBehaviourBase<COMPONENT, TRACK> : TweenMixerBehaviour<MaterialTweenBehaviour, COMPONENT, MaterialTweenMixerData>
    where COMPONENT : Object
    where TRACK : PlaybleTweenTrack<MaterialTweenBehaviour, COMPONENT, MaterialTweenMixerData>
{
    private MaterialTweenMixerData m_BlendedValue = new MaterialTweenMixerData();
    protected MaterialTweenMixerData m_DefaultValue = new MaterialTweenMixerData();
    public MaterialTweenMixerData blendeValue => m_BlendedValue;

    protected TRACK m_Track => track as TRACK;
    protected TRACK m_MasterTrack => masterTrack as TRACK;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);

        m_BlendedValue.colorDataDict = new Dictionary<int, ColorData>();
        m_BlendedValue.floatDataDict = new Dictionary<int, FloatData>();
        m_BlendedValue.vectorDataDict = new Dictionary<int, VectorData>();

        m_DefaultValue.colorDataDict = new Dictionary<int, ColorData>();
        m_DefaultValue.floatDataDict = new Dictionary<int, FloatData>();
        m_DefaultValue.vectorDataDict = new Dictionary<int, VectorData>();

         processedData.colorDataDict = new Dictionary<int, ColorData>();
        processedData.floatDataDict = new Dictionary<int, FloatData>();
        processedData.vectorDataDict = new Dictionary<int, VectorData>();
    }

    protected override ref MaterialTweenMixerData ProcessTweenFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        float valueTotalWeight = 0f;

        m_BlendedValue.weight = 0;

        foreach (var item in m_BlendedValue.colorDataDict)
        {
            var data = item.Value;
            data.color = Color.clear;
        }
        foreach (var item in m_BlendedValue.floatDataDict)
        {
            var data = item.Value;
            data.value = 0;
        }
        foreach (var item in m_BlendedValue.vectorDataDict)
        {
            var data = item.Value;
            data.vector = Vector4.zero;
        }

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<MaterialTweenBehaviour> playableInput = (ScriptPlayable<MaterialTweenBehaviour>)playable.GetInput(i);
            var input = playableInput.GetBehaviour();

            float inputWeight = playable.GetInputWeight(i);

            var time = playableInput.GetTime();
            float normalizedTime = (float)(time / input.clipDuration);
            float tweenProgress = input.EvaluateCurrentCurve(normalizedTime);

            valueTotalWeight += inputWeight;

            var colors = input.GetColorParameters();
            var floats = input.GetFloatParameters();
            var vectors = input.GetVectorParameters();

            for (int j = 0; j < colors.Count; j++)
            {
                var cProperty = colors[j];
                var value = cProperty.GetValue(tweenProgress);
                if (m_BlendedValue.colorDataDict.ContainsKey(cProperty.propertyID))
                    m_BlendedValue.colorDataDict[cProperty.propertyID].Add(value * inputWeight);
                else
                {
                    m_BlendedValue.colorDataDict.Add(cProperty.propertyID, new ColorData() { propertyID = cProperty.propertyID, color = value * inputWeight });

                    if (!m_DefaultValue.colorDataDict.ContainsKey(cProperty.propertyID))
                    {
                        if (GetColorMaterialProperty(cProperty.propertyID, out Color c))
                        {
                            m_DefaultValue.colorDataDict.Add(cProperty.propertyID, new ColorData() { color = c, defaultColor = value, propertyID = cProperty.propertyID });
                        }
                    }
                }

            }

            for (int j = 0; j < floats.Count; j++)
            {
                var fProperty = floats[j];
                var value = fProperty.GetValue(tweenProgress);
                if (m_BlendedValue.floatDataDict.ContainsKey(fProperty.propertyID))
                    m_BlendedValue.floatDataDict[fProperty.propertyID].Add(value * inputWeight);
                else
                {
                    m_BlendedValue.floatDataDict.Add(fProperty.propertyID, new FloatData() { propertyID = fProperty.propertyID, value = value * inputWeight });

                    if (!m_DefaultValue.floatDataDict.ContainsKey(fProperty.propertyID))
                    {
                        if (GetFloatMaterialProperty(fProperty.propertyID, out float f))
                        {
                            m_DefaultValue.floatDataDict.Add(fProperty.propertyID, new FloatData() { value = f, defaultValue = f, propertyID = fProperty.propertyID });
                        }
                    }
                }

            }

            for (int j = 0; j < vectors.Count; j++)
            {
                var vProperty = vectors[j];
                var value = vProperty.GetValue(tweenProgress);
                if (m_BlendedValue.vectorDataDict.ContainsKey(vProperty.propertyID))
                    m_BlendedValue.vectorDataDict[vProperty.propertyID].Add(value * inputWeight);
                else
                {
                    m_BlendedValue.vectorDataDict.Add(vProperty.propertyID, new VectorData() { propertyID = vProperty.propertyID, vector = value * inputWeight });

                    if (!m_DefaultValue.vectorDataDict.ContainsKey(vProperty.propertyID))
                    {
                        if (GetVectorMaterialProperty(vProperty.propertyID, out Vector4 v))
                        {
                            m_DefaultValue.vectorDataDict.Add(vProperty.propertyID, new VectorData() { vector = v, defaultVector = v, type = VectorData.EVectorType.Vector4, propertyID = vProperty.propertyID });
                        }
                    }
                }

            }

        }
        m_BlendedValue.weight = valueTotalWeight;
        return ref m_BlendedValue;
    }
    protected override ref MaterialTweenMixerData AddMixTrack(ref MaterialTweenMixerData currentData, ref MaterialTweenMixerData lastData)
    {
        foreach (var item in currentData.colorDataDict)
        {
            int key = item.Key;
            ColorData data;
            if (lastData.colorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.color += data.color;
            }
        }
        foreach (var item in currentData.floatDataDict)
        {
            int key = item.Key;
            FloatData data;
            if (lastData.floatDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.value += data.value;
            }
        }
        foreach (var item in currentData.vectorDataDict)
        {
            int key = item.Key;
            VectorData data;
            if (lastData.vectorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.vector += data.vector;
            }
        }

        return ref currentData;
    }
    protected override ref MaterialTweenMixerData SubtracMixTrack(ref MaterialTweenMixerData currentData, ref MaterialTweenMixerData lastData)
    {
        foreach (var item in currentData.colorDataDict)
        {
            int key = item.Key;
            ColorData data;
            if (lastData.colorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.color -= data.color;
            }
        }
        foreach (var item in currentData.floatDataDict)
        {
            int key = item.Key;
            FloatData data;
            if (lastData.floatDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.value -= data.value;
            }
        }
        foreach (var item in currentData.vectorDataDict)
        {
            int key = item.Key;
            VectorData data;
            if (lastData.vectorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.vector -= data.vector;
            }
        }

        return ref currentData;
    }
    protected override ref MaterialTweenMixerData MultiplyMixTrack(ref MaterialTweenMixerData currentData, ref MaterialTweenMixerData lastData)
    {
        foreach (var item in currentData.colorDataDict)
        {
            int key = item.Key;
            ColorData data;
            if (lastData.colorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.color = Vector4.Scale(colorData.color, data.color);
            }
        }
        foreach (var item in currentData.floatDataDict)
        {
            int key = item.Key;
            FloatData data;
            if (lastData.floatDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.value *= data.value;
            }
        }
        foreach (var item in currentData.vectorDataDict)
        {
            int key = item.Key;
            VectorData data;
            if (lastData.vectorDataDict.TryGetValue(key, out data))
            {
                var colorData = item.Value;
                colorData.vector = Vector4.Scale(colorData.vector, data.vector);
            }
        }

        return ref currentData;
    }
    protected override ref MaterialTweenMixerData AverageMixTrack(ref MaterialTweenMixerData currentData, ref MaterialTweenMixerData lastData)
    {
        if (lastData.weight > 0 && currentData.weight > 0)
        {

            foreach (var item in currentData.colorDataDict)
            {
                int key = item.Key;
                ColorData data;
                if (lastData.colorDataDict.TryGetValue(key, out data))
                {
                    var colorData = item.Value;
                    colorData.color += data.color;
                    colorData.color *= 0.5f;
                }
            }
            foreach (var item in currentData.floatDataDict)
            {
                int key = item.Key;
                FloatData data;
                if (lastData.floatDataDict.TryGetValue(key, out data))
                {
                    var colorData = item.Value;
                    colorData.value += data.value;
                    colorData.value *= 0.5f;
                }
            }
            foreach (var item in currentData.vectorDataDict)
            {
                int key = item.Key;
                VectorData data;
                if (lastData.vectorDataDict.TryGetValue(key, out data))
                {
                    var colorData = item.Value;
                    colorData.vector += data.vector;
                    colorData.vector *= 0.5f;
                }
            }

            return ref currentData;
        }

        if (currentData.weight > 0 && lastData.weight == 0) return ref currentData;
        return ref lastData;
    }
    protected override ref MaterialTweenMixerData OverrideMixTrack(ref MaterialTweenMixerData currentData, ref MaterialTweenMixerData lastData)
    {
        if (lastData.weight > 0) return ref lastData;
        return ref currentData;
    }

    protected abstract bool GetVectorMaterialProperty(int id, out Vector4 value);
    protected abstract bool GetFloatMaterialProperty(int id, out float value);
    protected abstract bool GetColorMaterialProperty(int id, out Color value);

}
