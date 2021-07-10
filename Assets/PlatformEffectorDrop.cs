using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEffectorDrop : MonoBehaviour
{
    private float waitTime = 0.3f;

    private PlatformEffector2D effector;

    private void Start() {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) {
            effector.rotationalOffset = 0;
        }
        if (Input.GetButton("Crouch")) {
            effector.rotationalOffset = 180;
            waitTime = 0.3f;

        }

        if (waitTime <= 0 && effector.rotationalOffset == 180) {
            effector.rotationalOffset = 0;
            waitTime = 0.3f;
        } else if (waitTime >= 0) {
            waitTime -= Time.deltaTime;
        }

    }
}
