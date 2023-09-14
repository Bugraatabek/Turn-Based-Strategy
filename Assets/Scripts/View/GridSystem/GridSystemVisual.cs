using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform _gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Start() 
    {
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                Transform gridSystemVisualSingleTransform = 
                    Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                
                _gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }


        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
        ActionSystem.Instance.onSelectedUnitChangedValidActionList += ActionSystem_OnSelectedUnitChangedValidActionList;
    }

    private void ActionSystem_OnSelectedUnitChangedValidActionList(List<GridPosition> gridPositionList)
    {
        ShowGridPositionList(gridPositionList);
    }

    private void ActionSystem_OnSelectedUnitChanged(IUnit unit)
    {
        ShowGridPositionList(unit.GetValidActionGridPositionList());
    }

    public void HideAllGridPositions()
    {
        foreach (var gridSystemVisualSingle in _gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        HideAllGridPositions();
        if(gridPositionList == null) return;
        foreach (var gridPosition in gridPositionList)
        {
            GridSystemVisualSingle gridSystemVisualSingle = _gridSystemVisualSingleArray[gridPosition.x,gridPosition.z];
            gridSystemVisualSingle.Show();
        }
    }

    private void OnDisable() {
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
    }
}
