using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float timeOffset = 0f;

    private TextMesh textMesh;

    // Start is called before the first frame update
    void Awake() {
        StopAllCoroutines();
        textMesh = GetComponent<TextMesh>();

        StartCoroutine(UpdateTime());

    }

    private IEnumerator UpdateTime() {
        while (true) {

            UpdateText();
            yield return new WaitForSecondsRealtime(60f);
        }
    }


    private void UpdateText() {
        textMesh.text = System.DateTime.Now.ToString("HH:mm");
    }
}
