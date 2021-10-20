using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.4f, 0.3f, 0.1f)]
[TrackClipType(typeof(CameraMatrix2TweenClip))]
[TrackBindingType(typeof(Camera))]
public class CameraMatrix2TweenTrack : PlaybleTweenTrack<CameraMatrix2TweenBehaviour, Camera, CameraMatrix2TweenMixerData>
{
    public bool trackPosition = true;
    public bool trackRotation = true;
    public bool trackScale = true;
    public bool trackProjectionObliqueness = true;
    public bool trackFOV = true;
    public Vector2Int index = Vector2Int.zero;
    public float value;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        base.CreateTrackMixer(graph, go, inputCount);
        var mixer = ScriptPlayable<CameraMatrix2TweenMixerBehaviour>.Create(graph, inputCount);
        mixerBehaviour = mixer.GetBehaviour();
        OnCreatedMixerBehaviour(mixer.GetBehaviour());
        return mixer;
    }
}