using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayManager
{
    private static PlayManager _instance;
    public static PlayManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayManager();
            return _instance;
        }
    }

    private System.Random random = new System.Random();
    private Math currentMath;
    private int[][] prizeIndexesToSearch;

    public void initialize(string mathName)
    {
        currentMath = Math.Load(Application.streamingAssetsPath + "/" + mathName + ".json");
        createIndexSearch();
        Debug.Log("Playmanager initialized");
        GameStateMachine.Instance.ChangeState(GameStates.Waiting);
    }

    private void createIndexSearch()
    {
        prizeIndexesToSearch = new int[currentMath.prizes.Length][];
        for (int t = 0; t < currentMath.prizes.Length; t++)
        {
            prizeIndexesToSearch[t] = new int[currentMath.symbols.Length];
            for (int i = 0; i < currentMath.prizes[t].symbols.Length; i++)
                prizeIndexesToSearch[t][i] = Array.FindIndex(currentMath.symbols, m => m.Equals(currentMath.prizes[t].symbols[i]));
        }
    }

    public Ticket play(double anyPrizeChance)
    {
        PlayerMoney.instance.subtractFromBalance(currentMath.wager);
        if (random.NextDouble() <= anyPrizeChance)
        {
            Ticket ticket = createPayingTicket();
            PlayerMoney.instance.addToBalance(ticket.value);
            return ticket;
        }
        return createNonPayingTicket();
    }

    public Ticket createPayingTicket()
    {
        Ticket ticket = new Ticket();
        var specificPrize = random.NextDouble();
        double acc = 0;
        for (int t = 0; t < currentMath.prizes.Length; t++)
        {
            var prize = currentMath.prizes[t];
            acc += prize.chance;
            if (specificPrize <= acc)
            {
                ticket.value = prize.value;
                ticket.symbols = prize.symbols;
                return ticket;
            }
        }
        throw new Exception("Invalid state");
    }

    public Ticket createNonPayingTicket()
    {
        Ticket ticket = new Ticket();
        int size = currentMath.prizes[0].symbols.Length;
        var candidateSymbols = new int[size];
        int attempts = 0;
        while (true)
        {
            bool isPrizeShouldRetry = false;
            for (int i = 0; i < size; i++)
                candidateSymbols[i] = random.Next(size);
            for (int c = 0; c < prizeIndexesToSearch.Length; c++)
            {
                if (Enumerable.SequenceEqual(candidateSymbols, prizeIndexesToSearch[c]))
                {
                    isPrizeShouldRetry = true; break;
                }
            }
            if (!isPrizeShouldRetry)
                break;
            attempts++;
            if (attempts > 1000000)
                throw new Exception("Failed to find a not paying ticket. Are you sure your math is not crazy enough?");
        }

        ticket.symbols = new string[size];
        for (int t = 0; t < size; t++)
        {
            ticket.symbols[t] = currentMath.symbols[candidateSymbols[t]];
        }
        ticket.value = 0;
        return ticket;
    }

    /// //////////////////////////////////////////

    private int _sequenceIndex;

    public void resetSequence()
    {
        _sequenceIndex = 0;
    }

    public Ticket playSequence()
    {
        PlayerMoney.instance.subtractFromBalance(currentMath.wager);
        var targetPrize = currentMath.sequence[_sequenceIndex];
        _sequenceIndex++;
        if (_sequenceIndex == currentMath.sequence.Length)
            _sequenceIndex = 0;
        if (targetPrize == 0)
            return createNonPayingTicket();
        var possiblePrizes = currentMath.prizes.Where(x => x.value == targetPrize).ToArray();
        Ticket ticket = new Ticket();
        ticket.value = targetPrize;
        ticket.symbols = possiblePrizes[random.Next(possiblePrizes.Length)].symbols;
        PlayerMoney.instance.addToBalance(targetPrize);
        return ticket;
    }

    //////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class Prize
    {
        public double chance;
        public int value;
        public string[] symbols;
    }

    [Serializable]
    public class Math
    {
        public int wager;
        public string[] symbols;
        public Prize[] prizes;
        public int[] sequence;
        public static Math Load(string filename) => JsonUtility.FromJson<Math>(File.ReadAllText(filename));

    }

    [Serializable]
    public class Ticket
    {
        public int value;
        public string[] symbols;
        public override string ToString() => JsonUtility.ToJson(this);
    }

}
