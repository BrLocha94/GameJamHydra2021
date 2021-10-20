
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Timeline;
using UnityEditor.Timeline.Actions;
using UnityEngine.Timeline;

/*[MenuEntry("Add Override Track", MenuPriority.CustomTrackActionSection.addOverrideTrack), UsedImplicitly]
class AddOverrideTrackAction : TrackAction
{
    public override bool Execute(IEnumerable<TrackAsset> tracks)
    {

        foreach (var animTrack in tracks.OfType<TMPTweenTrack>())
        {

            var track = TimelineEditor.inspectedAsset.CreateTrack(typeof(TMPTweenTrack), animTrack, "Override " + animTrack.GetChildTracks().Count());
            if (track != null)
            {
                if (animTrack != null)
                    animTrack.SetCollapsed(false);

                

               // var editor = CustomTimelineEditorCache.GetTrackEditor(track);
               // editor.OnCreate_Safe(track, null);
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            }
        }

        return true;
    }

    public override ActionValidity Validate(IEnumerable<TrackAsset> tracks)
    {
        if (tracks.Any(t => t.isSubTrack || !t.GetType().IsAssignableFrom(typeof(TMPTweenTrack))))
            return ActionValidity.NotApplicable;

        if (tracks.Any(t => t.lockedInHierarchy))
            return ActionValidity.Invalid;

        return ActionValidity.Valid;
    }
}*/

