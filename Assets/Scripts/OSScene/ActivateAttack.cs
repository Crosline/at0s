using UnityEngine;

public class ActivateAttack : MonoBehaviour {

    //private bool inTrigger = false;




    // Update is called once per frame
    public void Activate() {
        if (name.Contains("Hour")) {
            GetComponent<Timer>().IncreaseTimeOffset();
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && !inTrigger) {
            inTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("2DPlayer") && inTrigger) {
            inTrigger = false;
        }
    }*/
}
