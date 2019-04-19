using Imperium;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FleetCommander : MonoBehaviour
{
    private int selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Map) | (1 << (int)ObjectLayers.Station) | (1 << (int)ObjectLayers.Asteroid);
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1))
            {
                List<GameObject> selectedGOs = ObjectSelector.Instance.selectedGOs;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, selectLayer))
                {
                    GameObject selected = hit.collider.gameObject;
                    if(selected.layer != (int)ObjectLayers.Map && selected.GetComponent<MapObject>() == null)
                    {
                        return;
                    }
                    switch (selected.layer)
                    {
                        case (int)ObjectLayers.Map:
                            //Moves all the selected ships
                            foreach (GameObject go in selectedGOs)
                            {
                                if (go == null)
                                {
                                    continue;
                                }
                                ShipController shipController = go.GetComponent<ShipController>();

                                if (shipController != null)
                                {
                                    shipController.MoveToPosition(hit.point, 1f, !Input.GetKey(KeyCode.LeftShift), false);
                                }
                            }
                            break;

                        case (int)ObjectLayers.Ship:
                            int selectedPlayer = PlayerDatabase.Instance.GetObjectPlayer(selected);
                            foreach (GameObject go in selectedGOs)
                            {
                                if (!PlayerDatabase.Instance.IsFromPlayer(go, selectedPlayer))
                                {
                                    go.GetComponent<ShipController>().AttackTarget(selected, !Input.GetKey(KeyCode.LeftShift), false);
                                }
                            }
                            break;

                        case (int)ObjectLayers.Station:
                            StationController stationController = selected.GetComponent<StationController>();

                            if (stationController.Constructed == false)
                            {
                                foreach (GameObject go in selectedGOs)
                                {
                                    if (go.GetComponent<StationConstructor>() != null)
                                    {
                                        go.GetComponent<ShipController>().BuildStation(selected, !Input.GetKey(KeyCode.LeftShift), false);
                                    }
                                }
                            }
                            break;

                        case (int)ObjectLayers.Asteroid:

                            foreach (GameObject go in selectedGOs)
                            {
                                ShipController shipController = go.GetComponent<ShipController>();
                                if (shipController != null && go.GetComponent<MineController>() != null)
                                {
                                    shipController.MineAsteroid(selected, !Input.GetKey(KeyCode.LeftShift));
                                }
                            }
                            break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            List<GameObject> selectedGOs = ObjectSelector.Instance.selectedGOs;
            foreach (GameObject gameObject in selectedGOs)
            {
                Destroy(gameObject);
            }
            selectedGOs.Clear();
        }
    }
}