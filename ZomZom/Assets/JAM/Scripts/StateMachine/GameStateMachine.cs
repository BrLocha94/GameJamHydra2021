using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoSingleton<GameStateMachine>
{
    [Header("If this variable is diferent than null, the initial state is overwritten")]
    [SerializeField]
    private GameStates forceInitialState = GameStates.Null;
    [Space]
    [SerializeField]
    private List<Node> stateMachine = new List<Node>();

    private int index = 0;
    public GameStates currentState() => stateMachine[index].state;

    protected override void ExecuteOnAwake()
    {
        base.ExecuteOnAwake();

        if(forceInitialState != GameStates.Null)
        {
            for (int i = 0; i < stateMachine.Count; i++)
            {
                if (stateMachine[i].state == forceInitialState)
                {
                    Debug.Log("Game state changed from " + stateMachine[index].state + " to " + stateMachine[i].state);
                    index = i;
                    return;
                }
            }
        }
    }

    public void ChangeState(GameStates newGameState)
    {
        if (stateMachine[index].allowedTransitions.Contains(newGameState))
        {
            for(int i = 0; i < stateMachine.Count; i++)
            {
                if(stateMachine[i].state == newGameState)
                {
                    Debug.Log("Game state changed from " + stateMachine[index].state + " to " + stateMachine[i].state);
                    index = i;
                    return;
                }
            }
        }
    }

    [System.Serializable]
    private class Node
    {
        public GameStates state;
        public List<GameStates> allowedTransitions;
    }
}

public enum GameStates
{
    Null,
    Initializing,
    Waiting,
    RollingReel,
}