using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
    public bool isRight = false;

    public bool isCorrect = false;

    public List<int> correctAngles;

    private void Start() {
        if (correctAngles.Count == 0) {
            isCorrect = true;
        }
    }

    public void RotatePipe() {
        if (isRight) {

            if (transform.localEulerAngles.z + 90 >= 360) {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 90 - 360);
            } else {

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 90);

            }
        } else {
            if (transform.localEulerAngles.z - 90 <= -360) {

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 90 + 360);
            } else {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 90);
            }
        }

        if (correctAngles.Contains((int)transform.localEulerAngles.z)) {
            Debug.Log("Correct!");
            isCorrect = true;
        }


    }
}
