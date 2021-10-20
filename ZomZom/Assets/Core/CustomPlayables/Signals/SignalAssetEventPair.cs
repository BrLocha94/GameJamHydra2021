using System;
using UnityEngine.Events;
using UnityEngine.Timeline;

[Serializable]
public class SignalAssetEventPair<T>
{
    public SignalAsset signalAsset;
    public UnityEvent<T> events;
}