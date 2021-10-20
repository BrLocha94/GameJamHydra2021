using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PRSTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> : PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA>
    where BEHAVIOUR : PlayableCallbackBehaviour, new()
    where COMPONENT : class
    where MIXERDATA : ITweenMixerData
{
    [SerializeField] private bool m_Relative = false;
    [SerializeField] private bool m_ConvertPosition = false;
    [SerializeField] private ExposedReference<Camera> m_FromCamera;
    [SerializeField] private ExposedReference<Camera> m_ToCamera;
    public bool convertPosition => m_ConvertPosition;
    public bool relative => m_Relative;
    public Camera fromCamera { private set; get; }
    public Camera toCamera { private set; get; }

    public bool trackPosition = true;
    public bool trackRotation = true;
    public bool trackScale = true;

    public bool localPosition = false;
    public bool localRotation = false;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        fromCamera = m_FromCamera.Resolve(graph.GetResolver());
        toCamera = m_ToCamera.Resolve(graph.GetResolver());
        return default(Playable);
    }
}
