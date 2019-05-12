using Imperium.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CombatStatsCanvasController : MonoBehaviour
{
    public Vector3 combatCanvasPositionOffset;
    public GameObject combatCanvasPrebab;
    public float combatCanvasScale;
    public CombatStats combatStats;
    public Image hp;
    public Image shields;
    private bool active;
    private bool mouseOver;

    public void SetActive(bool active)
    {
        this.active = active;
        hp.gameObject.transform.parent.gameObject.SetActive(active);
        shields.gameObject.transform.parent.gameObject.SetActive(active);

        if(active)
        {
            combatStats.AddObserver(OnStatsUpdate);
            UpdateFill();
        }
        else
        {
            combatStats.AddObserver(OnStatsUpdate);
        }
    }
    void OnStatsUpdate(CombatStats combatStats, int hp, int maxHP, int shields, int maxShields, int shieldRegen, float fieldOfView)
    {
        UpdateFill();
    }



    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;

        if (!ObjectSelector.Instance.selectedGOs.Contains(gameObject))
        {
            SetActive(false);
        }
    }

    private void Start()
    {
        GameObject combatCanvasGO = Instantiate(combatCanvasPrebab, transform.position, combatCanvasPrebab.transform.rotation, transform);
        combatCanvasGO.transform.localScale = new Vector3(combatCanvasGO.transform.localScale.x * combatCanvasScale,
               combatCanvasGO.transform.localScale.y * combatCanvasScale, combatCanvasGO.transform.localScale.z * combatCanvasScale);

        combatCanvasGO.transform.localPosition = combatCanvasPositionOffset;
        

        combatStats = gameObject.GetComponent<ICombatable>().CombatStats;

        CombatStatsCanvas combatCanvas = combatCanvasGO.GetComponent<CombatStatsCanvas>();

        hp = combatCanvas.hp.GetComponentInChildren<Image>();
        shields = combatCanvas.shields.GetComponentInChildren<Image>();
        SetActive(false);
    }

    private void UpdateFill()
    {
        hp.fillAmount = (float)combatStats.HP / combatStats.MaxHP;
        shields.fillAmount = (float)combatStats.Shields / combatStats.MaxShields;
    }

    private void Update()
    {

        if (MapObjecsRenderingController.Instance.visibleObjects.Contains(gameObject) && (ObjectSelector.Instance.selectedGOs.Contains(gameObject) || mouseOver))
        {
            if (!active)
            {
                SetActive(true);
            }
        }
        else
        {
            if (active)
            {
                SetActive(false);
            }
        }
    }
}