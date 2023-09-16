using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    [SerializeField] private Transform _gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private BaseAction selectedAction;
    private IUnit selectedUnit;

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
        ActionSystem.Instance.onSelectedActionChanged += ActionSystem_OnSelectedActionChanged;
    }

    private void ActionSystem_OnSelectedActionChanged(BaseAction selectedAction)
    {
        this.selectedAction = selectedAction;
        UpdateGridVisual();
    }

    private void ActionSystem_OnSelectedUnitChangedValidActionList(List<GridPosition> gridPositionList)
    {
        UpdateGridVisual();
    }

    private void ActionSystem_OnSelectedUnitChanged(IUnit unit)
    {
        selectedUnit = unit;
        UpdateGridVisual();
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        

        GridVisualType gridVisualType;
        switch(selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetRange(), GridVisualType.RedSoft);
                break;
        }

        ShowGridPositionList(selectedUnit.GetValidActionGridPositionList(), gridVisualType);
        
    }

    public void HideAllGridPositions()
    {
        foreach (var gridSystemVisualSingle in _gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        if(gridPositionList == null) return;
        foreach (var gridPosition in gridPositionList)
        {
            GridSystemVisualSingle gridSystemVisualSingle = _gridSystemVisualSingleArray[gridPosition.x,gridPosition.z];
            gridSystemVisualSingle.Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x,z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > range) continue;
               
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType) 
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Could not find GridVisualMaterial for GridVisualType" + gridVisualType);
        return null;
    }

    private void OnDisable() {
        ActionSystem.Instance.onSelectedUnitChanged += ActionSystem_OnSelectedUnitChanged;
    }
}
