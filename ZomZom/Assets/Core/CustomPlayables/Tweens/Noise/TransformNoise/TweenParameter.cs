using BezierSolution;
using JazzDev.Easing;
using JazzDev.Noiser;
using System;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TweenParameter
{
    public enum ETweenType : int { Fixed = 0, Ease = 1, AnimationCurve = 2 }

    [SerializeField] private AnimationCurve m_Curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private ETweenType m_Type;
    [SerializeField] private EaseStyle m_Ease = EaseStyle.Linear;
    [SerializeField] private float m_Value;
    [SerializeField] private float m_Multiplier = 1;
    private Ease ease;

    public TweenParameter(ETweenType type = ETweenType.Fixed, float fixedValue = 0, EaseStyle ease = EaseStyle.Linear)
    {
        m_Type = type;
        m_Value = fixedValue;
        m_Ease = ease;
        m_Multiplier = 1;
    }
    public virtual float GetValue(float t)
    {
        switch (m_Type)
        {
            case ETweenType.Fixed: return m_Value * m_Multiplier;
            case ETweenType.Ease: return EaseMethods.EasedLerp(EaseMethods.GetEase(m_Ease), 0, 1, t) * m_Multiplier;
            case ETweenType.AnimationCurve: return m_Curve.Evaluate(t) * m_Multiplier;
            default: return m_Value * m_Multiplier;
        }
    }

}

[System.Serializable]
public class NoisedTweenParameter : TweenParameter
{
    [SerializeField] protected bool m_NoiseMode = false;
    [SerializeField] protected NoiseChannelConfig m_NoiseChannelConfig = new NoiseChannelConfig();

    public override float GetValue(float t)
    {
        if (!m_NoiseMode) return base.GetValue(t);

        float multiplier = m_NoiseChannelConfig.multiplier;

        return m_NoiseChannelConfig.enable ?
               NoiserGeneretor.GetNoise(t * m_NoiseChannelConfig.frequency.GetValue(t) * multiplier,
               m_NoiseChannelConfig.amplitude.GetValue(t) * multiplier,
               m_NoiseChannelConfig.valueCenter.GetValue(t),
               m_NoiseChannelConfig.offset) : 0;
    }
}

[System.Serializable]
public class ValueTweenParameterBase<T>
{
    [SerializeField] protected bool m_Enable = false;
    [SerializeField] protected bool m_IndividualChannels = false;
    [SerializeField] protected TweenParameter m_Tween = new TweenParameter(TweenParameter.ETweenType.Ease, 1);
    [SerializeField] protected NoisedTweenParameter[] m_TweenIndividualChannels;
    [SerializeField] protected T m_DisabledValue;
    public bool enable => m_Enable;
    protected virtual int channels => 0;
    public ValueTweenParameterBase(T disabledValue, bool enable = false)
    {
        this.m_DisabledValue = disabledValue;
        this.m_Enable = enable;

        if (channels > 0)
        {
            m_TweenIndividualChannels = new NoisedTweenParameter[channels];

            for (int i = 0; i < channels; i++)
            {
                m_TweenIndividualChannels[i] = new NoisedTweenParameter();
            }
        }
    }

    public void TemporaryFIX()
    {
        //        if(m_TweenIndividualChannels!=null)return;
        if (channels > 0)
        {
            if(m_TweenIndividualChannels==null) m_TweenIndividualChannels = new NoisedTweenParameter[channels];
            if(m_TweenIndividualChannels.Length!=channels)Array.Resize(ref m_TweenIndividualChannels,channels);
            
            for (int i = 0; i < channels; i++)
            {
                if(m_TweenIndividualChannels[i]==null)
                m_TweenIndividualChannels[i] = new NoisedTweenParameter();
            }
        }
    }
    public virtual T GetValue(float t)
    {
        TemporaryFIX();
        return default(T);
    }
}

[System.Serializable]
public class StartEndValueTweenParameter<T> : ValueTweenParameterBase<T>
{
    [SerializeField] private T m_Start;
    [SerializeField] private T m_End;
    [SerializeField] private SO_StartEndDataBase<T> m_StarEndData;
    [SerializeField] private bool m_FromAsset = false;

    public StartEndValueTweenParameter(T defaultValue, T disabledValue, bool enable = false) : base(disabledValue, enable)
    {
        this.defaultValue = this.m_Start = this.m_End = defaultValue;
    }

    public T start => m_FromAsset&&m_StarEndData!=null?m_StarEndData.start:m_Start;
    public T end => m_FromAsset&&m_StarEndData!=null?m_StarEndData.end:m_End;
    public T defaultValue { get; private set; }
    public override T GetValue(float t)
    {
        TemporaryFIX();
        return default(T);
    }
}

[System.Serializable]
public class Vector3TweenParameter : StartEndValueTweenParameter<Vector3>
{
    protected override int channels => 3;
    public Vector3TweenParameter(Vector3 defaultValue, Vector3 disabledValue, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }

    public override Vector3 GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                float x = Mathf.LerpUnclamped(start.x, end.x, m_TweenIndividualChannels[0].GetValue(t));
                float y = Mathf.LerpUnclamped(start.y, end.y, m_TweenIndividualChannels[1].GetValue(t));
                float z = Mathf.LerpUnclamped(start.z, end.z, m_TweenIndividualChannels[2].GetValue(t));
                return new Vector3(x, y, z);
            }
            return Vector3.LerpUnclamped(start, end, m_Tween.GetValue(t));
        }
        else return Vector3.zero;
    }
}

[System.Serializable]
public class Vector2TweenParameter : StartEndValueTweenParameter<Vector2>
{
    protected override int channels => 2;
    public Vector2TweenParameter(Vector2 defaultValue, Vector2 disabledValue, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }

    public override Vector2 GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                float x = Mathf.LerpUnclamped(start.x, end.x, m_TweenIndividualChannels[1].GetValue(t));
                float y = Mathf.LerpUnclamped(start.x, end.x, m_TweenIndividualChannels[0].GetValue(t));
                return new Vector2(x, y);
            }
            return Vector2.LerpUnclamped(start,end, m_Tween.GetValue(t));
        }
        else return Vector2.zero;
    }
}

[System.Serializable]
public class FloatTweenParameter : StartEndValueTweenParameter<float>
{
    protected override int channels => 1;
    public FloatTweenParameter(float defaultValue, float disabledValue, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }

    public override float GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                return Mathf.LerpUnclamped(start, end, m_TweenIndividualChannels[0].GetValue(t));
            }

            return Mathf.LerpUnclamped(start, end, m_Tween.GetValue(t));
        }
        
        return m_DisabledValue;
    }
}

[System.Serializable]
public class ColorTweenParameter : StartEndValueTweenParameter<Color>
{
    public ColorTweenParameter(Color defaultValue, Color disabledValue, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }

    public override Color GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable) return Color.LerpUnclamped(start, end, m_Tween.GetValue(t));
        return m_DisabledValue;
    }
}
[System.Serializable]
public class GradientTweenParameter : ValueTweenParameterBase<Color>
{
    [SerializeField] private Gradient m_Gradient;

    public GradientTweenParameter(Color disabledValue, bool enable = false) : base(disabledValue, enable) { }

    public override Color GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable && m_Gradient != null)
        {
            return m_Gradient.Evaluate(m_Tween.GetValue(t));
        }
        return m_DisabledValue;
    }
}

[System.Serializable]
public abstract class ResolvableTweenParameterBase<T> : ValueTweenParameterBase<T>
{
    protected ResolvableTweenParameterBase(T disabledValue, bool enable = false) : base(disabledValue, enable)
    {
    }

    public virtual IExposedPropertyTable Resolve(PlayableGraph graph)
    {
        TemporaryFIX();
        return graph.GetResolver();
    }
}

[System.Serializable]
public abstract class BezierTweenParameterBase<T> : ResolvableTweenParameterBase<T>
{
    [SerializeField] protected float m_TweenMaxTime = 1f;
    [SerializeField] protected float m_TweenMinTime = 0f;
    [SerializeField] protected ExposedReference<BezierSpline> m_Bezier;
    [SerializeField] protected bool m_EvenlySpaced;
    protected BezierSpline bezier;

    public BezierTweenParameterBase(T disabledValue, bool useEvenlySpaced = false, bool enable = false) : base(disabledValue, enable)
    {
        this.m_EvenlySpaced = useEvenlySpaced;
    }
    public override IExposedPropertyTable Resolve(PlayableGraph graph)
    {
        var resolver = base.Resolve(graph);
        bezier = m_Bezier.Resolve(resolver);
        return resolver;
    }
}

[System.Serializable]
public class BezierPositionTweenParameter : BezierTweenParameterBase<Vector3>
{
    public BezierPositionTweenParameter(Vector3 disabledValue, bool useEvenlySpaced = false, bool enable = false) : base(disabledValue, useEvenlySpaced, enable) { }
    public override Vector3 GetValue(float t)
    {
        if (enable && bezier != null)
        {
            float tTween = m_Tween.GetValue(t).Remap(0, 1, m_TweenMinTime, m_TweenMaxTime);
            if (m_EvenlySpaced) { return bezier.evenlySpacedPoints.spline.GetPoint(tTween); }
            else { return bezier.GetPoint(tTween); }
        }
        return m_DisabledValue;
    }
}

[System.Serializable]
public class BezierRotationTweenParameter : BezierTweenParameterBase<Vector3>
{
    [SerializeField] protected Vector3 m_VectorUp = Vector3.up;
    public BezierRotationTweenParameter(Vector3 disabledValue, bool useEvenlySpaced = false, bool enable = false) : base(disabledValue, useEvenlySpaced, enable) { }
    public override Vector3 GetValue(float t)
    {
        if (enable && bezier != null)
        {
            float tTween = m_Tween.GetValue(t);
            if (m_EvenlySpaced) { return Quaternion.LookRotation(bezier.evenlySpacedPoints.spline.GetTangent(tTween), m_VectorUp).eulerAngles; }
            else { return Quaternion.LookRotation(bezier.GetTangent(tTween), m_VectorUp).eulerAngles; }
        }
        return m_DisabledValue;
    }
}

[System.Serializable]
public abstract class TransformTweenParameterBase : ResolvableTweenParameterBase<Vector3>
{
    [SerializeField] private ExposedReference<Transform> m_Start;
    [SerializeField] private ExposedReference<Transform> m_End;
    [SerializeField] protected bool m_FromLocal = false;
    [SerializeField] protected bool m_EndByOffset = false;
    [SerializeField] protected Vector3 m_EndOffset = Vector3.zero;
    protected Transform start;
    protected Transform end;
    protected abstract Vector3 startLocalValue { get; }
    protected abstract Vector3 startValue { get; }
    protected abstract Vector3 endLocalValue { get; }
    protected abstract Vector3 endValue { get; }

    protected override int channels => 3;

    protected TransformTweenParameterBase(Vector3 disabledValue, bool enable = false) : base(disabledValue, enable)
    {
    }

    public override IExposedPropertyTable Resolve(PlayableGraph graph)
    { 
        var resolver = base.Resolve(graph);
        start = m_Start.Resolve(resolver);
        end = m_End.Resolve(resolver);
        return resolver;
    }

    public override Vector3 GetValue(float t)
    {
        base.GetValue(t);

        if (enable && start != null)
        {
            if (end == null && !m_EndByOffset) return m_DisabledValue;

            Vector3 s = m_FromLocal ? startLocalValue : startValue;
            Vector3 e = m_EndByOffset ? s + m_EndOffset : m_FromLocal ? endLocalValue : endValue;

            if (m_IndividualChannels)
            {
                float x = Mathf.Lerp(s.x, e.x, m_TweenIndividualChannels[0].GetValue(t));
                float y = Mathf.Lerp(s.y, e.y,  m_TweenIndividualChannels[1].GetValue(t));
                float z = Mathf.Lerp(s.z, e.z, m_TweenIndividualChannels[2].GetValue(t));
                return new Vector3(x, y, z);
            }

            return Vector3.LerpUnclamped(s, e, m_Tween.GetValue(t));
        }
        return m_DisabledValue;
    }
}

[System.Serializable]
public class TransformPositionTweenParameter : TransformTweenParameterBase
{
    public TransformPositionTweenParameter(Vector3 disabledPosition, bool enable = false) : base(disabledPosition, enable) { }

    protected override Vector3 startLocalValue => start.localPosition;

    protected override Vector3 startValue => start.position;

    protected override Vector3 endLocalValue => end.localPosition;

    protected override Vector3 endValue => end.position;
}

[System.Serializable]
public class TransformRotationTweenParameter : TransformTweenParameterBase
{
    public TransformRotationTweenParameter(Vector3 disabledPosition, bool enable = false) : base(disabledPosition, enable) { }

    protected override Vector3 startLocalValue => start.localEulerAngles;

    protected override Vector3 startValue => start.eulerAngles;

    protected override Vector3 endLocalValue => end.localEulerAngles;

    protected override Vector3 endValue => end.eulerAngles;

}

[System.Serializable]
public class TransformScaleTweenParameter : TransformTweenParameterBase
{
    public TransformScaleTweenParameter(Vector3 disabledPosition, bool enable = false) : base(disabledPosition, enable) { }

    protected override Vector3 startLocalValue => start.localScale;

    protected override Vector3 startValue => start.lossyScale;

    protected override Vector3 endLocalValue => end.localScale;

    protected override Vector3 endValue => end.lossyScale;
}

#region Material

[System.Serializable]
public class MaterialTweenParameterBase<T> : StartEndValueTweenParameter<T>
{
    [SerializeField] protected string m_PropertyName = string.Empty;

    private string m_LastPropertyName = string.Empty;
    private int m_PropertyID;
    public int propertyID
    {
        get
        {
            if (m_LastPropertyName != m_PropertyName)
            {
                m_LastPropertyName = m_PropertyName;
                m_PropertyID = Shader.PropertyToID(m_PropertyName);
            }
            return m_PropertyID;
        }
    }
    public MaterialTweenParameterBase(T defaultValue, T disabledValue, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }
}
[System.Serializable]
public class MaterialFloatTweenParameter : MaterialTweenParameterBase<float>
{
    protected override int channels => 1;
    public MaterialFloatTweenParameter(float defaultValue = 0, float disabledValue = 0, bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }
    public override float GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                return Mathf.LerpUnclamped(start, end, m_TweenIndividualChannels[0].GetValue(t));
            }

            return Mathf.LerpUnclamped(start, end, m_Tween.GetValue(t));
        }

        return m_DisabledValue;
    }
}
[System.Serializable]
public class MaterialColorTweenParameter : MaterialTweenParameterBase<Color>
{
    protected override int channels => 1;
    public MaterialColorTweenParameter(Color defaultValue = default(Color), Color disabledValue = default(Color), bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }
    public override Color GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                return Color.LerpUnclamped(start, end, m_TweenIndividualChannels[0].GetValue(t));
            }

            return Color.LerpUnclamped(start, end, m_Tween.GetValue(t));
        }

        return m_DisabledValue;
    }
}
[System.Serializable]
public class MaterialVectorTweenParameter : MaterialTweenParameterBase<Vector4>
{
    protected override int channels => 4;
    public MaterialVectorTweenParameter(Vector4 defaultValue = default(Vector4), Vector4 disabledValue = default(Vector4), bool enable = false) : base(defaultValue, disabledValue, enable)
    {
    }
    public override Vector4 GetValue(float t)
    {
        base.GetValue(t);

        if (m_Enable)
        {
            if (m_IndividualChannels)
            {
                float x = Mathf.LerpUnclamped(start.x, end.x, m_TweenIndividualChannels[0].GetValue(t));
                float y = Mathf.LerpUnclamped(start.y, end.y, m_TweenIndividualChannels[1].GetValue(t));
                float z = Mathf.LerpUnclamped(start.z, end.z, m_TweenIndividualChannels[2].GetValue(t));
                float w = Mathf.LerpUnclamped(start.z, end.z, m_TweenIndividualChannels[2].GetValue(t));
                return new Vector4(x, y, z, w);
            }

            return Color.LerpUnclamped(start, end, m_Tween.GetValue(t));
        }

        return m_DisabledValue;
    }
}

#endregion





