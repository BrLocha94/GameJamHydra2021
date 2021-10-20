using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class RenderSettingsClip : TweenClipBase<RenderSettingsBehaviour>
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RenderSettingsBehaviour>.Create(graph, template);
        return playable;
    }
}
