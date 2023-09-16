using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance {get; private set;}
    
    public int CurrentTurn {get {return _currentTurn;}}

    public event Action<IUnit> onNextUnitTurn;
    public event Action onEveryTurnFinished;
    public event Action onNewTurn;
    
    private int _currentTurn = 1;

    
    private List<IUnit> activeUnitsList = new List<IUnit>();

    private Dictionary<IUnit,bool> turnDict = new Dictionary<IUnit, bool>();
    private Dictionary<int,IUnit> initiativeToUnitDict = new Dictionary<int,IUnit>();
    
    private Queue<IUnit> turnQueue = new Queue<IUnit>();
    
    private InitiativeSystem initiativeSystem;



    private float testTimer = 0;


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
        BuildActiveUnitsList();
        initiativeSystem.SetInitiatives(activeUnitsList); // Sets initiative to all units in active units list for the combat.

        BuildInitiativeToUnitDict();
        BuildTurnDict();
        BuildTurnQueue();
        SubscribeToUnitTurns();
        onNextUnitTurn?.Invoke(GetNextUnitInTurn());
    }

    private void Update() 
    {
        testTimer += Time.deltaTime;
    }

    private void BuildActiveUnitsList() // Keeps data of all active IUnits in the scene
    {
        activeUnitsList = new List<IUnit>();
        List<GameObject> unitGameObjects = GameObject.FindGameObjectsWithTag("Unit").ToList();
        
        foreach (GameObject unitGameObject in unitGameObjects)
        {
            unitGameObject.TryGetComponent<IUnit>(out IUnit unit );
            activeUnitsList.Add(unit);
        }
    }

    private void BuildInitiativeToUnitDict() // Used to add units by a descending order to queue
    {
        initiativeToUnitDict = new Dictionary<int,IUnit>();
        foreach (var unit in activeUnitsList)
        {
            initiativeToUnitDict.Add(unit.GetInitiative(),unit);
        }
    }

    private void BuildTurnDict() // Used to check if everyone played their turn.
    {
        turnDict = new Dictionary<IUnit, bool>();
        foreach (IUnit unit in activeUnitsList)
        {
            turnDict.Add(unit, false);
        }
    }

    private void BuildTurnQueue() // Builds the turn queue according to initiatives in a descending order.
    {
        List<int> initiativeList = new List<int>();
        foreach (var unit in activeUnitsList)
        {
            initiativeList.Add(unit.GetInitiative());
        }
        initiativeList.Sort();


        turnQueue = new Queue<IUnit>();
        for (int i = 0; i < initiativeList.Count; i++)
        {
            turnQueue.Enqueue(initiativeToUnitDict[initiativeList[i]]);
        }
    }


    private void SubscribeToUnitTurns()
    {
        foreach (IUnit unit in activeUnitsList)
        {
            unit.onTurnFinished += IUnit_OnTurnFinished;
            unit.onDead += IUnit_OnDead;
        }
    }

    private void IUnit_OnDead(IUnit unit)
    {
        RemoveUnitFromSystem(unit);
    }

    private void IUnit_OnTurnFinished(IUnit unit)
    {
        EndTurn(unit);
    }

    public void EndTurn(IUnit unit)
    {
        turnDict[unit] = true;
        if(!turnDict.ContainsValue(false)) //checking if every unit played its turn.
        {
            onEveryTurnFinished?.Invoke(); //communicated with the UI.
            NextTurn();
            return;
        }
        onNextUnitTurn?.Invoke(GetNextUnitInTurn()); 
    }

    public void NextTurn()
    {
        _currentTurn += 1;

        SetupNewTurn();
        
        onNewTurn?.Invoke();
        onNextUnitTurn?.Invoke(GetNextUnitInTurn());
    }

    private void SetupNewTurn()
    {
        BuildTurnQueue();
        foreach (var unit in activeUnitsList)
        {
            unit.ResetTurn();
            turnDict[unit] = false;
        }
    }

    private IUnit GetNextUnitInTurn()
    {
        IUnit nextUnitToPlayTurn = turnQueue.Dequeue();
        return nextUnitToPlayTurn;
    }

    private void RemoveUnitFromSystem(IUnit unitToRemove)
    {
        activeUnitsList.Remove(unitToRemove);
        turnDict.Remove(unitToRemove);
        initiativeToUnitDict.Remove(unitToRemove.GetInitiative());

        turnQueue = new Queue<IUnit>(turnQueue.Where(unit => unit != unitToRemove));

        unitToRemove.onTurnFinished -= IUnit_OnTurnFinished;
        unitToRemove.onDead -= IUnit_OnDead;
    }
}
