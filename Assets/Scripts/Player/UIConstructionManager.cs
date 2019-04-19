using Imperium;
using Imperium.Economy;
using Imperium.MapObjects;
using Imperium.Rendering;
using Imperium.Research;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIConstructionManager : MonoBehaviour
{
    public GameObject constructionButtonPrefab;
    public GameObject constructionSection;

    private float constructionButtonPrefabOriginalPosX;
    private float constructionButtonPrefabWidth;

    private PossibleStationConstruction possibleStation;

    private void OnSelectionChange(List<GameObject> selectedGOs)
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

            ResearchController researchController = selectedGOs[0].GetComponent<ResearchController>();

            if (researchController != null)
            {
                List<ResearchTree> researchTrees = researchController.researchTrees;
                for (int i = 0, j = 0; i < researchTrees.Count; i++)
                {
                    if (researchTrees[i].NextNode != null)
                    {
                        float buttonPositionX = (constructionButtonPrefabOriginalPosX - 10) + (constructionButtonPrefabWidth * j) + 10; //10 It's the offset between buttons

                        GameObject button = Instantiate(constructionButtonPrefab, constructionSection.transform);

                        RectTransform rectTransform = button.GetComponent<RectTransform>();

                        float yPosition = rectTransform.position.y - 550;
                        rectTransform.anchoredPosition3D = new Vector3(buttonPositionX, yPosition, rectTransform.localPosition.z);

                        //Debug.Log("( " + constructionButtonPrefabOriginalPosX + "- 10 )" + " + " + "( " + constructionButtonPrefabWidth + " * " + i + ") + 10 = " + buttonPositionX);

                        button.GetComponentInChildren<RawImage>().texture = researchTrees[i].NextNode.research.texture;
                        button.SetActive(true);

                        //SetShipConstructionButtonClickCallback(constructor, button, stationConstructions[i]);
                        SetResearchButtonCallback(researchController, researchTrees[i], button);
                        j++;
                    }
                }
            }
        }
    }

    private void SetResearchButtonCallback(ResearchController researchController, ResearchTree researchTree, GameObject button)
    {
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            researchController.DoResearch(researchTree.NextNode);
        });
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
            Vector3 spawnPosition = stationConstructor.gameObject.transform.position;

            GameObject stationPrefab = Spawner.Instance.dummyStationDictionary[stationConstruction.stationType];
            GameObject station = Instantiate(stationPrefab, spawnPosition, Quaternion.identity);

            if (possibleStation != null)
            {
                Destroy(possibleStation.gameObject);
            }

            station.SetActive(true);
            FogOfWarUtility.SetRendering(true, station);
            possibleStation = new PossibleStationConstruction(station, stationConstruction, stationConstructor);
        });
    }

    private void Start()
    {
        RectTransform rectTransform = constructionButtonPrefab.GetComponent<RectTransform>();
        constructionButtonPrefabWidth = rectTransform.sizeDelta.x;
        constructionButtonPrefabOriginalPosX = rectTransform.position.x;

        constructionSection = GameObject.FindObjectOfType<ConstructionSection>().gameObject;

        ObjectSelector.Instance.AddSelectionObserver(OnSelectionChange);
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
        if (!EventSystem.current.IsPointerOverGameObject() && possibleStation != null)
        {
            StationConstructionToCursor();
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