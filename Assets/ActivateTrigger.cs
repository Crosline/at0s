using UnityEngine;
using UnityEngine.UI;

public class ActivateTrigger : MonoBehaviour {

    public bool isUp = false;

    public MoveScreen ms;

    public ActivateTrigger at;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("2DPlayer")) {
            if (ms != null) {
                ms.Activate(this, at, isUp);
            }
        }
    }

}

