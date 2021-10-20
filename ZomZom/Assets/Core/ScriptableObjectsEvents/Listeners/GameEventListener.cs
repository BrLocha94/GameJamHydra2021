using UnityEngine;
using UnityEngine.Events;

namespace Scriptable.Events
{
    public abstract class GameEventListener<T, E, UER> : GameEventListenerBase<T> where E: BaseGameEvent<T> where UER: UnityEvent<T>
    {
       [SerializeField] private E gameEvent;
       [SerializeField] private UER unityEventResponse;
        public E GameEvent { get => gameEvent; set => gameEvent = value; }

        private void OnEnable()
        {
            gameEvent?.RegisterListener(this);
        }
        private void OnDisable()
        {
            gameEvent?.UnregisterListener(this);
        }

        public override void Invoke(T item)
        {
            unityEventResponse?.Invoke(item);
        }
    }
    public abstract class GameEventListenerBase<T>: MonoBehaviour, IGameEventListener<T> 
    {
        public abstract void Invoke(T item);
    }
}

