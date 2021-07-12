using UnityEngine;

public class InternetExplorer : MonoBehaviour {

    public GameObject[] errors;

    public GameObject gizliDosya;

    private bool done = false;


    private int errorCount = 0;

    public void IncreaseError() {
        if (done) return;
        if (errorCount >= errors.Length) {
            if (gizliDosya != null) {
                gizliDosya.SetActive(true);
                PlayerPrefs.SetInt("gizli2", 1);
            }
            DialogueTrigger.Instance.TriggerDialogue("Internet_End");
            transform.parent.GetComponent<Animator>().Play("OSClose");


            foreach (GameObject go in errors) {
                go.SetActive(false);
            }
            errorCount = 0;


            StartCoroutine(
            FindObjectOfType<CrosshairManager>().RestartOs());
            done = true;
            return;
        }

        errors[errorCount].SetActive(true);
        errorCount++;

    }

}
