using System.Collections.Generic;
using UnityEngine;

public class InitiativeSystem 
{
    private int maxInitiative = 20;

    private List<int> _availableInitiativesList = new List<int>();
    
    public InitiativeSystem()
    {
        
    }

    private void BuildAvailableInitiativesList()
    {
        _availableInitiativesList = new List<int>();
        for (int i = 0; i < maxInitiative; i++)
        {
            _availableInitiativesList.Add(i);
        }
    }

    private int GetAnInitiative()
    {
        if(_availableInitiativesList.Count <= 0) BuildAvailableInitiativesList();

        int randomInitiative = _availableInitiativesList[Random.Range(0,_availableInitiativesList.Count)];
        _availableInitiativesList.Remove(randomInitiative);
        return randomInitiative;
    }

    public void SetInitiatives(List<IUnit> units)
    {
        foreach (IUnit unit in units)
        {
            unit.SetInitiative(GetAnInitiative());
        }
    }
}