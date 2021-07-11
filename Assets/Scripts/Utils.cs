using UnityEngine;

public class Utils : MonoBehaviour {

    // Update is called once per frame
    public void SetTag(string newTag) {
        this.gameObject.tag = newTag;
    }


    public void StopAnimator() {
        GetComponent<Animator>().enabled = false;
    }
}
