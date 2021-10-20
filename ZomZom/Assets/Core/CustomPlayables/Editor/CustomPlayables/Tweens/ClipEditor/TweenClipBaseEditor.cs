using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

public class TweenClipBaseEditor<T> : ClipEditor where T : PlayableCallbackBehaviour, new()
{
    private static readonly Color[] colors = new Color[] { Color.red, Color.green, Color.yellow, Color.blue, Color.magenta, Color.white };
    private static readonly float width = 1;
    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        base.DrawBackground(clip, region);

        var callbackbehaviour = (clip.asset as TweenClipBase<T>).template;
        var callbacks = callbackbehaviour.callbacks;
        float heigth = region.position.height / callbacks.Count;

        for (int i = 0; i < callbacks.Count; i++)
        {
            var itens = callbacks[i];

            for (int j = 0; j < itens.Count; j++)
            {
                var item = itens[j];

                float clipWidth = region.position.width;
                float clipXpos = region.position.min.x;

                Rect markerRect = new Rect(clipXpos + width + (clipWidth - width * 3) * item.time, region.position.y + i * (heigth), width, heigth);
                EditorGUI.DrawRect(markerRect, colors[i]);
            }
        }
    }

    public override void OnClipChanged(TimelineClip clip)
    {
        base.OnClipChanged(clip);
    }

    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        return base.GetClipOptions(clip);
    }

}

