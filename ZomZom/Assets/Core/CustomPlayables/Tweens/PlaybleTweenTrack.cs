using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> : TrackAsset, ILayerable
    where BEHAVIOUR : PlayableCallbackBehaviour, new()
    where COMPONENT : class
    where MIXERDATA : ITweenMixerData

{
    public bool postplaybackResetToDefault = true;
    public ETweenMixerDataMode mixerDataMode = ETweenMixerDataMode.Add;
    protected bool isLayerTrack = false;
    protected int layerIndex = -1;
    public TweenMixerBehaviour<BEHAVIOUR, COMPONENT, MIXERDATA> mixerBehaviour { protected set; get; }
    public List<PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA>> subTracks = new List<PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA>>();
    public PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> masterTrack;
    public bool IsValidTrack(PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> track)
    {
        return (track.hasClips && !track.muted);
    }
    public bool HasValidSubTracks()
    {
        for (int i = 0; i < subTracks.Count; i++)
        {
            if (IsValidTrack(subTracks[i])) return true;
        }

        return false;
    }
    public bool GetPreviousValidTrack(int index, out PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> previousTrack)
    {
        previousTrack = null;
        int startIndex = index - 1;
        if (startIndex < 0) return false;

        for (int i = startIndex; i >= 0; i--)
        {
            var subTrack = subTracks[i];

            if (IsValidTrack(subTrack))
            {
                previousTrack = subTrack;
                return true;
            }
        }

        return false;
    }
    public PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> GetLastValidTrack()
    {
        PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA> previousTrack;

        if(GetPreviousValidTrack(subTracks.Count, out previousTrack))
        {
            return previousTrack;
        }
        return null;
    }
    public Playable CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        masterTrack = this;
        subTracks = new List<PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA>>();
        int layerIndex = 0;
        foreach (var subTrack in GetChildTracks())
        {
            var child = subTrack as PlaybleTweenTrack<BEHAVIOUR, COMPONENT, MIXERDATA>;
            if (child != null)
            {
                child.isLayerTrack = true;
                child.layerIndex = layerIndex;
                child.masterTrack = this;
                subTracks.Add(child);
                layerIndex++;
            }
        }

        masterTrack = this;

        var mixer = AnimationLayerMixerPlayable.Create(graph, inputCount);
        return mixer;
    }
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var clips = GetClips();
        foreach (var clip in clips)
        {
            var loopClip = clip.asset as TweenClipBase<BEHAVIOUR>;
            loopClip.template.clip = clip;
        }
        return default(Playable);
    }

    protected virtual void OnCreatedMixerBehaviour(TweenMixerBehaviour<BEHAVIOUR, COMPONENT, MIXERDATA> mixerBehaviour)
    {
        mixerBehaviour.layerIndex = layerIndex;
        mixerBehaviour.masterTrack = masterTrack;
        mixerBehaviour.track = this;
    }
}
