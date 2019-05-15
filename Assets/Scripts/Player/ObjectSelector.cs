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
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000f);

        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit raycastHit = raycastHits[i];

            GameObject hitObject = raycastHit.collider.gameObject;
            ISelectable selectable = hitObject.GetComponent<ISelectable>();
            if (selectable != null &&  MapObjecsRenderingController.Instance.visibleObjects.Contains(hitObject))
            {
                
                if(selectedGOs.Contains(hitObject))
                {
                    continue;
                }
                else if (Input.GetKey(KeyCode.LeftShift)) //If the player is pressid leftShift, the selected GO must be added to selected
                {
                    //If selected is already on the selected list, it will be removed;
                    foreach (GameObject go in selectedGOs)
                    {
                        if (go.Equals(hitObject))
                        {
                            selectedGOs.Remove(hitObject); //Remove object from selected
                            return;
                        }
                    }
                    SelectGameObject(hitObject); //Add object to selected
                    return;
                }
                else //If the player is not pressing LeftShift, the list of selected object will be cleared and the selected will be added
                {
                    ClearSelectedGOList();
                    SelectGameObject(hitObject);
                    return;
                }
            }
        }

        ClearSelectedGOList();

    }

    private void SelectGameObject(GameObject gameObject)
    {
        selectedGOs.Add(gameObject);
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