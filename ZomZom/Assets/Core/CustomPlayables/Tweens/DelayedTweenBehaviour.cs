using UnityEngine;

public class DelayedTweenBehaviour : PlayableTweenBehaviourBase
{
    [Min(0)]
    public float delay = 0.0f;

    [SerializeField]private bool m_ReverseOrder = false;
    [SerializeField]private bool m_Backwards = false;
    public bool reverse=>m_ReverseOrder;
    public bool backwards=>m_Backwards;
}
