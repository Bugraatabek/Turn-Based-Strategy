using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance {get; private set;}
    
    public int CurrentTurn {get {return _currentTurn;}}

    public event Action onCombatStarted;
    public event Action onSelectedUnitFinishedTurn;
    public event Action onEveryTurnFinished;
    public event Action onNewTurn;
    
    private int _currentTurn;
    private bool isPlayerTurn = true;

    
    private List<IUnit> activeUnitsList = new List<IUnit>();
    private Dictionary<IUnit,bool> turnDict = new Dictionary<IUnit, bool>();

    private Dictionary<int,IUnit> _initiativeDict = new Dictionary<int,IUnit>();
    private List<int> _initiativeList = new List<int>();
    private Queue<IUnit> _turnQueue = new Queue<IUnit>();
    
    private InitiativeSystem initiativeSystem;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        initiativeSystem = new InitiativeSystem();
    }

    private void Start()
    {
        BuildUnitsList();
        BuildTurnDict();
        SubscribeToUnitTurns();
        BuildInitiativeList();
        BuildTurnQueue();
        onCombatStarted?.Invoke();
    }

    private void BuildUnitsList()
    {
        activeUnitsList = new List<IUnit>();
        List<GameObject> unitGameObjects = GameObject.FindGameObjectsWithTag("Unit").ToList();
        
        foreach (GameObject unitGameObject in unitGameObjects)
        {
            activeUnitsList.Add(unitGameObject.GetComponent<IUnit>());
        }
    }

    private void BuildTurnDict()
    {
        foreach (IUnit unit in activeUnitsList)
        {
            turnDict.Add(unit, false);
        }
    }

    private void BuildInitiativeList()
    {
        _initiativeList = new List<int>();
        _initiativeDict = new Dictionary<int,IUnit>();

        foreach (IUnit unit in activeUnitsList)
        {
            int unitInitiative = initiativeSystem.GetAnInitiative();
            _initiativeList.Add(unitInitiative);
            _initiativeDict.Add(unitInitiative, unit);
        }
        _initiativeList.Sort();
    }

    private void BuildTurnQueue()
    {
        _turnQueue = new Queue<IUnit>();
        for (int i = 0; i < activeUnitsList.Count; i++)
        {
            _turnQueue.Enqueue(_initiativeDict[_initiativeList[i]]);
        }
    }

    private void SubscribeToUnitTurns()
    {
        foreach (IUnit unit in activeUnitsList)
        {
            unit.onTurnFinished += IUnit_OnTurnFinished;
        }
    }

    private void IUnit_OnTurnFinished(IUnit unit)
    {
        EndTurn(unit);

        onSelectedUnitFinishedTurn?.Invoke();
    }

    public void EndTurn(IUnit unit)
    {
        turnDict[unit] = true;
        if(!turnDict.ContainsValue(false))
        {
            onEveryTurnFinished?.Invoke();
        }
    }

    public void NextTurn()
    {
        if(turnDict.ContainsValue(false)) return;
        isPlayerTurn = !isPlayerTurn;
        _currentTurn += 1;
        ResetTurnData();
        onNewTurn?.Invoke();
    }

    private void ResetTurnData()
    {
        foreach (IUnit unit in activeUnitsList)
        {
            turnDict[unit] = false;
            unit.ResetTurn();
        }
    }

    public bool IsEveryTurnFinished()
    {
        if(!turnDict.ContainsValue(false)) return true;
        return false;
    }

    public IUnit GetUnitPlayingCurrentTurn()
    {
        return _turnQueue.Dequeue();
        //Add A logic for on next turn the queue should loop to first unit.
    }
}
