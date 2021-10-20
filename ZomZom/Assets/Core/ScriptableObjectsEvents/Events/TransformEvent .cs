using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Events
{
    [CreateAssetMenu(fileName ="New Transform Event", menuName = "Scriptable Events/Transform")]
    public class TransformEvent : BaseGameEvent<Transform>{}
}
