using System;
using UnityEngine;
using UnityEngine.Events;

public class Vector3Control : ControlBase<Vector3>
{
    public Vector3 add = Vector3.zero;
    public Vector3 multiplier = Vector3.one;
    public override Vector3 value
    {
        get
        {
            return Vector3.Scale((m_Value + add), multiplier);
        }
        set
        {
            m_Value = value;
            OnControlValueChanged?.Invoke(Vector3.Scale((m_Value + add), multiplier));
        }
    }
}
