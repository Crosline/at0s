using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    [Header("Settings")]
    public bool lockCursor = true;
    public float mouseSensitivity = 10f;

    public Transform playerBody;

    public bool isPitchClamp;


    public Vector2 pitchMinMax = new Vector2(-40, 85);

    private float xRotation = 0f;


    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void LateUpdate() {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        if (isPitchClamp)
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);


    }
}
