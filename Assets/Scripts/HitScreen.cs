using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScreen : MonoBehaviour
{
    public static HitScreen Instance;

    void Awake()
    {
        Instance = this;
    }

    Vector3 cameraInitialPos;
    public float shakeMagnetude = 0.05f, shakeTime = 0.2f;
    public Camera playerCam;

    public void ShakeIt()
    {
        cameraInitialPos = playerCam.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector3 cameraIntermediatePos = playerCam.transform.position;
        cameraIntermediatePos.x += cameraShakingOffsetX;
        cameraIntermediatePos.y += cameraShakingOffsetY;
        playerCam.transform.position = cameraIntermediatePos;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        playerCam.transform.position = cameraInitialPos;
    }
}
