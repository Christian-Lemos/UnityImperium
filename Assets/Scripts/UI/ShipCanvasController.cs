using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipCanvasController : MonoBehaviour {

    [SerializeField]
    private GameObject shipCanvasPrefab;
    private ObjectController objectController;

    [SerializeField]
    private GameObject shipCanvasGO;
    [SerializeField]
    private ShipCanvas shipCanvas;

    private new Camera camera;

    private void Start()
    {
        objectController = this.gameObject.transform.parent.gameObject.GetComponent<ObjectController>();
        shipCanvasGO = Instantiate(shipCanvasPrefab, this.gameObject.transform);
        shipCanvas = shipCanvasGO.GetComponent<ShipCanvas>();
        camera = Camera.main;
    }

    
    private void Update () {

        try
        {
            shipCanvas.HpSlider.value = (objectController.stats.HP * 100) / objectController.stats.MaxHP;
            shipCanvas.ShieldSlider.value = (objectController.stats.Shields * 100) / objectController.stats.MaxShields;
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
