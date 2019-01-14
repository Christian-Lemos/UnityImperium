using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imperium.Enum;
using Imperium;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class handles commands set by mouse
/// </summary>
public class MouseCommandsController : MonoBehaviour {
    /// <summary>
    /// This is the list of selected game objects
    /// </summary>
    [SerializeField]
    private List<GameObject> selectedGOs;
    [SerializeField]
    private GameObject selectPanel;

    [SerializeField]
    private GameObject constructionSection;

    [SerializeField]
    private GameObject constructionButtonPrefab;


    private Spawner spawner;

    private float constructionButtonPrefabOriginalPosX;
    private float constructionButtonPrefabWidth;


    private int selectLayer;

    private PlayerDatabase playerDatabase;


	void Start () {
        selectedGOs = new List<GameObject>();
        selectLayer = (1 << (int)ObjectLayers.Ship) | (1 << (int)ObjectLayers.Map);
        playerDatabase = PlayerDatabase.INSTANCE;

        RectTransform rectTransform = constructionButtonPrefab.GetComponent<RectTransform>();

        constructionButtonPrefabWidth = rectTransform.sizeDelta.x;
        constructionButtonPrefabOriginalPosX = rectTransform.position.x;

        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();

    }
	
	void Update ()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            ObjectSelector(); //left click
            FleetCommand(); //right click
        }
        
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

            if (Physics.Raycast(ray, out hit, 1000f, selectLayer))
            {
                GameObject selected = (GameObject)hit.collider.gameObject;
                if(selected.layer == (int) ObjectLayers.Map) //If right clicked on map
                {
                    foreach (GameObject go in selectedGOs)
                    {
                        ShipController controller = go.GetComponent<ShipController>();
                        controller.MoveToPosition(hit.point, 1f);
     
                    }
                }
                else if(selected.layer == (int)ObjectLayers.Ship)
                {
                    int selectedPlayer = playerDatabase.GetObjectPlayer(selected);
                    foreach (GameObject go in selectedGOs)
                    {
                        if (!playerDatabase.IsFromPlayer(go, selectedPlayer))
                        {
                            go.GetComponent<ShipController>().AttackTarget(selected);
                        }
                        
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
                GameObject selected = hit.collider.gameObject;
                if (selected.layer == (int) ObjectLayers.Ship)
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
            Constructor constructor = selectedGOs[0].GetComponent<Constructor>();

            if(constructor != null)
            {
                List<Constructor.ShipConstruction> shipConstructions = constructor.ShipConstructions;

                int size = shipConstructions.Count;

                for(int i = 0; i < size; i++)
                {
                    Ship ship = ShipFactory.getInstance().CreateShip(shipConstructions[i].ConstructionType);
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
        }
        
    }

    private void SetShipConstructionButtonClickCallback(Constructor constructor, GameObject button, Constructor.ShipConstruction shipConstruction)
    {
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            constructor.BuildShip(shipConstruction.ConstructionType);
        });
    }
}
