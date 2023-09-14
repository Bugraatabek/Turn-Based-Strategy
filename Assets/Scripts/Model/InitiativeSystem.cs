using System.Collections.Generic;
using UnityEngine;

public class InitiativeSystem 
{
    private int maxInitiative = 20;
    private int minInitiative = 1;

    private List<int> _takenInitiativesList = new List<int>();
    public InitiativeSystem(){}

    public int GetAnInitiative()
    {
        int randomInitiative = Random.Range(minInitiative,maxInitiative);
        if(_takenInitiativesList.Contains(randomInitiative))
        {
            randomInitiative = GetAnInitiative();
        }
        _takenInitiativesList.Add(randomInitiative);
        return randomInitiative;
    }
}