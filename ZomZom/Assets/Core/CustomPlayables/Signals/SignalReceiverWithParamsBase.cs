using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class SignalReceiverWithParamsBase<T> : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair<T>[] signalAssetEventPairs;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is ParameterizedEmitter<T> emitter)
        {
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, emitter.asset));
            foreach (var m in matches)
            {
                m.events.Invoke(emitter.parameter);
            }
        }
    }
}
