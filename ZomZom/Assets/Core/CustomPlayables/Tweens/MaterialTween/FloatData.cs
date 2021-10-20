public class FloatData
{
    public int propertyID;
    public float value;
    public float defaultValue;
    public void Add(float v)
    {
        value += v;
    }
    public void Initialize(float v)
    {
        defaultValue = v;
    }
}
