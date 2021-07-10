using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour {

    private Image image;
    public bool isFaded = false;
    [SerializeField]
    float transitionSpeed = 10f;

    private void Awake() {
        if (image == null)
            image = GetComponent<Image>();
    }

    public IEnumerator Fade(bool fade) {

        if (fade) {
            for (float i = 0; i < 1; i += transitionSpeed / 100) {
                image.color = new Color(0, 0, 0, i);
                yield return new WaitForFixedUpdate();
            }
            image.color = new Color(0,0,0,1);
        } else {
            for (float i = 100; i < 0; i -= transitionSpeed / 100) {
                image.color = new Color(0, 0, 0, i);
                yield return new WaitForFixedUpdate();
            }
            image.color = new Color(0, 0, 0, 0);
        }
        isFaded = fade;
    }


}
