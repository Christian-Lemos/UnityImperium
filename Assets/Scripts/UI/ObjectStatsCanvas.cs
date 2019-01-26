using UnityEngine;
using UnityEngine.UI;

public class ObjectStatsCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject HPSliderGO;

    [SerializeField]
    private GameObject ShieldSliderGO;

    public Slider HpSlider { get; private set; }
    public Slider ShieldSlider { get; private set; }

    private void Start()
    {
        HpSlider = HPSliderGO.GetComponent<Slider>();
        ShieldSlider = ShieldSliderGO.GetComponent<Slider>();
    }
}