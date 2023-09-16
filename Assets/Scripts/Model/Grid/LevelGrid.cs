using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance {get; private set;}

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int _width, _height;
    private const float _cellSize = 2f;
    private GridSystem _gridSystem;

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

        _gridSystem = new GridSystem(_width,_height,_cellSize);
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObjectAtPosition = _gridSystem.GetGridObject(gridPosition);
        gridObjectAtPosition.AddUnit(unit);
    }

    public List<Unit> GetUnitsListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObjectAtPosition = _gridSystem.GetGridObject(gridPosition);
        return gridObjectAtPosition.GetUnitList();
    }

    public void RemoveUnitFromGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObjectAtPosition = _gridSystem.GetGridObject(gridPosition);
        gridObjectAtPosition.RemoveUnit(unit);
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => _gridSystem.Width;
    public int GetHeight() => _gridSystem.Height;

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) 
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public Unit GetUnitAtGridPosition(Vector3 worldPoint)
    {
        GridPosition gridPosition = GetGridPosition(worldPoint);
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public Vector3 GetWorldPosition(Unit unit)
    {
        return GetWorldPosition(GetGridPosition(unit.transform.position));
    }
}
