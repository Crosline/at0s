using UnityEngine;

public class InternetExplorer : MonoBehaviour {

    public GameObject[] errors;

    public GameObject gizliDosya;


    private int errorCount = 0;

    public void IncreaseError() {
        if (errorCount >= errors.Length) {
            if (gizliDosya != null)
                gizliDosya.SetActive(true);
            transform.parent.GetComponent<Animator>().Play("OSClose");


            foreach (GameObject go in errors) {
                go.SetActive(false);
            }
            errorCount = 0;


            StartCoroutine(
            FindObjectOfType<CrosshairManager>().RestartOs());
            return;
        }

        errors[errorCount].SetActive(true);
        errorCount++;

    }

}
