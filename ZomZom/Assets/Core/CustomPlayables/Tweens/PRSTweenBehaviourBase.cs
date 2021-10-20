using UnityEngine;

public abstract class PRSTweenBehaviourBase : PlayableTweenBehaviourBase
{
    public virtual bool position { get; } = true;
    public virtual bool rotation { get; } = true;
    public virtual bool scale { get; } = true;

    public abstract Vector3 GetPosition(float t, Vector3 defaultValue);
    public abstract Vector3 GetRotation(float t, Vector3 defaultValue);
    public abstract Vector3 GetScale(float t, Vector3 defaultValue);
}
