using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSShower : MonoBehaviour {

	
    private Text text;

	void Start () {
		text = GetComponent<Text>();
	}
	
	
	void Update () {
		float fps = 1f/Time.deltaTime;

        text.text = fps.ToString("0");
	}
}
