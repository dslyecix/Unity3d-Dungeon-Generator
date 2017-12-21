using UnityEngine;
 
public class CameraController : MonoBehaviour {
 
    public float movementSpeed;
 
    void Update () {
        movementSpeed = 1f;

        if ( Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 0.5f;
        }

        transform.position = transform.position + (transform.right * Input.GetAxisRaw("Horizontal") * movementSpeed) + (transform.forward * Input.GetAxisRaw("Vertical") * movementSpeed) + (transform.up * Input.GetAxisRaw("Depth") * movementSpeed / 3f);
        transform.eulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0f);
    }
}
 