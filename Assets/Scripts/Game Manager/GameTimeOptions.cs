using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeOptions : MonoBehaviour {

    public bool paused;
	public float masterSpeed;

    public static GameTimeOptions Instance;

    private void Awake()
    {
        Instance = this;
    }
}
