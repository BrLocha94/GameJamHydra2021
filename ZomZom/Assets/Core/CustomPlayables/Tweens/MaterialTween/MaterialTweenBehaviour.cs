using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class MaterialTweenBehaviour : PlayableTweenBehaviourBase
{
    [SerializeField] private List<MaterialFloatTweenParameter> m_FloatParameters = new List<MaterialFloatTweenParameter>();
    [SerializeField] private List<MaterialColorTweenParameter> m_ColorParameters = new List<MaterialColorTweenParameter>();
    [SerializeField] private List<MaterialVectorTweenParameter> m_VectorParameters = new List<MaterialVectorTweenParameter>();

    public void AddFloatParameter()
    {
        m_FloatParameters.Add(new MaterialFloatTweenParameter());
    }
    public void RemoveFloatParameter(int index)
    {
        m_FloatParameters.RemoveAt(index);
    }
    public void AddColorParameter()
    {
        m_ColorParameters.Add(new MaterialColorTweenParameter());
    }
    public void RemoveColorParameter(int index)
    {
        m_ColorParameters.RemoveAt(index);
    }
    public void AddVectorParameter()
    {
        m_VectorParameters.Add(new MaterialVectorTweenParameter());
    }
    public void RemoveVectorParameter(int index)
    {
        m_VectorParameters.RemoveAt(index);
    }

    public List<MaterialFloatTweenParameter> GetFloatParameters()
    {
        return m_FloatParameters;
    }
    public List<MaterialColorTweenParameter> GetColorParameters()
    {
        return m_ColorParameters;
    }
    public List<MaterialVectorTweenParameter> GetVectorParameters()
    {
        return m_VectorParameters;
    }
}

