using Imperium.Navigation;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraMovement cameraMovement;

    // Use this for initialization
    private void Start()
    {
        cameraMovement = new CameraMovement(gameObject.GetComponent<Transform>());
    }

    // Update is called once per frame
    private void Update()
    {
        float v_axis = Input.GetAxis("Vertical");
        float h_axis = Input.GetAxis("Horizontal");
        cameraMovement.Move(h_axis, v_axis);
    }
}