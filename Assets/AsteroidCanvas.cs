using Imperium.Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AsteroidCanvas : MonoBehaviour
{

    public GameObject resourceTypeImageGO;
    public GameObject resourceQuantityTextGO;
    
    public RawImage rawImage;
    public Text text;

    public int resourceQuantity;
    public Resource resource;

    private void Start()
    {
        rawImage = resourceTypeImageGO.GetComponent<RawImage>();

        rawImage.texture = resource.Icon;
        text = resourceQuantityTextGO.GetComponent<Text>();
        
   }


    private void LateUpdate()
    {
        //objectCanvasGO.transform.LookAt(camera.transform);
        //objectCanvasGO.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
    }

    /*private abstract class Executable
    {
        public RawImage rawImage;
        public Text text;
        public ResourceType resourceType;
        public Resource resource;

        protected Executable(RawImage rawImage, Text text, ResourceType resourceType, Resource resource)
        {
            this.rawImage = rawImage;
            this.text = text;
            this.resourceType = resourceType;
            this.resource = resource;
        }

        abstract void Execute();


    }

    private class DummyAsteroidExecutable : Executable
    {
        public DummyAsteroidExecutable(RawImage rawImage, Text text, ResourceType resourceType, Resource resource) : base(rawImage, text, resourceType)
        {
        }

        private override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }*/
}
