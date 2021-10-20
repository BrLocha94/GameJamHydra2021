using System.Collections.Generic;

public class Conditions
{
    public enum Mode { ALL_FALSE, ALL_TRUE, ONE_TRUE, ONE_FALSE }
    private Mode mode;

    public List<Condition> conditionsList = new List<Condition>();
    public Conditions(Mode mode = Mode.ALL_TRUE) { this.mode = mode; }
    public void Add(Condition condition)
    {
        if (!conditionsList.Contains(condition))
            conditionsList.Add(condition);
    }
    public void RemoveCondition(Condition condition)
    {
        conditionsList.Remove(condition);
    }
    /// <summary>
    /// Returns true if all added conditions met the condition mode
    /// </summary>
    /// <returns></returns>
    public bool MetConditions()
    {
        bool result = true;

        switch (mode)
        {
            case Mode.ALL_FALSE:
                for (int i = 0; i < conditionsList.Count; i++)
                {
                    if (conditionsList[i].MetCondition())
                    {
                        result = false;
                        break;
                    }
                }
                break;
            case Mode.ALL_TRUE:
                for (int i = 0; i < conditionsList.Count; i++)
                {
                    if (!conditionsList[i].MetCondition())
                    {
                        result = false;
                        break;
                    }
                }
                break;
            case Mode.ONE_TRUE:
                result = false;
                for (int i = 0; i < conditionsList.Count; i++)
                {
                    if (conditionsList[i].MetCondition())
                    {
                        result = true;
                        break;
                    }
                }
                break;
            case Mode.ONE_FALSE:
                result = false;
                for (int i = 0; i < conditionsList.Count; i++)
                {
                    if (!conditionsList[i].MetCondition())
                    {
                        result = true;
                        break;
                    }
                }
                break;
        }

        return result;
    }
}
