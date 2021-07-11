using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour {

    public int timeOffset = 0;

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
            yield return new WaitForSecondsRealtime(10f);
        }
    }


    private void UpdateText() {
        textMesh.text = (System.DateTime.Now.AddHours(timeOffset)).ToString("HH:mm");
    }

    public void IncreaseTimeOffset() {

        timeOffset++;
        if (timeOffset > 3) {
            timeOffset += 3;
        } else if (timeOffset > 10) {
            timeOffset += 10;
        } else if (timeOffset > 24) {
            timeOffset = 0;


            // FINISH THE UPLOAD HERE BRO



        } else {
            timeOffset++;
        }

        UpdateText();
        //UPDATE FOLDER IF ENABLED
    }
}
