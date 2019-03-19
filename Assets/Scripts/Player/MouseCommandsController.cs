using Imperium;
using Imperium.Economy;
using Imperium.Rendering;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles commands set by mouse
/// </summary>
public class MouseCommandsController : MonoBehaviour
{
    public static MouseCommandsController Instance;

    [SerializeField]
    public GameObject constructionButtonPrefab;

    public GameObject constructionSection;

    public int player;

    /// <summary>
    /// This is the list of selected game objects
    /// </summary>
    public List<GameObject> selectedGOs = new List<GameObject>();

    public GameObject selectPanel;

    private float constructionButtonPrefabOriginalPosX;

    private float constructionButtonPrefabWidth;

    private PlayerDatabase playerDatabase;

    private PossibleStationConstruction possibleStation;
    private int selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Map) | (1 << (int)ObjectLayers.Station) | (1 << (int)ObjectLayers.Asteroid);

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

    /// <summary>
    /// This method handles fleet's commands like move selected this to position
    /// </summary>
    private void FleetCommand()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, selectLayer))
            {
                GameObject selected = hit.collider.gameObject;

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
                        int selectedPlayer = playerDatabase.GetObjectPlayer(selected);
                        foreach (GameObject go in selectedGOs)
                        {
                            if (!playerDatabase.IsFromPlayer(go, selectedPlayer))
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
                GameObject selected = hit.collider.gameObject;
                
                if (selected.layer == (int)ObjectLayers.Ship || selected.layer == (int)ObjectLayers.Station)
                {
                    if(!MapObjecsRenderingController.Instance.visibleObjects.Contains(selected))
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
            ShowConstructionOptions();
        }
    }

    private void SetShipConstructionButtonClickCallback(ShipConstructor constructor, GameObject button, ShipConstruction shipConstruction)
    {
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            constructor.BuildShip(shipConstruction.shipType);
        });
    }

    private void SetStationConstructionButtonClickCallback(StationConstructor stationConstructor, GameObject button, StationConstruction stationConstruction)
    {
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject stationPrefab = Spawner.Instance.true_station_associations[stationConstruction.stationType];

            Vector3 spawnPosition = stationConstructor.gameObject.transform.position;

            GameObject station = Instantiate(stationPrefab, spawnPosition, Quaternion.identity);


            
            station.GetComponent<MapObject>().enabled = false;
            station.GetComponent<StationController>().enabled = false;
            station.GetComponent<MapObjectCombatter>().enabled = false;
            station.GetComponent<CombatStatsCanvasController>().enabled = false;
            station.GetComponent<StationConstructionProgressCanvasController>().enabled = false;

            TurretController[] turretControllers = station.GetComponentsInChildren<TurretController>();
            foreach (TurretController turretController in turretControllers)
            {
                turretController.gameObject.SetActive(false);
            }

            station.layer = 0;
            if (possibleStation != null)
            {
                Destroy(possibleStation.gameObject);
            }

            station.SetActive(true);
            FogOfWarUtility.SetRendering(true, station);
            possibleStation = new PossibleStationConstruction(station, stationConstruction, stationConstructor);
        });
    }

    private void ShowConstructionOptions()
    {
        foreach (Transform child in constructionSection.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (selectedGOs.Count == 1)
        {
            ShipConstructor constructor = selectedGOs[0].GetComponent<ShipConstructor>();

            if (constructor != null)
            {
                List<ShipConstruction> shipConstructions = constructor.ShipConstructions;
                int size = shipConstructions.Count;

                for (int i = 0; i < size; i++)
                {
                    Ship ship = ShipFactory.getInstance().CreateShip(shipConstructions[i].shipType);
                    float buttonPositionX = (constructionButtonPrefabOriginalPosX - 10) + (constructionButtonPrefabWidth * i) + 10; //10 It's the offset between buttons

                    GameObject button = Instantiate(constructionButtonPrefab, constructionSection.transform);

                    RectTransform rectTransform = button.GetComponent<RectTransform>();

                    rectTransform.anchoredPosition3D = new Vector3(buttonPositionX, rectTransform.localPosition.y, rectTransform.localPosition.z);

                    //Debug.Log("( " + constructionButtonPrefabOriginalPosX + "- 10 )" + " + " + "( " + constructionButtonPrefabWidth + " * " + i + ") + 10 = " + buttonPositionX);

                    button.GetComponentInChildren<RawImage>().texture = ship.shipIcon;
                    button.SetActive(true);

                    SetShipConstructionButtonClickCallback(constructor, button, shipConstructions[i]);
                }
            }
            else
            {
                StationConstructor stationConstructor = selectedGOs[0].GetComponent<StationConstructor>();
                if (stationConstructor != null)
                {
                    List<StationConstruction> stationConstructions = stationConstructor.stationConstructions;
                    int size = stationConstructions.Count;
                    for (int i = 0; i < size; i++)
                    {
                        Station station = StationFactory.getInstance().CreateStation(stationConstructions[i].stationType);
                        float buttonPositionX = (constructionButtonPrefabOriginalPosX - 10) + (constructionButtonPrefabWidth * i) + 10; //10 It's the offset between buttons

                        GameObject button = Instantiate(constructionButtonPrefab, constructionSection.transform);

                        RectTransform rectTransform = button.GetComponent<RectTransform>();

                        rectTransform.anchoredPosition3D = new Vector3(buttonPositionX, rectTransform.localPosition.y, rectTransform.localPosition.z);

                        //Debug.Log("( " + constructionButtonPrefabOriginalPosX + "- 10 )" + " + " + "( " + constructionButtonPrefabWidth + " * " + i + ") + 10 = " + buttonPositionX);

                        button.GetComponentInChildren<RawImage>().texture = station.stationIcon;
                        button.SetActive(true);

                        //SetShipConstructionButtonClickCallback(constructor, button, stationConstructions[i]);
                        SetStationConstructionButtonClickCallback(stationConstructor, button, stationConstructions[i]);
                    }
                }
            }
        }
    }

    private void Start()
    {
        playerDatabase = PlayerDatabase.Instance;
        Instance = this;
        RectTransform rectTransform = constructionButtonPrefab.GetComponent<RectTransform>();

        constructionButtonPrefabWidth = rectTransform.sizeDelta.x;
        constructionButtonPrefabOriginalPosX = rectTransform.position.x;
    }

    private void StationConstructionToCursor()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, 1 << (int)ObjectLayers.Map))
        {
            possibleStation.gameObject.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))//Left click
            {
                Destroy(possibleStation.gameObject);

                GameObject station = possibleStation.stationConstructor.BuildStation(possibleStation.stationConstruction.stationType, hit.point);

                possibleStation.stationConstructor.GetComponent<ShipController>().BuildStation(station, !Input.GetKey(KeyCode.LeftShift), false);

                possibleStation = null;
            }
            else if (Input.GetMouseButtonDown(1))//right click
            {
                Destroy(possibleStation.gameObject);
                possibleStation = null;
            }
        }
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (possibleStation != null)
            {
                StationConstructionToCursor();
            }
            else
            {
                ObjectSelector(); //left click
                FleetCommand(); //right click
            }
        }
    }

    private class PossibleStationConstruction
    {
        public GameObject gameObject;
        public StationConstruction stationConstruction;
        public StationConstructor stationConstructor;

        public PossibleStationConstruction(GameObject gameObject, StationConstruction stationConstruction, StationConstructor stationConstructor)
        {
            this.gameObject = gameObject;
            this.stationConstruction = stationConstruction;
            this.stationConstructor = stationConstructor;
        }
    }
}