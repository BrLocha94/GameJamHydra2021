using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class FloatControl : ControlBase<float>
{
    public float add = 0;
    public float multiplier = 1;
    public override float value
    {
        get
        {
            return (m_Value + add) * multiplier;
        }
        set
        {
            m_Value = value;
            OnControlValueChanged?.Invoke((m_Value + add) * multiplier);
        }
    }
}
