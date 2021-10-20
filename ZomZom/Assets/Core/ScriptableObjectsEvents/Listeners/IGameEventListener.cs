using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Events
{
    public interface IGameEventListener<T>
    {
        void Invoke(T item);
    }
}
