using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ParticleSystemActivatorClip : PlayableAsset, ITimelineClipAsset
    {
        /// <summary>
        /// Returns a description of the features supported by activation clips
        /// </summary>
        public ClipCaps clipCaps { get { return ClipCaps.None; } }

        /// <summary>
        /// Overrides PlayableAsset.CreatePlayable() to inject needed Playables for an activation asset
        /// </summary>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return Playable.Create(graph);
        }
    }
