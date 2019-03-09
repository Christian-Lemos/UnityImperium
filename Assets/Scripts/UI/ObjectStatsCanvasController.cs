using Imperium.Combat;
using Imperium.MapObjects;
using UnityEngine;
public class ObjectStatsCanvasController : MonoBehaviour {

    
    private static GameObject objectSlidersPrefab;

    private new Camera camera;
    private GameObject objectCanvasGO;
    private MapObject mapObject;
    private ObjectStatsCanvas objectStatsCanvas;

    private CombatStats combatStats;
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
        combatStats = this.gameObject.transform.parent.gameObject.GetComponent<MapObjectCombatter>().combatStats;
        
        objectCanvasGO = Instantiate(objectSlidersPrefab, this.gameObject.transform);
        objectStatsCanvas = objectCanvasGO.GetComponent<ObjectStatsCanvas>();
        camera = Camera.main;
    }

    
    private void Update () {

        //Try catch is used to prevent errors if the objectController.stats are still not set
        try
        {
            objectStatsCanvas.HpSlider.value = (combatStats.HP * 100) / combatStats.maxHP;
            objectStatsCanvas.ShieldSlider.value = (combatStats.Shields * 100) / combatStats.maxShields;
        }
        catch
        {
            
        }
        
    }
}
