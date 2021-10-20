using System.Collections.Generic;
using UnityEngine;

public abstract class TweenableBase<T> : MonoBehaviour
{
    public abstract Dictionary<int, string> TweenableMembers { get; }
    public abstract void SetTweenableValue(int index, T value);
    public abstract T GetTweenableValue(int index);
}
