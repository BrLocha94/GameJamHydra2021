using UnityEngine.Timeline;

public class SignalEmitterWithBool : ParameterizedEmitter<bool> { }

public class SignalEmitterWithInt : ParameterizedEmitter<int> { }

public class SignalEmitterWithFloat : ParameterizedEmitter<float> { }

public class SignalEmitterWithString : ParameterizedEmitter<string> { }

public class ParameterizedEmitter<T> : SignalEmitter
{
    public T parameter;
}
