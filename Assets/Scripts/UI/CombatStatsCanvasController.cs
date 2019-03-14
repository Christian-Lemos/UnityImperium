using Imperium.Combat;
using UnityEngine;
using UnityEngine.UI;

public class CombatStatsCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject combatCanvasPrebab;

    [SerializeField]
    private float combatCanvasScale;

    [SerializeField]
    private Vector3 combatCanvasPositionOffset;

    [SerializeField]
    private CombatStats combatStats;

    [SerializeField]
    private Image hp;
    [SerializeField]
    private Image shields;


    private bool active;

    public void SetActive(bool active)
    {
        this.active = active;

        hp.gameObject.transform.parent.gameObject.SetActive(active);
        shields.gameObject.transform.parent.gameObject.SetActive(active);
    }

    private void Start()
    {
        GameObject combatCanvasGO = Instantiate(combatCanvasPrebab, transform.position, combatCanvasPrebab.transform.rotation, transform);
        combatCanvasGO.transform.localScale = new Vector3(combatCanvasGO.transform.localScale.x * combatCanvasScale,
               combatCanvasGO.transform.localScale.y * combatCanvasScale, combatCanvasGO.transform.localScale.z * combatCanvasScale);

        combatCanvasGO.transform.localPosition = combatCanvasPositionOffset;


        combatStats = this.gameObject.GetComponent<MapObjectCombatter>().combatStats;

        CombatStatsCanvas combatCanvas = combatCanvasGO.GetComponent<CombatStatsCanvas>();

        hp = combatCanvas.hp.GetComponentInChildren<Image>();
        shields = combatCanvas.shields.GetComponentInChildren<Image>();
        SetActive(false);
    }

    private void Update()
    {
        if(active)
        {
            hp.fillAmount = (float)combatStats.HP / combatStats.maxHP;
            shields.fillAmount = (float)combatStats.Shields / combatStats.maxShields;
        }
    }

    private void OnMouseOver()
    {
        SetActive(true);
        
    }
    private void OnMouseExit()
    {
        if(!MouseCommandsController.Instance.selectedGOs.Contains(this.gameObject))
        {
            SetActive(false);
        }
    }


}