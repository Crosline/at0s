using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    [Header("Settings")]
    public bool lockCursor;
    public float mouseSensitivity = 10f;
    public float rotationSmoothTime = 8f;
    public Transform playerBody;

    public bool pitchLock = false;
    public bool isPitchClamp;


    public Vector2 pitchMinMax = new Vector2(-40, 85);

    private float yaw;
    private float pitch;

    private Vector3 currentRotation;



    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void LateUpdate() {

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

        if (!pitchLock) {
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            if (isPitchClamp) {
                pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            }
        }

        Vector3 p = new Vector3(pitch, yaw, 0);

        currentRotation = Vector3.Lerp(currentRotation, p, rotationSmoothTime * Time.deltaTime);

        transform.localEulerAngles = currentRotation;

        Vector3 e = transform.eulerAngles;
        e.x = playerBody.eulerAngles.x;
        playerBody.eulerAngles = e;
    }
}
