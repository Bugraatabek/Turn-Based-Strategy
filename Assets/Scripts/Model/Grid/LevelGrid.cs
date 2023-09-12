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
    private GridSystem gridSystem;

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

        gridSystem = new GridSystem(_width,_height,_cellSize);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObjectAtPosition = gridSystem.GetGridObject(gridPosition);
        gridObjectAtPosition.AddUnit(unit);
        print("LevelGrid Script: Inside SetUnitAtGridPosition");
    }

    public List<Unit> GetUnitsListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObjectAtPosition = gridSystem.GetGridObject(gridPosition);
        return gridObjectAtPosition.GetUnitList();
    }

    public void RemoveUnitFromGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObjectAtPosition = gridSystem.GetGridObject(gridPosition);
        gridObjectAtPosition.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
}
