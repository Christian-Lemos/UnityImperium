using Imperium.Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCanvasController : MonoBehaviour 
{ 
    public GameObject asteroidCanvasGO;
    public AsteroidCanvas asteroidCanvas;
    public bool mouseOver;
    void Start()
    {
        
        asteroidCanvas = asteroidCanvasGO.GetComponentInChildren<AsteroidCanvas>(true);

        AsteroidController asteroidController = GetComponent<AsteroidController>();
        if(asteroidController != null)
        {
            asteroidCanvas.resourceQuantity = asteroidController.ResourceQuantity;
            asteroidCanvas.resource = new Resource(asteroidController.resourceType);

        }
        else
        {
            DummyAsteroid dummyAsteroid = GetComponent<DummyAsteroid>();
            if(dummyAsteroid != null)
            {
                asteroidCanvas.resourceQuantity = dummyAsteroid.resourceQuantity;
                asteroidCanvas.resource = new Resource(dummyAsteroid.resourceType);
            }
        }

        asteroidCanvas.gameObject.SetActive(true);


    }

    private int GetResourceQuantity()
    {
        AsteroidController asteroidController = GetComponent<AsteroidController>();
        if(asteroidController != null)
        {
            return asteroidController.ResourceQuantity;
        }
        else
        {
            DummyAsteroid dummyAsteroid = GetComponent<DummyAsteroid>();
            if(dummyAsteroid != null)
            {
               return dummyAsteroid.resourceQuantity;
            }
        }
        return 0;
    }


    private void Update()
    {
        if(mouseOver)
        {
            asteroidCanvas.text.text = GetResourceQuantity().ToString();
            //asteroidCanvasGO.transform.LookAt(Camera.main.transform);
           // asteroidCanvasGO.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    private void OnMouseOver()
    {
        mouseOver = true;
        asteroidCanvas.resourceTypeImageGO.transform.parent.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        mouseOver = false;
        asteroidCanvas.resourceTypeImageGO.transform.parent.gameObject.SetActive(false);
    }

}
