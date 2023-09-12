using Controller;
using UnityEngine;

public class Testing : MonoBehaviour 
{
    [SerializeField] Unit unit;
    [SerializeField] GridSystemVisual gridSystemVisual;
    private void Update() 
    {
        // if(Input.GetKeyDown(KeyCode.T))
        // {
        //     gridSystemVisual.ShowGridPositionList(unit.GetValidActionGridPositionList());
        // }
    }
}
