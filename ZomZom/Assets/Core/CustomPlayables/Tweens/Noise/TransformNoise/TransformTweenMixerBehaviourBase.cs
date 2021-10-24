using UnityEngine;

public abstract class TransformTweenMixerBehaviourBase<BEHAVIOUR> : PRSMixerBehaviour<Transform, BEHAVIOUR>
    where BEHAVIOUR : PRSTweenBehaviourBase, new()
{
    protected override void GetDefaultValues()
    {
        m_DefaultPosition = trackBinding.localPosition;
        m_DefaultRotation = trackBinding.localEulerAngles;
        m_DefaultScale = trackBinding.localScale;
    }
    protected override void ResetToDefaultValues()
    {
        if (trackBinding != null)
        {
            trackBinding.localPosition = m_DefaultPosition;
            trackBinding.localEulerAngles = m_DefaultRotation;
            trackBinding.localScale = m_DefaultScale;
        }
    }
    protected override void SetPosition(Vector3 pos)
    {
        if (m_MasterTrack.localPosition) trackBinding.localPosition = pos;
        else trackBinding.position = pos;
    }
    protected override void SetRotation(Vector3 rot)
    {
        if (m_MasterTrack.localRotation) trackBinding.localEulerAngles = rot;
        else trackBinding.localEulerAngles = rot;
    }
    protected override void SetScale(Vector3 scale)
    {
        trackBinding.localScale = scale;
    }
}
