using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public float movementSpeed;
    public float minMovementSpeed;
    public float maxMovementSpeed;

    public float scrollSpeed;

    public float minScrollSpeed;
    public float maxScrollSpeed;

    public float movementPerAltitude;
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private float sSpeed;

    // Update is called once per frame
    private void LateUpdate()
    {
        speed = movementPerAltitude * transform.position.y * transform.position.y * movementSpeed;
        sSpeed =  movementPerAltitude * transform.position.y * transform.position.y * scrollSpeed;

        float v_axis = Input.GetAxis("Vertical");
        float h_axis = Input.GetAxis("Horizontal");
        float z_axis = Input.GetAxis("Mouse ScrollWheel");

        Move(h_axis, v_axis, z_axis);
    }

    public void Move(float horizontalInput, float verticalInput, float zInput)
    {
        float finalSpeed;
        if(speed < minMovementSpeed)
        {
            finalSpeed = minMovementSpeed;
        }
        else if(speed > maxMovementSpeed)
        {
            finalSpeed = maxMovementSpeed;
        }
        else
        {
            finalSpeed = speed;
        }

        float finalScrollSpeed;
        if(sSpeed < minScrollSpeed)
        {
            finalScrollSpeed = minScrollSpeed;
        }
        else if(sSpeed > maxScrollSpeed)
        {
            finalScrollSpeed = maxScrollSpeed;
        }
        else
        {
            finalScrollSpeed = sSpeed;
        }

        float x_movement = horizontalInput * finalSpeed * Time.deltaTime;
        float y_movement = verticalInput * finalSpeed * Time.deltaTime;
        float z_movement = zInput * finalScrollSpeed * Time.deltaTime;

        transform.Translate(x_movement, y_movement, z_movement);
    }
}