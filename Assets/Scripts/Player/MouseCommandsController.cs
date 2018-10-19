using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;

/// <summary>
/// This class handles commands set by mouse
/// </summary>
public class MouseCommandsController : MonoBehaviour {
    /// <summary>
    /// This is the list of selected game objects
    /// </summary>
    [SerializeField]
    private List<GameObject> selectedGOs;

	void Start () {
        selectedGOs = new List<GameObject>();
	}
	
	void Update ()
    {
        ObjectSelector(); //left click
        FleetCommand(); //right click
	}

    /// <summary>
    /// This method handles fleet's commands like move selected this to position
    /// </summary>
    private void FleetCommand()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                GameObject selected = (GameObject)hit.collider.gameObject;
                if(selected.layer == (int) ObjectLayers.Map) //If right clicked on map
                {
                    foreach (GameObject go in selectedGOs)
                    {
                        ShipMovementController controller = go.GetComponent<ShipMovementController>();
                        if(controller != null)
                        {
                            controller.MoveToPosition(hit.point, 1f);
                        }
                    }
                }
                else if(selected.layer == (int)ObjectLayers.Ship)
                {
                    foreach(GameObject go in selectedGOs)
                    {
                        go.GetComponent<ShipController>().AttackTarget(selected);
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method handles the object selection (like selecting ships)
    /// </summary>
    private void ObjectSelector()
    {
        if (Input.GetMouseButtonDown(0)) //If left click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                GameObject selected = (GameObject)hit.collider.gameObject;
                if (selected.layer == (int) ObjectLayers.Ship)
                {
                    if(Input.GetKey(KeyCode.LeftShift)) //If the player is pressid leftShift, the selected GO must be added to selected
                    {
                        //If selected is already on the selected list, it will be removed;
                        foreach (GameObject go in selectedGOs)
                        {
                            if (go.Equals(selected))
                            {
                                selectedGOs.Remove(selected); //Remove object from selected
                                return;
                            }
                        }
                        selectedGOs.Add(selected); //Add object to selected
                    }
                    else //If the player is not pressing LeftShift, the list of selected object will be cleared and the selected will be added
                    {
                        selectedGOs.Clear();
                        selectedGOs.Add(selected);
                    }
                    
                }
                else //Clear the selected list if clicked on no ships
                {
                    selectedGOs.Clear();
                }
            }
        }
    }
}
