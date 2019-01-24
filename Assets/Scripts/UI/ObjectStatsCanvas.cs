using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectStatsCanvas : MonoBehaviour {

    [SerializeField]
    private GameObject HPSliderGO;
    [SerializeField]
    private GameObject ShieldSliderGO;

    public Slider HpSlider { get; private set; }
    public Slider ShieldSlider { get; private set; }

    void Start()
    {
        HpSlider = HPSliderGO.GetComponent<Slider>();
        ShieldSlider = ShieldSliderGO.GetComponent<Slider>();
    }
}
