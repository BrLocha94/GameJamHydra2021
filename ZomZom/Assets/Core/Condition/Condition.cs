using System;

public class Condition
{
    private Func<bool> conditionDef;
    public Condition(Func<bool> conditionDef)
    {
        this.conditionDef = conditionDef;
    }

    /// <summary>
    /// Returns true if the condition defined is true
    /// </summary>
    /// <returns></returns>
    public bool MetCondition()
    {
        var result = conditionDef.Invoke();
        return result;
    }
}
