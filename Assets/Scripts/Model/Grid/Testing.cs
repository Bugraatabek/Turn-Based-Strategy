using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class Testing : MonoBehaviour 
{
    [SerializeField] Unit unit;
    

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridPosition endGridPosition = LevelGrid.Instance.GetGridPosition(RaycastController.Instance.GetMousePosition());
            GridPosition startGridPosition = new GridPosition(0,0);

            List<GridPosition> gridPositionList = PathFinding.Instance.FindPath(startGridPosition, endGridPosition);

            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.white,
                    10f
                );

            }
        }
    }


}
