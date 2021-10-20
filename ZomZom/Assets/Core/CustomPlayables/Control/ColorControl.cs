using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorControl : ControlBase<Color>
{
    public override Dictionary<int, string> TweenableMembers => throw new NotImplementedException();

    private void Reset()
    {
        m_Value = Color.white;
    }
}
