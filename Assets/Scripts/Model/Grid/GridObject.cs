using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;
    private List<Unit> _unitsList = new List<Unit>();

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _unitsList)
        {
            unitString += unit + "\n";
        }

        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unitToSet)
    {
        _unitsList.Add(unitToSet);
    }

    public List<Unit> GetUnitList()
    {
        return _unitsList;
    }

    public void RemoveUnit(Unit unit)
    {
        _unitsList.Remove(unit);
    }

    public bool HasAnyUnit()
    {
        return _unitsList.Count > 0;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return _unitsList[0];
        }
        return null;
    }
}
