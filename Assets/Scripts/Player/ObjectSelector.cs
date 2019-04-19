using Imperium;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    public List<GameObject> selectedGOs = new List<GameObject>();
    private static ObjectSelector _instance;
    private List<GameObject> previousSelectedGOs = new List<GameObject>();
    private HashSet<Action<List<GameObject>>> selectionObservers = new HashSet<Action<List<GameObject>>>();
    public bool lockSelection = false;
    // private int selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Map) | (1 << (int)ObjectLayers.Station) | (1 << (int)ObjectLayers.Asteroid);

    public ObjectSelector()
    {
        Instance = this;
    }

    public static ObjectSelector Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public void AddSelectionObserver(Action<List<GameObject>> action)
    {
        selectionObservers.Add(action);
    }

    public void RemoveSelectionObserver(Action<List<GameObject>> action)
    {
        selectionObservers.RemoveWhere((Action<List<GameObject>> a) =>
        {
            return a.Equals(action);
        });
    }

    private void BoxSelector()
    {
    }

    private void CallSelectionObservers()
    {
        if (DidSelectedGOsChange())
        {
            foreach (Action<List<GameObject>> action in selectionObservers)
            {
                action.Invoke(selectedGOs);
            }
        }
    }

    private void ClearSelectedGOList()
    {
        foreach (GameObject go in selectedGOs)
        {
            if (go == null)
            {
                continue;
            }
            CombatStatsCanvasController combatStatsCanvasController = go.GetComponent<CombatStatsCanvasController>();

            if (combatStatsCanvasController != null)
            {
                combatStatsCanvasController.SetActive(false);
            }
        }
        selectedGOs.Clear();
    }

    private bool DidSelectedGOsChange()
    {
        if (selectedGOs.Count != previousSelectedGOs.Count)
        {
            return true;
        }
        for (int i = 0; i < selectedGOs.Count; i++)
        {
            if (!previousSelectedGOs.Contains(selectedGOs[i]))
            {
                return true;
            }
        }
        return false;
    }

    private void RaySelector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            GameObject selected = hit.collider.gameObject;

            if (selected.layer == (int)ObjectLayers.Ship || selected.layer == (int)ObjectLayers.Station)
            {
                if (!MapObjecsRenderingController.Instance.visibleObjects.Contains(selected))
                {
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftShift)) //If the player is pressid leftShift, the selected GO must be added to selected
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
                    ClearSelectedGOList();
                    selectedGOs.Add(selected);
                }
            }
            else //Clear the selected list if clicked on no ships or stations
            {
                ClearSelectedGOList();
            }
        }
    }

    private void Start()
    {
    }
    // Update is called once per frame
    private void Update()
    {
        if (!this.lockSelection && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0)) //If left click
            {
                RaySelector();
                CallSelectionObservers();
                previousSelectedGOs = new List<GameObject>(selectedGOs);
            }
            /*else if (Input.GetMouseButton(0))
            {
                BoxSelector();
                CallSelectionObservers();
            }*/
        }
    }
}