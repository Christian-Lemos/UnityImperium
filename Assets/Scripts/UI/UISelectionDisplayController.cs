using System;
using Imperium;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionDisplayController : MonoBehaviour
{
    public GameObject nameNStats;
    public Text selectedText;
    public RawImage selectedImage;
    private List<GameObject> gameObjects;

    private Action action;

    public UISelectionStats shipStats;
    public UISelectionStats stationStats;

    private enum ShowType
    {
        Single, Mixed
    };

    private void OnSelectionChange(List<GameObject> gameObjects)
    {
        this.gameObjects = new List<GameObject>(gameObjects);
        if(gameObjects.Count == 1)
        {
            if(gameObjects[0].layer == (int)ObjectLayers.Ship)
            {
                ShipController shipController = gameObjects[0].GetComponent<ShipController>();
                selectedText.text = shipController.Ship.name;
                selectedImage.texture = shipController.Ship.shipIcon;

                action = UpdateShip;
                ShowSelectionStats(shipStats);
                SetVisibility(true);
            }
            else if(gameObjects[0].layer == (int)ObjectLayers.Station)
            {
                StationController stationController = gameObjects[0].GetComponent<StationController>();
                selectedText.text = stationController.Station.name;
                selectedImage.texture = stationController.Station.stationIcon;

                action = UpdateStation;
                ShowSelectionStats(stationStats);
                SetVisibility(true);
            }
            else
            {
                SetVisibility(false);
                action = null;
            }
        }
        else
        {
            action = null;
            SetVisibility(false);
        }
    }

    private void ShowSelectionStats(UISelectionStats uISelectionStats)
    {
        shipStats.gameObject.SetActive(false);
        stationStats.gameObject.SetActive(false);
        if(uISelectionStats != null)
        {
            uISelectionStats.gameObject.SetActive(true);
        }
    }


    private void SetVisibility(bool value)
    {
        selectedImage.gameObject.SetActive(value);
        selectedText.gameObject.SetActive(value);
        nameNStats.SetActive(value);
    }

    private void UpdateShip()
    {
        ShipController shipController = gameObjects[0].GetComponent<ShipController>();
    
        shipStats.hpText.text = shipController.Ship.combatStats.HP.ToString();
        shipStats.shieldText.text = shipController.Ship.combatStats.Shields.ToString();
        shipStats.speedText.text = shipController.Ship.speed.ToString();
    }

    private void UpdateStation()
    {
        StationController stationController = gameObjects[0].GetComponent<StationController>();

        stationStats.hpText.text = stationController.Station.combatStats.HP.ToString();
        stationStats.shieldText.text = stationController.Station.combatStats.Shields.ToString();
        if(stationController.constructionProgress < 100)
        {
            stationStats.constructionText.text = stationController.constructionProgress.ToString() + "%";
            stationStats.constructionImage.gameObject.SetActive(true);
            stationStats.constructionText.gameObject.SetActive(true);
        }
        else
        {
            stationStats.constructionImage.gameObject.SetActive(false);
            stationStats.constructionText.gameObject.SetActive(false);
        }
        
    }

    private void Start()
    {
        ObjectSelector.Instance.AddSelectionObserver(OnSelectionChange);
    }

    private void Update()
    {
        if(action != null)
        {
            action.Invoke();
        }
    }
}