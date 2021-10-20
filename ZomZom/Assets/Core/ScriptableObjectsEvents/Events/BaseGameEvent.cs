using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Events
{
    public class BaseGameEvent<T> : ScriptableObject
    {
       private readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();

        public void Invoke(T item)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].Invoke(item);
            }
        }
        public void RegisterListener(IGameEventListener<T> listener)
        {
            if(!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }
        public void UnregisterListener(IGameEventListener<T> listener)
        {
            if(eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
