using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TweenCallbackBase
{
    
    [Range(0f, 1f)] [SerializeField] protected float normalizedTime = 0;
    public float time => normalizedTime;
    public virtual void Initialize(Playable playable) { }
    public virtual void Fire() { }
}
