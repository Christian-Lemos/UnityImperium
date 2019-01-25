using Imperium;
using Imperium.Economy;
using Imperium.Enum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles commands set by mouse
/// </summary>
public class MouseCommandsController : MonoBehaviour
{
    /// <summary>
    /// This is the list of selected game objects
    /// </summary>
    [SerializeField]
    private List<GameObject> selectedGOs;

    public GameObject selectPanel;
    public GameObject constructionSection;

    [SerializeField]
    public GameObject constructionButtonPrefab;

    public int player;

    private float constructionButtonPrefabOriginalPosX;
    private float constructionButtonPrefabWidth;

    private int selectLayer;

    private PlayerDatabase playerDatabase;

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

    private PossibleStationConstruction possibleStation;

    private void Start()
    {
        selectedGOs = new List<GameObject>();
        selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Map) | (1 << (int)ObjectLayers.Station);
        playerDatabase = PlayerDatabase.Instance;

        RectTransform rectTransform = constructionButtonPrefab.GetComponent<RectTransform>();

        constructionButtonPrefabWidth = rectTransform.sizeDelta.x;
        constructionButtonPrefabOriginalPosX = rectTransform.position.x;
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
                            ShipController shipController = go.GetComponent<ShipController>();

                            if (shipController != null)
                            {
                                shipController.MoveToPosition(hit.point, 1f, !Input.GetKey(KeyCode.LeftShift));
                            }
                        }
                        break;

                    case (int)ObjectLayers.Ship:
                        int selectedPlayer = playerDatabase.GetObjectPlayer(selected);
                        foreach (GameObject go in selectedGOs)
                        {
                            if (!playerDatabase.IsFromPlayer(go, selectedPlayer))
                            {
                                go.GetComponent<ShipController>().AttackTarget(selected, !Input.GetKey(KeyCode.LeftShift));
                            }
                        }
                        break;

                    case (int)ObjectLayers.Station:
                        StationController stationController = selected.GetComponent<StationController>();

                        if (stationController.constructed == false)
                        {
                            foreach (GameObject go in selectedGOs)
                            {
                                if (go.GetComponent<StationConstructor>() != null)
                                {
                                    go.GetComponent<ShipController>().BuildStation(selected, !Input.GetKey(KeyCode.LeftShift));
                                }
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
                    if (Input.GetKey(KeyCode.LeftShift)) //If the player is pressid leftShift, the selected GO must be added to selected
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
            ShowConstructionOptions();
        }
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

                    button.GetComponentInChildren<RawImage>().texture = ship.ShipIcon;
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

                        button.GetComponentInChildren<RawImage>().texture = station.StationIcon;
                        button.SetActive(true);

                        //SetShipConstructionButtonClickCallback(constructor, button, stationConstructions[i]);
                        SetStationConstructionButtonClickCallback(stationConstructor, button, stationConstructions[i]);
                    }
                }
            }
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
            station.GetComponentInChildren<ObjectStatsCanvasController>().gameObject.SetActive(false);
            Destroy(station.GetComponent<StationController>());

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
            possibleStation = new PossibleStationConstruction(station, stationConstruction, stationConstructor);
        });
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
                possibleStation.stationConstructor.BuildStation(possibleStation.stationConstruction.stationType, hit.point);
                Destroy(possibleStation.gameObject);
                possibleStation = null;
            }
            else if (Input.GetMouseButtonDown(1))//right click
            {
                Destroy(possibleStation.gameObject);
                possibleStation = null;
            }
        }
    }
}