using Scriptable.Events;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TweenParameterCallback<T> : TweenCallbackBase
{
    [SerializeField] protected T data;
    [SerializeField] private ExposedReference<GameEventListenerBase<T>> eventListenerReference;
    private GameEventListenerBase<T> eventListener;
    public override void Initialize(Playable playable)
    {
        eventListener = eventListenerReference.Resolve(playable.GetGraph().GetResolver());
    }
    public override void Fire()
    {
        eventListener?.Invoke(data);
    }
}
