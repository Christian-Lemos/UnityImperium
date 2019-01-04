using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipCanvasController : MonoBehaviour {

    [SerializeField]
    private GameObject shipCanvasPrefab;
    private ShipController shipController;

    [SerializeField]
    private GameObject shipCanvasGO;
    [SerializeField]
    private ShipCanvas shipCanvas;

    private new Camera camera;

    private void Start()
    {
        shipController = this.gameObject.transform.parent.gameObject.GetComponent<ShipController>();
        shipCanvasGO = Instantiate(shipCanvasPrefab, this.gameObject.transform);
        shipCanvas = shipCanvasGO.GetComponent<ShipCanvas>();
        camera = Camera.main;
    }

    
    private void Update () {

        try
        {
            shipCanvas.HpSlider.value = (shipController.Ship.shipStats.HP * 100) / shipController.Ship.shipStats.MaxHP;
            shipCanvas.ShieldSlider.value = (shipController.Ship.shipStats.Shields * 100) / shipController.Ship.shipStats.MaxShields;
        }
        catch
        {
            
        }
        
    }

    private void LateUpdate()
    {
        shipCanvasGO.transform.LookAt(camera.transform);
        shipCanvasGO.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
    }

}
