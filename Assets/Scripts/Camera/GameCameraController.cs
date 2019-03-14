using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public float movementSpeed;
    public float scrollSpeed;

    // Update is called once per frame
    private void LateUpdate()
    {
        float v_axis = Input.GetAxis("Vertical");
        float h_axis = Input.GetAxis("Horizontal");
        float z_axis = Input.GetAxis("Mouse ScrollWheel");

        Move(h_axis, v_axis, z_axis);
    }

    public void Move(float horizontalInput, float verticalInput, float zInput)
    {
        float x_movement = horizontalInput * movementSpeed * Time.deltaTime;
        float y_movement = verticalInput * movementSpeed * Time.deltaTime;
        float z_movement = zInput * scrollSpeed * Time.deltaTime;

        transform.Translate(x_movement, y_movement, z_movement);
    }
}