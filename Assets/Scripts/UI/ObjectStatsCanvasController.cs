using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectStatsCanvasController : MonoBehaviour {

    
    private static GameObject objectSlidersPrefab;

    private new Camera camera;
    private GameObject objectCanvasGO;
    private ObjectController objectController;
    private ObjectStatsCanvas objectStatsCanvas;
    private void LateUpdate()
    {
        objectCanvasGO.transform.LookAt(camera.transform);
        objectCanvasGO.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
    }

    private void Start()
    {
        if(objectSlidersPrefab == null)
        {
            objectSlidersPrefab = Resources.Load("UI"+ System.IO.Path.DirectorySeparatorChar +"ObjectStatsSliders") as GameObject;
        }

        objectController = this.gameObject.transform.parent.gameObject.GetComponent<ObjectController>();
        objectCanvasGO = Instantiate(objectSlidersPrefab, this.gameObject.transform);
        objectStatsCanvas = objectCanvasGO.GetComponent<ObjectStatsCanvas>();
        camera = Camera.main;
    }

    
    private void Update () {

        //Try catch is used to prevent errors if the objectController.stats are still not set
        try
        {
            objectStatsCanvas.HpSlider.value = (objectController.stats.HP * 100) / objectController.stats.MaxHP;
            objectStatsCanvas.ShieldSlider.value = (objectController.stats.Shields * 100) / objectController.stats.MaxShields;
        }
        catch
        {
            
        }
        
    }
}
