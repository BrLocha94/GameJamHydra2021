using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Events
{
    [CreateAssetMenu(fileName ="New Void Event", menuName = "Scriptable Events/Void")]
    public class VoidEvent : BaseGameEvent<Void>
    {
        public void Invoke()=>Invoke(new Void());
    }
}
