using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float sensibilidad = 100;
    [SerializeField] Transform playerBody;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    float xRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;
        
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot,-90,90);

        transform.localRotation = Quaternion.Euler(xRot,0,0);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
