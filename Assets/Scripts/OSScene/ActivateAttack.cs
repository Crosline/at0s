using UnityEngine;

public class ActivateAttack : MonoBehaviour {

    //private bool inTrigger = false;




    // Update is called once per frame
    public void Activate() {
        if (name.Contains("Hour")) {
            GetComponent<Timer>().IncreaseTimeOffset();
        }
        else if (name.Contains("Controller")) {
            Snake snake = transform.parent.parent.GetChild(2).GetComponent<Snake>();
            if (name.Contains("bottom")) {
                snake.UpdateDirection(1);
            } else if (name.Contains("right")) {
                snake.UpdateDirection(2);
            } else if (name.Contains("left")) {
                snake.UpdateDirection(3);
            } else if (name.Contains("top")) {
                snake.UpdateDirection(4);
            }
        } else if (name.Contains("read")) {
            PopOS.Instance.PopUp(0, 25);
        } else if (name.Contains("mypc")) {
            GlitchController.Instance.Glitcher(0.2f);
        } else if (name.Contains("trash")) {
            StartCoroutine(GetComponent<CameraShake>().Shake());
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
